using Newtonsoft.Json.Linq;
using System.IO;

namespace TranslationMergeApp
{
    public class JsonComparer
    {
        public static void MergeJsonFiles(string sourcePath, string destinationPath)
        {
            // Read the contents of both JSON files
            string srcJson = File.ReadAllText(sourcePath);
            string destJson = File.ReadAllText(destinationPath);

            // Parse the JSON data into JObjects
            JObject srcData = JObject.Parse(srcJson);
            JObject destData = JObject.Parse(destJson);

            // Iterate over each property in the source JSON file
            foreach (var property in srcData.Properties())
            {
                // Check if the property is empty in the src JSON file
                if (string.IsNullOrEmpty((string)destData[property.Name]["string"]))
                {
                    destData[property.Name]["string"] = property.Value["string"];
                }
            }

            //Write the merged JSON data to the destination file
            File.WriteAllText(destinationPath, destData.ToString());
        }
    }
}
