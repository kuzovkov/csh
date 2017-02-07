/**
 * Класс для считывания из XML файла и хранения конфигурационных данных
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace MyConfig
{
    class Config
    {
        private XmlReader reader = null;
        /*структура для хранения записей*/
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

        /**
         * конструктор, принимает имя файла конфигурации
         * @param filename имя файла конфигурации
         * */
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

        /**
         * чтение данных конфигурации из файла
         * @param reader объект XmlReader
         * */
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

        /**
        * получение строкового значения
        * @param name имя значения
        * @return значение параметра
        * */
        public string getValue(string name)
        {
            foreach (record rec in this.confItem)
            {
                if (rec.name == name) return rec.value;
            }
            return "";
        }

        /**
        * получение целочисленного значения
        * @param name имя значения
        * @return значение параметра
        * */
        public int getIntValue(string name)
        {
            foreach (record rec in this.confItem)
            {
                if (rec.name == name) return Convert.ToInt32(rec.value, 10);
            }
            return 0;
        }

        /**
        * вывод на печать всех пар имя: значение
        * */
        public void prinList()
        {
            foreach (record rec in this.confItem)
            {
                Console.WriteLine("{0}: {1}", rec.name, rec.value);
            }
        }
    }


}
