﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OfficeCommon.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.8.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("iiko")]
        public string IIKO_APPDATA_SUBPATH {
            get {
                return ((string)(this["IIKO_APPDATA_SUBPATH"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Rms")]
        public string RMS_EDITION_SUBPATH {
            get {
                return ((string)(this["RMS_EDITION_SUBPATH"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Chain")]
        public string CHAIN_EDITION_SUBPATH {
            get {
                return ((string)(this["CHAIN_EDITION_SUBPATH"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("back-log.log")]
        public string BACK_LOG_FILENAME {
            get {
                return ((string)(this["BACK_LOG_FILENAME"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("999999999.999999999")]
        public decimal MAX_DECIMAL_VALUE {
            get {
                return ((decimal)(this["MAX_DECIMAL_VALUE"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://{language}.help.iikosoftware.com/articles/#!iikooffice/{topic}")]
        public string HELP_URI {
            get {
                return ((string)(this["HELP_URI"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("259200000")]
        public int SERVICE_METHOD_TIMEOUT_MILLISECONDS {
            get {
                return ((int)(this["SERVICE_METHOD_TIMEOUT_MILLISECONDS"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10000")]
        public int GETTING_SERVER_INFO_TIMEOUT_MILLISECONDS {
            get {
                return ((int)(this["GETTING_SERVER_INFO_TIMEOUT_MILLISECONDS"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}://{1}:{2}{3}/get_server_info.jsp?encoding=UTF-8")]
        public string SERVER_INFO_PAGE_ADDRESS {
            get {
                return ((string)(this["SERVER_INFO_PAGE_ADDRESS"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Plugins")]
        public string PluginsDirectoryRelativePath {
            get {
                return ((string)(this["PluginsDirectoryRelativePath"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool IgnoreCleverenceDriverDriverExceptions {
            get {
                return ((bool)(this["IgnoreCleverenceDriverDriverExceptions"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("90")]
        public int LogFileAgeDays {
            get {
                return ((int)(this["LogFileAgeDays"]));
            }
        }
    }
}
