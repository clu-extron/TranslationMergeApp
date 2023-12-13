using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TranslationMergeApp
{
    public class XlfComparer
    {
        public static void MergeXlfFiles(string sourceFilePath, string destinationFilePath)
        {
            XmlDocument srcDoc = new XmlDocument();
            XmlDocument destDoc = new XmlDocument();

            // Load the XLF file
            srcDoc.Load(sourceFilePath);
            destDoc.Load(destinationFilePath);

            // Get the root element
            XmlElement rootSrcDoc = srcDoc.DocumentElement;
            XmlElement rootDestDoc = destDoc.DocumentElement;

            // Iterate over the <trans-unit> elements
            XmlNodeList srcDataNodes = rootSrcDoc.GetElementsByTagName("trans-unit");
            XmlNodeList destDataNodes = rootDestDoc.GetElementsByTagName("trans-unit");

            foreach (XmlNode srcDataNode in srcDataNodes)
            {
                string srcKey = srcDataNode.Attributes["id"]?.InnerText;
                XmlNode srcValueNode = srcDataNode.ChildNodes[0];
                string srcValue = srcValueNode?.InnerText;

                foreach (XmlNode destDataNode in destDataNodes)
                {
                    string destKey = destDataNode.Attributes["id"]?.InnerText;
                    string destValue = destDataNode.ChildNodes[0]?.InnerText;

                    if (string.Equals(destKey, srcKey, StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(destValue))
                        {   
                            destDataNode.ChildNodes[0].InnerText = srcValue;   
                        }
                        break;
                    }
                }
            }
            destDoc.Save(destinationFilePath);
            Console.WriteLine("Resx files merged successfully!");
        }
    }
}
