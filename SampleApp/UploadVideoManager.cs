using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SampleApp {

   class UploadVideoManager {

      internal abstract class UploadItem {

         protected JObject mJObject;

         internal UploadItem(JObject jObject) {
            mJObject = jObject;
         }

         internal UploadItem() {
            mJObject = new JObject();
            setAttr("id", Guid.NewGuid().ToString());
         }

         internal JObject getJObject() {
            return mJObject;
         }

         protected void setAttr(String attr, String value) {
            mJObject.Add(attr, new JValue(value));
         }

         protected String getAttr(String attr) {
            JToken value;
            if (mJObject.TryGetValue(attr, out value)) {
               return value.ToString();
            }
            return null;
         }

         protected String getAsString(params String[] fields) {
            String result = null;
            foreach (String field in fields) {
               if (null == result) {
                  result = getAttr(field);
               } else {
                  result += ", " + getAttr(field);
               }
            }
            return result;
         }

         protected bool matchesString(String str, int count, int idIndex) {
            String[] parts = str.Split(',');
            if (count != parts.Length) {
               return false;
            }
            String id = parts[idIndex].Trim();
            return getAttr("id").Equals(id);
         }

         internal abstract String getAsString();
         internal abstract bool matchesString(String str);
      }


      internal class PendingUploadItem : UploadItem {

         internal PendingUploadItem(JObject item) : base(item) {
         }

         internal PendingUploadItem(String fileName, String permission, String title, String description) : base() {
            setAttr("filename", fileName);
            setAttr("permission", permission);
            setAttr("title", title);
            setAttr("description", description);
         }

         internal override String getAsString() {
            return getAsString("title", "filename", "id");
         }

         internal override bool matchesString(String str) {
            return matchesString(str, 3, 2);
         }

      }

      internal abstract class ItemsModel<T> where T : UploadItem {

         internal readonly List<T> mItems = new List<T>();

         private int mSelectedIndex = -1;
         private ListBox mView = null;

         internal bool loadItemsFrom(String data) {
            if (String.Empty.Equals(data)) {
               return false;
            }
            mItems.Clear();
            try {
               JObject jObject = JObject.Parse(data);
               JArray jItems = (JArray)jObject.GetValue("items");
               for (int i = 0; i < jItems.Count; i += 1) {
                  JObject jItem = jItems.Value<JObject>(i);
                  T newItem = newItemFromJObject(jItem);
                  mItems.Add(newItem);
               }
            } catch (Exception ex) {
               return false;
            }
            resyncView();
            return true;
         }

         internal abstract T newItemFromJObject(JObject jObject);

         internal virtual void addItem(T item) {
            mItems.Add(item);
            resyncView();
         }

         internal void resyncView() {
            resyncView(true);
         }

         internal void resyncView(bool shouldSave) {
            if (shouldSave) {
               save();
            }
            if (null != mView) {
               mView.Items.Clear();
               foreach (T item in mItems) {
                  mView.Items.Add(item.getAsString());
               }
               if (mItems.Count < 1) {
                  mSelectedIndex = -1;
               } else {
                  if (mSelectedIndex < 0) {
                     mSelectedIndex = 0;
                  } else if (mSelectedIndex >= mItems.Count) {
                     mSelectedIndex = mItems.Count - 1;
                  }
                  mView.SelectedIndex = mSelectedIndex;
               }
            }
         }

         internal void removeAllItems() {
            mItems.Clear();
            resyncView();
         }

         internal void removeItem(T item) {
            if (mItems.Count > 0 && mItems.Remove(item)) {
               resyncView();
            }
         }

         internal void removeSelectedItem() {
            if (mItems.Count > 0 && mSelectedIndex >= 0 && mSelectedIndex < mItems.Count) {
               mItems.RemoveAt(mSelectedIndex);
               resyncView();
            }
         }

         internal void moveSelectedItemUp() {
            if (mItems.Count > 0 && mSelectedIndex > 0 && mSelectedIndex < mItems.Count) {
               T item = mItems[mSelectedIndex];
               mItems.RemoveAt(mSelectedIndex);
               mSelectedIndex -= 1;
               mItems.Insert(mSelectedIndex, item);
               resyncView();
            }
         }

         internal void moveSelectedItemDown() {
            if (mItems.Count > 0 && mSelectedIndex >= 0 && mSelectedIndex < mItems.Count - 1) {
               T item = mItems[mSelectedIndex];
               mItems.RemoveAt(mSelectedIndex);
               mSelectedIndex += 1;
               mItems.Insert(mSelectedIndex, item);
               resyncView();
            }
         }

         internal void setView(ListBox view) {

            if (null != mView) {
               mView.SelectedIndexChanged -= onSelectedIndexChanged;
            }
            mView = view;
            if (null != mView) {
               mView.SelectedIndexChanged += onSelectedIndexChanged;
               resyncView(false);
            }
            
         }

         internal void onSelectedIndexChanged(object sender, System.EventArgs e) {
            mSelectedIndex = mView.SelectedIndex;
         }

         internal String getAsString() {
            JArray jItems = new JArray();

            foreach (T item in mItems) {
               jItems.Add(item.getJObject());
            }
            JObject jObject = new JObject();
            jObject.Add("items", jItems);
            return jObject.ToString();
         }

         abstract internal void save();
      }

      internal class PendingUploadItemsModel : ItemsModel<PendingUploadItem> {

         internal PendingUploadItemsModel() : base() {
            loadItemsFrom(AppSettings.Default.pendingUploadData);
         }

         internal override PendingUploadItem newItemFromJObject(JObject jObject) {
            return new PendingUploadItem(jObject);
         }

         internal override void save() {
            AppSettings.Default.pendingUploadData = getAsString();
            AppSettings.Default.Save(); 
         }

         internal override void addItem(PendingUploadItem item) {
            if (UploadVideoManager.sUploadVideoManager.tryUploadImmediate(item)) {
               return;
            }
            base.addItem(item);
         }
      }

      private PendingUploadItem mActiveUpload = null;

      private readonly PendingUploadItemsModel mPendingUploads = new PendingUploadItemsModel();

      private UploadVideoManager() {
      }

      private ListBox mActiveUploadView = null;

      internal void setActiveUploadView(ListBox activeUploadView) {
         mActiveUploadView = activeUploadView;
         refreshActiveUploadView();
      }

      private void refreshActiveUploadView() {

      }

      private bool tryUploadImmediate(PendingUploadItem item) {
         return false;
      }

      internal PendingUploadItemsModel getPendingUploads() {
         return mPendingUploads;
      }

      private static UploadVideoManager sUploadVideoManager = null;

      public static UploadVideoManager getInstance() {
         if (null == sUploadVideoManager) {
            sUploadVideoManager = new UploadVideoManager();
         }
         return sUploadVideoManager;
      }
   }




}
