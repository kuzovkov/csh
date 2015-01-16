/**
 * Класс содержащий параметры конфигурации
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace tcp_client
{
    class Config
    {
        public string config_file = "hb_cap.conf.xml";
        public Int32 port = 10000;
        public string ip = "127.0.0.1";
        public string dump_folder = "tcp_dump";
        public int dump_number = 0;
        public string dump_ext = "bin";
        public string dump_request_prefix = "req";
        public string dump_response_prefix = "res";
        public int read_buffer = 4096;

        public Config() { read();  }

        public Config(string filename)
        {
            this.config_file = filename;
            read();
        }
        
        private bool read()
        {
            if (!File.Exists(this.config_file))
            {
                Console.WriteLine("File {0} not exists", this.config_file);
                return false;
            }
            string s = "";
            string xmlString = "";
            StreamReader sr = File.OpenText(config_file);
            while ((s = sr.ReadLine()) != null)
            {
                xmlString += s;
            }
            XmlReader reader = XmlReader.Create(new StringReader(xmlString));
            try
            {
                reader.ReadToFollowing("ip_address");
                this.ip = reader.ReadElementContentAsString();
                reader.ReadToFollowing("port");
                this.port = reader.ReadElementContentAsInt();
                reader.ReadToFollowing("dump_folder");
                this.dump_folder = reader.ReadElementContentAsString();
                reader.ReadToFollowing("dump_number");
                this.dump_number = reader.ReadElementContentAsInt();
                reader.ReadToFollowing("dump_ext");
                this.dump_ext = reader.ReadElementContentAsString();
                reader.ReadToFollowing("dump_request_prefix");
                this.dump_request_prefix = reader.ReadElementContentAsString();
                reader.ReadToFollowing("dump_response_prefix");
                this.dump_response_prefix = reader.ReadElementContentAsString();
                reader.ReadToFollowing("read_buffer");
                this.read_buffer = reader.ReadElementContentAsInt();
            }
            catch (Exception e) 
            {
                Console.WriteLine("Ошибка парсинга {0} - {1}",this.config_file, e.Message);
                return false;
            }
            return true;
        }
    }
}
