﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EasySave {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EasySave.translation.strings", typeof(strings).Assembly);
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
        ///   Looks up a localized string similar to Give the name of the backup : .
        /// </summary>
        internal static string Ask_Backup_Name {
            get {
                return ResourceManager.GetString("Ask_Backup_Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Give the name of the backup you want to delete : .
        /// </summary>
        internal static string Ask_Backup_Name_Delete {
            get {
                return ResourceManager.GetString("Ask_Backup_Name_Delete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Give the name of the backup you want to rename : .
        /// </summary>
        internal static string Ask_Backup_Name_Rename {
            get {
                return ResourceManager.GetString("Ask_Backup_Name_Rename", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Give the new name of the backup :.
        /// </summary>
        internal static string Ask_Backup_New_Name_Rename {
            get {
                return ResourceManager.GetString("Ask_Backup_New_Name_Rename", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Give the source path directory : .
        /// </summary>
        internal static string Ask_Backup_Source_Path {
            get {
                return ResourceManager.GetString("Ask_Backup_Source_Path", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Give the target path directory : .
        /// </summary>
        internal static string Ask_Backup_Target_Path {
            get {
                return ResourceManager.GetString("Ask_Backup_Target_Path", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Give the type [d = differential / c =complete]: .
        /// </summary>
        internal static string Ask_Backup_Type {
            get {
                return ResourceManager.GetString("Ask_Backup_Type", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Which language do you want ? [fr/en] : .
        /// </summary>
        internal static string Ask_Language {
            get {
                return ResourceManager.GetString("Ask_Language", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please, enter a valid input.
        /// </summary>
        internal static string Ask_Valid_Input {
            get {
                return ResourceManager.GetString("Ask_Valid_Input", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Change the language.
        /// </summary>
        internal static string Change_Language {
            get {
                return ResourceManager.GetString("Change_Language", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please, enter a valid choice between 1 and 7.
        /// </summary>
        internal static string Console_Ask_Valid_Choice {
            get {
                return ResourceManager.GetString("Console_Ask_Valid_Choice", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Create a backup.
        /// </summary>
        internal static string Create_Backup {
            get {
                return ResourceManager.GetString("Create_Backup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Delete a backup.
        /// </summary>
        internal static string Delete_Backup {
            get {
                return ResourceManager.GetString("Delete_Backup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error : a backup with this name already exists.
        /// </summary>
        internal static string Error_Backup_Already_Exists {
            get {
                return ResourceManager.GetString("Error_Backup_Already_Exists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error : there is no backup with this name.
        /// </summary>
        internal static string Error_Backup_Not_Found {
            get {
                return ResourceManager.GetString("Error_Backup_Not_Found", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error : you can&apos;t rename you backup with the same name.
        /// </summary>
        internal static string Error_Rename_Same_Name {
            get {
                return ResourceManager.GetString("Error_Rename_Same_Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error : you have already 5 backups, please delete one and retry.
        /// </summary>
        internal static string Error_Too_Many_Backups {
            get {
                return ResourceManager.GetString("Error_Too_Many_Backups", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Execute all backups.
        /// </summary>
        internal static string Execute_All_Backups {
            get {
                return ResourceManager.GetString("Execute_All_Backups", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exit the application.
        /// </summary>
        internal static string Exit_App {
            get {
                return ResourceManager.GetString("Exit_App", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No backup found.
        /// </summary>
        internal static string Info_No_Backup {
            get {
                return ResourceManager.GetString("Info_No_Backup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Name.
        /// </summary>
        internal static string Name {
            get {
                return ResourceManager.GetString("Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rename a backup.
        /// </summary>
        internal static string Rename_Backup {
            get {
                return ResourceManager.GetString("Rename_Backup", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Display all backups.
        /// </summary>
        internal static string Show_Backups {
            get {
                return ResourceManager.GetString("Show_Backups", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Source path.
        /// </summary>
        internal static string Source_Path {
            get {
                return ResourceManager.GetString("Source_Path", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Success.
        /// </summary>
        internal static string Success {
            get {
                return ResourceManager.GetString("Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Target path.
        /// </summary>
        internal static string Target_Path {
            get {
                return ResourceManager.GetString("Target_Path", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Type : complete.
        /// </summary>
        internal static string Type_Complete {
            get {
                return ResourceManager.GetString("Type_Complete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Type : differential.
        /// </summary>
        internal static string Type_Differential {
            get {
                return ResourceManager.GetString("Type_Differential", resourceCulture);
            }
        }
    }
}
