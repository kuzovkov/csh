using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace Config
{
    class Config
    {
        private XmlReader reader = null;
        public struct record 
        {
            public string name;
            public string value;

            public record(string n, string val)
            {
                name = n;
                value = val;
            }

        };

        private List<record> confItem = new List<record>(); 

        public Config(string filename)
        {
            if (File.Exists(filename))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                settings.IgnoreComments = true;
                this.reader = XmlReader.Create(filename, settings);
                read(this.reader);
            }
            else { Console.WriteLine("Fail open file {0}", filename); }
        }

        private void read(XmlReader reader)
        {
            string value;
            string name;
            while (reader.Read())
            {
                value = reader.ReadString();
                name = reader.Name;
                if (value == "") continue;
                record rec = new record(name, value);
                this.confItem.Add(rec); 
            }
        }

        public string getValue(string name)
        {
            foreach (record rec in this.confItem)
            {
                if (rec.name == name) return rec.value;
            }
            return "";
        }

        public void prinList()
        {
            foreach (record rec in this.confItem)
            {
                Console.WriteLine("{0}: {1}",rec.name, rec.value);
            }
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Config conf = new Config("hb_cap.conf.xml");
            conf.prinList();
            Console.WriteLine(conf.getValue("ip_address"));
        }
    }
}
