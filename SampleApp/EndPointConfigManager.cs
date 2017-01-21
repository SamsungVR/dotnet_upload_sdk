using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SampleApp {

   public class EndPointConfigManager {

      private readonly List<EndPointConfig> mConfigList = new List<EndPointConfig>();
      private readonly IFormatter mFormatter = new BinaryFormatter();

      public EndPointConfigManager() {
         loadConfigsFromSettings();
      }

      public enum Event {
         CHANGED
      }

      public delegate void Observer(Event ev);

      private Observer mObservers;

      public void addObserver(Observer observer) {
         mObservers += observer;
      }

      public void removeObserver(Observer observer) {
         mObservers -= observer;
      }
             
      private void loadConfigsFromSettings() {
         mConfigList.Clear();

         string savedConfig = AppSettings.Default.endPointConfig;
         byte[] asBytes;
         try {
            asBytes = Convert.FromBase64String(savedConfig);
         } catch (Exception) {
            return;
         }
         MemoryStream stream = new MemoryStream(asBytes);

         while (stream.Position < stream.Length) {
            EndPointConfig cfg;
            try {
               cfg = (EndPointConfig)mFormatter.Deserialize(stream);
            } catch (Exception) {
               break;
            }
            if (null != cfg) {
               mConfigList.Add(cfg);
            }
         }
         stream.Close();
         onConfigChanged();
      }

      public List<EndPointConfig> getList() {
         return mConfigList;
      }

      private void onConfigChanged() {
         int len = mConfigList.Count;
         string selectedId = AppSettings.Default.endPointSelectedId;

         for (int i = 0; i < len; i += 1) {
            EndPointConfig config = mConfigList[i];
            if (null == selectedId || selectedId.Trim().Length < 1) {
               selectedId = config.getId();
               AppSettings.Default.endPointSelectedId = selectedId;
               AppSettings.Default.Save();
            }
         }
         if (null != mObservers) {
            mObservers(Event.CHANGED);
         }
      }

      private void saveConfigsToSettings() {
         MemoryStream stream = new MemoryStream();
         int len = mConfigList.Count;
         bool selectedIdExists = false;
         string selectedId = AppSettings.Default.endPointSelectedId;
         for (int i = 0; i < len; i += 1) {
            EndPointConfig config = mConfigList[i];
            string id = config.getId();
            if (id.Equals(selectedId)) {
               selectedIdExists = true;
            }
            mFormatter.Serialize(stream, config);
            stream.Flush();
         }
         stream.Close();
         byte[] asBytes = stream.ToArray();
         string asString = Convert.ToBase64String(asBytes);
         if (!selectedIdExists) {
            AppSettings.Default.endPointSelectedId = string.Empty;
         }
         AppSettings.Default.endPointConfig = asString;
         AppSettings.Default.Save();
         onConfigChanged();
      }

      public bool addOrUpdateConfig(EndPointConfig config) {
         string id = config.getId();
         if (null == id) {
            return false;
         }
         EndPointConfig existing = getConfig(id);
         if (null == existing) {
            mConfigList.Add(config);
         } else {
            if (existing != config) {
               existing.setApiKey(config.getApiKey());
               existing.setUrl(config.getUrl());
            }
         }
         saveConfigsToSettings();
         return true;
      }

      public void deleteConfig(string argId) {
         for (int i = mConfigList.Count - 1; i >= 0; i -= 1) {
            string id = mConfigList[i].getId();
            if (id.Equals(argId)) {
               mConfigList.RemoveAt(i);
               saveConfigsToSettings();
               break;
            }
         }
      }

      public EndPointConfig getConfig(string argId) {
         int len = mConfigList.Count;
         for (int i = 0; i < len; i += 1) {
            EndPointConfig config = mConfigList[i];
            string id = config.getId();
            if (id.Equals(argId)) {
               return config;
            }
         }
         return null;
      }

      public EndPointConfig getSelectedConfig() {
         string id = AppSettings.Default.endPointSelectedId;
         if (null == id || 0 == id.Trim().Length) {
            return null;
         }
         return getConfig(id);
      }

      public void selectConfig(string argId) {
         string selectedId = AppSettings.Default.endPointSelectedId;
         if (argId.Equals(selectedId)) {
            return;
         }
         AppSettings.Default.endPointSelectedId = argId;
         AppSettings.Default.Save();
         onConfigChanged();
      }

   }

}
