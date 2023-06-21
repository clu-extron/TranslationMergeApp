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

            string sourceFile = "C:\\Learn\\Files\\Test\\TLSDeskResources.json";
            string destinationFile = "C:\\Learn\\Files\\Test\\TLSDeskResources{zh-Hant}.json";

            JsonComparer.MergeJsonFiles(sourceFile, destinationFile);

            Console.WriteLine("Merge Fini");
        }
    }
}
