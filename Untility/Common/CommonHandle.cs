using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows;
using System.Xml;

namespace Untility
{
    public class CommonHandle
    {
        public CommonHandle()
        {

        }

        /// <summary>
        /// Add new key and value to appsettings
        /// </summary>
        /// <param name="newKey"></param>
        /// <param name="newValue"></param>
        public void AddConfigKey(string newKey, string newValue)
        {
            try
            {
                Configuration file = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                file.AppSettings.Settings.Add(newKey, newValue);

                file.Save(ConfigurationSaveMode.Modified);

                ConfigurationManager.RefreshSection("configuration");
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddConfigKey" + ex.ToString());
            }
        }

        /// <summary>
        /// Remove keys from SectionGroupName
        /// 
        /// </summary>
        /// <param name="SectionGroupName"></param>
        /// <param name="TagName"></param>
        public void RemoveKeys(string SectionGroupName, string TagName)
        {
            ConfigXmlDocument doc = new ConfigXmlDocument();
            Configuration file = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            doc.Load(file.FilePath);

            XmlNodeList nodeList = doc.GetElementsByTagName(TagName);

            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlAttribute att = nodeList[i].Attributes["key"];

                file.AppSettings.Settings.Remove(att.Name.ToString());
            }

            file.Save(ConfigurationSaveMode.Modified);

            System.Configuration.ConfigurationManager.RefreshSection("configuration");
        }
    }
}
