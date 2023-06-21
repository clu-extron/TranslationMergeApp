using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Resources;

public class ResxComparer
{
    public static void MergeResxFiles(string sourceFilePath, string destinationFilePath)
    {
        // Write the merged resources back to the destination file
        using (var destinationWriter = new ResXResourceWriter(destinationFilePath))
        {
            // Read the source .resx file
            using (var sourceReader = new ResXResourceReader(sourceFilePath))
            {
                sourceReader.UseResXDataNodes = true;
                // Read the destination .resx file
                using (var destinationReader = new ResXResourceReader(destinationFilePath))
                {
                    IDictionaryEnumerator srcEnumerator = sourceReader.GetEnumerator();
                    while (srcEnumerator.MoveNext())
                    {
                        string key = srcEnumerator.Key.ToString();
                        object value = srcEnumerator.Value;

                        IDictionaryEnumerator destEnumerator = destinationReader.GetEnumerator();
                        while (destEnumerator.MoveNext())
                        {
                            string destKey = destEnumerator.Key.ToString();
                            if (string.Equals(destKey, key, StringComparison.OrdinalIgnoreCase))
                            {
                                string comment = "";
                                ResXDataNode dataNode = (ResXDataNode)srcEnumerator.Value;
                                if (dataNode != null && dataNode.Comment != null)
                                {
                                    comment = dataNode.Comment;
                                }
                                if (string.IsNullOrEmpty((string)destEnumerator.Value))
                                {
                                    if (!string.IsNullOrEmpty(comment))
                                    {
                                        ResXDataNode resultNode = new ResXDataNode(key, dataNode.GetValue((ITypeResolutionService)null))
                                        {
                                            Comment = comment
                                        };
                                        destinationWriter.AddResource(resultNode);
                                    } else
                                    {
                                        destinationWriter.AddResource(key, value);
                                    }
                                    
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(comment))
                                    {
                                        ResXDataNode resultNode = new ResXDataNode(key, destEnumerator.Value)
                                        {
                                            Comment = comment
                                        };
                                        destinationWriter.AddResource(resultNode);
                                    }
                                    else
                                    {
                                        destinationWriter.AddResource(key, destEnumerator.Value);
                                    }  
                                }

                            }
                        }
                    }
                }

                // Save the changes to the destination file
                destinationWriter.Generate();
            }

            Console.WriteLine("Resx files merged successfully!");
        }
    }
}
