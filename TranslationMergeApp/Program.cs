using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationMergeApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Merge Start");

            // Test Case 1
            //string sourceFile = "C:\\Learn\\Files\\Test\\Resources.resx";
            //string destinationFile = "C:\\Learn\\Files\\Test\\Resources.zh-Hans.resx";

            //ResxComparer.MergeResxFiles(sourceFile, destinationFile);

            // Test Case 2
            //string sourceFile = "C:\\Learn\\Files\\Test\\TLSDeskResources.json";
            //string destinationFile = "C:\\Learn\\Files\\Test\\TLSDeskResources{zh-Hant}.json";

            //JsonComparer.MergeJsonFiles(sourceFile, destinationFile);

            // Test Case 3
            string resxFileName = "C:\\Learn\\Files\\Test\\Resources.zh-Hans.resx";
            string jsonFileName = "C:\\projects\\signal\\TLPScheduler_Dev\\TLPConfSW\\Source\\Extron.RoomAgent.Main\\Resources\\Localization\\Languages\\Desks\\TLSDeskResources.es.json";
            //StringOperations.GetFileName(resxFileName);
            //StringOperations.GetFileName(jsonFileName);

            StringOperations.GetName(resxFileName);
            StringOperations.GetName(jsonFileName);

            StringOperations.GetFormat(resxFileName);
            StringOperations.GetFormat(jsonFileName);

            Console.WriteLine("Merge Fini");
        }
    }
}
