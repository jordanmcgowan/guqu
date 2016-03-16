using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guqu.Models
{
    class JsonFileParser
    {
        public JsonFileParser()
        {

        }
        public Dictionary<string, string> retrieveValues(Dictionary<string, string> cd_service_TermDictionary, StreamReader fileStreamReader)
        {
            {
                Dictionary<string, string> cd_service_ValueDictionary = new Dictionary<string, string>();

                string jsonData = fileStreamReader.ReadToEnd();

                Dictionary<string, string> mdValues = new Dictionary<string, string>();
                if (jsonData != null)
                {
                    JObject googleMD = JObject.Parse(jsonData);
                    JToken curToken;
                    string mdValue;
                    foreach (KeyValuePair<string, string> entry in cd_service_TermDictionary)
                    {
                        //todo: should we use trygetvalue?
                        curToken = googleMD.GetValue(entry.Value);
                        //curToken holds the data from Jobject for a specific field.
                        mdValue = (string)Convert.ChangeType(curToken.ToString(), typeof(string));
                        //mdValue gets the value for that field 
                        cd_service_ValueDictionary.Add(entry.Key, mdValue);
                        //cd_google_Value_Dictionary holds the pairing from cd terms to the actual values.
                    }
                    //have all of the needed information to create a CD object.
                    return cd_service_ValueDictionary;
                }
                return null;
            }
        }
    }
    
}
