using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

using SettingsFile = Guqu.Models.SupportClasses.SettingsFile;

namespace Guqu.Models
{
    public class SettingsController
    {
        
        private readonly string SETTINGSFILEPATH = "../SETTINGFILEPATH.json";
        private SettingsFile settingsFile;
        public SettingsController()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            //settings exist
            if (File.Exists(SETTINGSFILEPATH))
            {
                string jsonCD = File.ReadAllText(SETTINGSFILEPATH);
                settingsFile = jsonSerializer.Deserialize<SettingsFile>(jsonCD);
            }
            else
            {
                //file does not exist, create it with default values
                //TODO: link this to a file with default values
                Boolean defaultSavePasswords = true;
                String defaultMDFP = "../GuquMetaDataStorage";
                settingsFile = new SettingsFile(defaultMDFP, defaultSavePasswords);
                var serializedJson = jsonSerializer.Serialize(settingsFile);
                File.WriteAllText(SETTINGSFILEPATH, serializedJson);
            }
        }
        public string getMetaDataFilePath()
        {
            return settingsFile.MetaDataFilePath;
        }
        public Boolean getIfSavePasswords()
        {
            return settingsFile.SavePasswords;
        }
        
    }
}
