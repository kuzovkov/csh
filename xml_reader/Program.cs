/**
 * XML Reader
 **/
using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace hb_cap
{
    class Program
    {
        static void Main(string[] args)
        {
            string config_file = @"test_xml.xml";
            if (!File.Exists(config_file))
            {
                Console.WriteLine("File {0} not exists", config_file);
            }
            string s = "";
            string xmlString = "";
            StreamReader sr = File.OpenText(config_file);
            while ((s = sr.ReadLine()) != null)
            {
                xmlString += s;
            }
            //Console.WriteLine(xmlString);
            XmlReader reader = XmlReader.Create(new StringReader(xmlString));
            reader.ReadToFollowing("title");
            string title = reader.ReadElementContentAsString();
            Console.WriteLine(title);


            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    if (reader.IsEmptyElement)
                        Console.WriteLine("<{0}/>", reader.Name);
                    else
                    {
                        Console.Write("<{0}> ", reader.Name);
                        reader.Read(); // Read the start tag.
                        if (reader.IsStartElement())  // Handle nested elements.
                            Console.Write("\r\n<{0}>", reader.Name);
                        Console.WriteLine(reader.ReadString());  //Read the text content of the element.
                    }
                }
            }

        }
    }
}
