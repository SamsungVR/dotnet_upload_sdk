﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UILib {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class UILibSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static UILibSettings defaultInstance = ((UILibSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new UILibSettings())));
        
        public static UILibSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string selectedProvider {
            get {
                return ((string)(this["selectedProvider"]));
            }
            set {
                this["selectedProvider"] = value;
            }
        }
    }
}
