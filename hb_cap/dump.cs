/**
 * Класс для работы с дампами
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MyConfig;

namespace MyDump
{
     
    class Dump
    {
        private Config config;
        private Byte[][] request = null;
        private Byte[][] response = null;
        private int[] assumeDump = null;
        private Byte[] responseDefault = new Byte[] { 0x07 };

        public Dump(Config conf) 
        {
            this.config = conf;
            this.load();
        }

        /**
         * Сравнение двух массивов байт
         * @param data1, data2 сравниваемые массивы байт
         * @return true если совпадают, иначе false
         * */
        public bool cmp(Byte[] data1, Byte[] data2)
        {
            if (data1 == null || data2 == null) return false;
            int len = (data1.Length < data2.Length) ? data1.Length : data2.Length;
            for (int i = 0; i < len; i++)
            {
                if (data1[i] != data2[i]) return false;
            }
            return true;
        }

        /**
        * Считывание в массивы дампов запросов и ответов из файлов
        * */
        private void load()
        {
            /*inicialize dumps array*/

            this.request = new Byte[this.config.getIntValue("dump_request_number")][];
            this.response = new Byte[this.config.getIntValue("dump_response_number")][];

            /*read dumps of requests*/
            string filename = "";
            for (int i = 0; i < this.config.getIntValue("dump_request_number"); i++)
            {
                filename = config.getValue("dump_folder") + @"\" + config.getValue("dump_request_prefix") + "." + i.ToString() + "." + config.getValue("dump_ext");
                if (File.Exists(filename))
                {
                    this.request[i] = File.ReadAllBytes(filename);
                }
                else
                {
                    this.request[i] = null;
                }
            }

            /*read dumps of responses*/
            for (int i = 0; i < this.config.getIntValue("dump_response_number"); i++)
            {
                filename = config.getValue("dump_folder") + @"\" + config.getValue("dump_response_prefix") + "." + i.ToString() + "." + config.getValue("dump_ext");
                if (File.Exists(filename))
                {
                    this.response[i] = File.ReadAllBytes(filename);
                }
                else
                {
                    this.response[i] = null;
                }
            }

            this.loadAssumeDump();
        }

        /**
        * Инициализация массива отвечающего за назначение соответсвий 
        * входных и выходных дампов
        * */
        private void loadAssumeDump()
        {
            this.assumeDump = new int[this.config.getIntValue("dump_request_number")]; 
            string selectDump = this.config.getValue("select_dump");
            string[] strArr = selectDump.Split(new Char[]{',',' '});
            int i = 0;
            foreach (string s in strArr)
            {
                if (s.Length > 0)
                {
                    this.assumeDump[i] = Convert.ToInt32(s, 10);
                    i++;
                }
            }
        }

        /**
         * Сравнение массива байт из запроса с имеющимися и 
         * возвращение соответсвующего байтового массива ответа
         * @param bytes входной массив байт
         * @return соответсвующий индекс или -1 
         * */
        public Byte[] select(Byte[] bytes)
        {
            for (int i = 0; i < this.config.getIntValue("dump_request_number"); i++)
            {
                if (this.cmp(bytes, this.request[i])) return this.response[this.assumeDump[i]];
            }
            return this.responseDefault;
        }

    }
}
