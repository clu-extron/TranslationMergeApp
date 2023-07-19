using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TranslationMergeApp
{
    public class ResxReaderTest
    {
        public static void ReadTest(string sourceFilePath, string destinationFilePath)
        {
            using (ResXResourceWriter destWriter = new ResXResourceWriter(destinationFilePath))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(sourceFilePath);

                XmlNodeList dataNodes = doc.SelectNodes("//root/data");
                int count = 0;
                foreach (XmlNode dataNode in dataNodes)
                {
                    string key = dataNode.Attributes["name"].Value;
                    XmlNode valueNode = dataNode.SelectSingleNode("value");
                    count++;
                    Console.WriteLine(count + ". " + key);
                }
            }
            
        }
    }
}
