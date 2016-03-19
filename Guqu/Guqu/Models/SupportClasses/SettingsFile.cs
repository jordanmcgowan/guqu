using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guqu.Models.SupportClasses
{
    [Serializable]
    class SettingsFile
    {
        //private string storeMetaDataFilePath;
        //private Boolean savePasswords;

        public SettingsFile(string storeMDFP, Boolean savePasswords)
        {
            MetaDataFilePath = storeMDFP;
            SavePasswords = savePasswords;
        }
        public SettingsFile()
        {

        }

        public string MetaDataFilePath { get; private set; }
        public bool SavePasswords { get; private set; }

        public static explicit operator SettingsFile(CommonDescriptor v)
        {
            throw new NotImplementedException();
        }
    }
}
