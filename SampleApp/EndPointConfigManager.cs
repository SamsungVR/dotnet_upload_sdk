using Newtonsoft.Json.Linq;
using SDKLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SampleApp {

   public class EndPointConfigManager {

      private readonly List<EndPointConfig> mConfigList = new List<EndPointConfig>();
      private string mSelectedId = null;
      private readonly IFormatter mFormatter = new BinaryFormatter();

      public EndPointConfigManager() {
         loadJsonConfig(getCurrentCfgFile(), false);
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

      private static readonly string CFG_FILE_NAME = "vrsdkcfg.cfg";

      private string getDefaultCfgFile() {
         return Path.Combine(System.Windows.Forms.Application.StartupPath, CFG_FILE_NAME);
      }

      private string nullChkCfgFile(string cfgFile) {
         if (null == cfgFile || string.Empty.Equals(cfgFile)) {
            return getDefaultCfgFile();
         }
         return cfgFile;
      }

      public string getCurrentCfgFile() {
         return nullChkCfgFile(AppSettings.Default.cfgFileFullPath);
      }

      private static readonly string JSON_ITEMS = "items";
      private static readonly string CFG_ID = "id";
      private static readonly string CFG_ENDPOINT = "endpoint";
      private static readonly string CFG_API_KEY = "apikey";
      private static readonly string CFG_SELECTED = "selected";

      private static readonly string CFG_SSO_APP_ID = "ssoappid";
      private static readonly string CFG_SSO_APP_SECRET = "ssoappsecret";

      public bool loadJsonConfig(string cfgFile) {
         return loadJsonConfig(cfgFile, true);
      }

      public bool loadJsonConfig(string cfgFile, bool save) {
         Log.d(TAG, "Loading json config from : " + cfgFile);

         mConfigList.Clear();

         if (save) {
            AppSettings.Default.cfgFileFullPath = cfgFile;
            AppSettings.Default.Save();
         }

         JObject json = null;
         try {
            string text = System.IO.File.ReadAllText(cfgFile);
            json = JObject.Parse(text);
         } catch (Exception ex) {
            json = null;
         }
         if (null != json) {
            string selectedId = Util.jsonOpt<string>(json, CFG_SELECTED, null);
            JObject items = Util.jsonOpt<JObject>(json, JSON_ITEMS, null);
            if (null != items) {
               IEnumerator<KeyValuePair<string, JToken>> cfgItemPairsEnum = items.GetEnumerator();
               while (cfgItemPairsEnum.MoveNext()) {
                  KeyValuePair<string, JToken> cfgItemPair = cfgItemPairsEnum.Current;
                  JToken cfgItem = cfgItemPair.Value;
                  string id = cfgItem.Value<string>(CFG_ID);
                  string apiKey = cfgItem.Value<string>(CFG_API_KEY);
                  string endPoint = cfgItem.Value<string>(CFG_ENDPOINT);
                  string ssoAppId = cfgItem.Value<string>(CFG_SSO_APP_ID);
                  string ssoAppSecret = cfgItem.Value<string>(CFG_SSO_APP_SECRET);

                  if (null != id && id.Equals(selectedId)) {
                     mSelectedId = id;
                  }

                  EndPointConfig cfg = new EndPointConfig(id);
                  cfg.setUrl(endPoint);
                  cfg.setApiKey(apiKey);

                  mConfigList.Add(cfg);
               }
            }
         }
         onConfigChanged();
         return null != json;
      }

      public string saveJsonConfig(string cfgFile) {
         JObject result = new JObject();
         JObject items = new JObject();
         result.Add(JSON_ITEMS, items);
         string selectedId = mSelectedId;
         List<EndPointConfig> list = getList();
         string matchedSelectedId = null, firstId = null;
         foreach (EndPointConfig cfg in list) {
            string id = cfg.getId();
            if (null == firstId) {
               firstId = id;
            }
            if (id.Equals(selectedId)) {
               matchedSelectedId = id;
            }
            JObject item = new JObject(
               new JProperty(CFG_ID, id),
               new JProperty(CFG_ENDPOINT, cfg.getUrl()),
               new JProperty(CFG_API_KEY, cfg.getApiKey()));
            items.Add(id, item);
         }
         if (null == matchedSelectedId) {
            matchedSelectedId = firstId;
         }
         if (null != matchedSelectedId) {
            result.Add(CFG_SELECTED, matchedSelectedId);
         }
         try {
            System.IO.File.WriteAllText(cfgFile, result.ToString(Newtonsoft.Json.Formatting.Indented));
            AppSettings.Default.cfgFileFullPath = cfgFile;
            AppSettings.Default.Save();
            onConfigChanged();
            return null;
         } catch (Exception ex) {
            string msg = ex.ToString();
            SDKLib.Log.d(TAG, "Failed to save json config " + msg);
            return msg;
         }
      }


      private static readonly string TAG = Util.getLogTag(typeof(EndPointConfigManager));


      public List<EndPointConfig> getList() {
         return mConfigList;
      }

      private void onConfigChanged() {
         int len = mConfigList.Count;
         string matchedId = null, firstSelectedId = null;
         for (int i = 0; i < len; i += 1) {

            EndPointConfig config = mConfigList[i];
            string id = config.getId();
            if (null == firstSelectedId) {
               firstSelectedId = id;
            }
            if (null != id && id.Equals(mSelectedId)) {
               matchedId = id;
            }
         }
         if (null == matchedId) {
            matchedId = firstSelectedId;
         }
         mSelectedId = matchedId;
         if (null != mObservers) {
            mObservers(Event.CHANGED);
         }
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
         onConfigChanged();
         return true;
      }

      public void deleteConfig(string argId) {
         for (int i = mConfigList.Count - 1; i >= 0; i -= 1) {
            string id = mConfigList[i].getId();
            if (id.Equals(argId)) {
               mConfigList.RemoveAt(i);
               break;
            }
         }
         onConfigChanged();
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
         if (null == mSelectedId || string.Empty.Equals(mSelectedId)) {
            return null;
         }
         return getConfig(mSelectedId);
      }

      public void selectConfig(string argId) {
         if (null != argId && argId.Equals(mSelectedId)) {
            return;
         }
         mSelectedId = argId;
         onConfigChanged();
      }

   }

}
