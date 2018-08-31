﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GaraioLogParser.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("GaraioLogParser.Resources.Resource", typeof(Resource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid value per N Calls.
        /// </summary>
        internal static string InvalidValueCalls {
            get {
                return ResourceManager.GetString("InvalidValueCalls", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No value per N Calls.
        /// </summary>
        internal static string NoValueCalls {
            get {
                return ResourceManager.GetString("NoValueCalls", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No value per Client IP.
        /// </summary>
        internal static string NoValueClientIP {
            get {
                return ResourceManager.GetString("NoValueClientIP", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No value per FQDN.
        /// </summary>
        internal static string NoValueFQDN {
            get {
                return ResourceManager.GetString("NoValueFQDN", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Index out of range.
        /// </summary>
        internal static string RecordIndexOutOfRangeException {
            get {
                return ResourceManager.GetString("RecordIndexOutOfRangeException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This file is not the correct file format used by IIS..
        /// </summary>
        internal static string W3CIISFormatException {
            get {
                return ResourceManager.GetString("W3CIISFormatException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wrong filter requested.
        /// </summary>
        internal static string WrongFilterRequestedException {
            get {
                return ResourceManager.GetString("WrongFilterRequestedException", resourceCulture);
            }
        }
    }
}
