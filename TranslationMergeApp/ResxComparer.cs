using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Resources;
using System.Xml;

public class ResxComparer
{
    public static void MergeResxFiles(string sourceFilePath, string destinationFilePath)
    {
        XmlDocument srcDoc = new XmlDocument();
        XmlDocument destDoc = new XmlDocument();

        srcDoc.Load(sourceFilePath);
        destDoc.Load(destinationFilePath);

        XmlNodeList srcDataNodes = srcDoc.SelectNodes("//root/data");
        XmlNodeList destDataNodes = destDoc.SelectNodes("//root/data");

        foreach (XmlNode srcDataNode in srcDataNodes)
        {
            string srcKey = srcDataNode.Attributes["name"].Value;
            XmlNode srcValueNode = srcDataNode.SelectSingleNode("value");
            string srcValue = srcValueNode.InnerText;

            foreach (XmlNode destDataNode in destDataNodes)
            {
                string destKey = destDataNode.Attributes["name"].Value;
                string destValue = destDataNode.SelectSingleNode("value").InnerText;

                if (string.Equals(destKey, srcKey, StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(destValue))
                    {
                        destDataNode.SelectSingleNode("value").InnerText = srcValue;
                    }
                    break;
                }
            }
        }
        destDoc.Save(destinationFilePath);
        Console.WriteLine("Resx files merged successfully!");
    }

    // Since this is an in-place operation, I don't need to handle the comment copy part.
    public static void MergeResxFiles4(string sourceFilePath, string destinationFilePath)
    {
        XmlDocument srcDoc = new XmlDocument();
        XmlDocument destDoc = new XmlDocument();

        srcDoc.Load(sourceFilePath);
        destDoc.Load(destinationFilePath);

        XmlNodeList srcDataNodes = srcDoc.SelectNodes("//root/data");
        XmlNodeList destDataNodes = destDoc.SelectNodes("//root/data");

        foreach (XmlNode srcDataNode in srcDataNodes)
        {
            string srcKey = srcDataNode.Attributes["name"].Value;
            XmlNode srcValueNode = srcDataNode.SelectSingleNode("value");
            string srcValue = srcValueNode.InnerText;
            XmlNode srcCommentNode = null;
            string srcComment = null;
            if (srcDataNode.SelectSingleNode("comment") != null)
            {
                srcCommentNode = srcDataNode.SelectSingleNode("comment");
                srcComment = srcCommentNode.InnerText;
            }

            foreach (XmlNode destDataNode in destDataNodes)
            {
                string destKey = destDataNode.Attributes["name"].Value;
                string destValue = destDataNode.SelectSingleNode("value").InnerText;
                XmlNode destCommentNode = null;
                string destComment = null;
                if (destDataNode.SelectSingleNode("comment") != null)
                {
                    destCommentNode = destDataNode.SelectSingleNode("comment");
                    destComment = destCommentNode.InnerText;
                }

                if (string.Equals(destKey, srcKey, StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(destValue))
                    {
                        if (!string.IsNullOrEmpty(destComment))
                        {
                            destCommentNode.InnerText = srcComment;
                        }
                        destDataNode.SelectSingleNode("value").InnerText = srcValue;
                    }
                    break;
                }
            }
        }
        destDoc.Save(destinationFilePath);
        Console.WriteLine("Resx files merged successfully!");
    }

    // This method cannot make in-place change to a resx data node, I want a in-place change instead of using ResxWrite to create a new one
    public static void MergeResxFiles3(string sourceFilePath, string destinationFilePath)
    {
        // Write the merged resources back to the destination file
        using (var destinationWriter = new ResXResourceWriter(destinationFilePath))
        {
            XmlDocument srcDoc = new XmlDocument();
            XmlDocument destDoc = new XmlDocument();

            srcDoc.Load(sourceFilePath);
            destDoc.Load(destinationFilePath);

            XmlNodeList srcDataNodes = srcDoc.SelectNodes("//root/data");
            XmlNodeList destDataNodes = destDoc.SelectNodes("//root/data");

            foreach (XmlNode srcDataNode in srcDataNodes)
            {
                string srcKey = srcDataNode.Attributes["name"].Value;
                XmlNode srcValueNode = srcDataNode.SelectSingleNode("value");
                string srcValue = srcValueNode.InnerText;
                XmlNode srcCommentNode;
                string srcComment = null;
                if (srcDataNode.SelectSingleNode("comment") != null)
                {
                    srcCommentNode = srcDataNode.SelectSingleNode("comment");
                    srcComment = srcCommentNode.InnerText;
                }

                foreach (XmlNode destDataNode in destDataNodes)
                {
                    string destKey = destDataNode.Attributes["name"].Value;
                    string destValue = destDataNode.SelectSingleNode("value").InnerText;
                    XmlNode destCommentNode;
                    string destComment = null;
                    if (destDataNode.SelectSingleNode("comment") != null)
                    {
                        destCommentNode = destDataNode.SelectSingleNode("comment");
                        destComment = destCommentNode.InnerText;
                    }

                    if (string.Equals(destKey, srcKey, StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(destValue))
                        {
                            if (!string.IsNullOrEmpty(destComment))
                            {
                                ResXDataNode resultNode = new ResXDataNode(srcKey, srcValue)
                                {
                                    Comment = srcComment
                                };
                                destinationWriter.AddResource(resultNode);
                            }
                            else
                            {
                                destinationWriter.AddResource(srcKey, srcValue);
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(destComment))
                            {
                                ResXDataNode resultNode = new ResXDataNode(destKey, destValue)
                                {
                                    Comment = srcComment
                                };
                                destinationWriter.AddResource(resultNode);
                            }
                            else
                            {
                                destinationWriter.AddResource(destKey, destValue);
                            }
                        }
                        break;
                    }
                }
            }

            // Save the changes to the destination file
            destinationWriter.Generate();
        }
        Console.WriteLine("Resx files merged successfully!");
    }

    // ResXResourceReader cannot handle empty node with type of "System.Resources.ResXFileRef, System.Windows.Forms"
    public static void MergeResxFiles2(string sourceFilePath, string destinationFilePath)
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
                    foreach (DictionaryEntry srcEntry in sourceReader)
                    {
                        string resourceName = srcEntry.Key.ToString();
                        object resourceValue = srcEntry.Value;

                        foreach (DictionaryEntry destEntry in destinationReader)
                        {
                            string destName = destEntry.Key.ToString();
                            object destValue = destEntry.Value;

                            if (destValue != null && destValue.GetType().FullName == "System.Resources.ResXFileRef, System.Windows.Forms")
                            {
                                continue;
                            }
                            else
                            {
                                if (string.Equals(destName, resourceName, StringComparison.OrdinalIgnoreCase))
                                {
                                    string comment = "";
                                    ResXDataNode dataNode = (ResXDataNode)resourceValue;
                                    if (dataNode != null && dataNode.Comment != null)
                                    {
                                        comment = dataNode.Comment;
                                    }
                                    if (string.IsNullOrEmpty((string)destValue))
                                    {
                                        if (!string.IsNullOrEmpty(comment))
                                        {
                                            ResXDataNode resultNode = new ResXDataNode(resourceName, dataNode.GetValue((ITypeResolutionService)null))
                                            {
                                                Comment = comment
                                            };
                                            destinationWriter.AddResource(resultNode);
                                        }
                                        else
                                        {
                                            destinationWriter.AddResource(resourceName, resourceValue);
                                        }
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(comment))
                                        {
                                            ResXDataNode resultNode = new ResXDataNode(destName, destValue)
                                            {
                                                Comment = comment
                                            };
                                            destinationWriter.AddResource(resultNode);
                                        }
                                        else
                                        {
                                            destinationWriter.AddResource(destName, destValue);
                                        }
                                    }
                                    break;
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

    // Enumerator cannot handle the datatype = System.Resources.ResXFileRef, System.Windows.Forms
    public static void MergeResxFiles1(string sourceFilePath, string destinationFilePath)
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
