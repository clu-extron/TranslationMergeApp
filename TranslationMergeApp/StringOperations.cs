using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TranslationMergeApp
{
    public class StringOperations
    {
        public static string GetFileName(string directoryPath)
        {
            try
            {
                string fileName = Path.GetFileName(directoryPath);
                //Console.WriteLine("File Name: " + fileName);
                return fileName;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return null;
            }
        }

        public static void GetName(string directoryPath)
        {
            string fileName = GetFileName(directoryPath);
            try
            {
                string namePattern = @"^(.+?)\.";
                Match match = Regex.Match(fileName, namePattern);
                if (match.Success)
                {
                    string name = match.Groups[1].Value;
                    Console.WriteLine("File Name: " + name);
                }
                else
                {
                    Console.WriteLine("Unable to extract the name from the file name.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        public static void GetFormat(string directoryPath)
        {
            string fileName = GetFileName(directoryPath);
            try
            {
                string formatPattern = @"\.([^.]+)$";
                Match match = Regex.Match(fileName, formatPattern);
                if (match.Success)
                {
                    string format = match.Groups[1].Value;
                    Console.WriteLine("Format: " + format);
                }
                else
                {
                    Console.WriteLine("Unable to extract the format from the file name.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        public static void GetDir(string directoryPath)
        {
            try
            {
                // Use regular expressions to extract the path part
                string pathPattern = @"^(.*[\\/])";
                Match match = Regex.Match(directoryPath, pathPattern);

                if (match.Success)
                {
                    string path = match.Groups[1].Value;
                    Console.WriteLine("Path: " + path);
                }
                else
                {
                    Console.WriteLine("Unable to extract the path from the file path.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }
    }
}
