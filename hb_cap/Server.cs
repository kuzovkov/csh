/**
 * TCP Listener
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using MyConfig;


namespace hb_cap
{
    class Server
    {
        private Config config;
        private TcpListener server = null;
        private Byte[][] request = null;
        private Byte[][] response = null;
        private Byte[] responseDefault = new Byte[] { 0x07 };
        
        /**
         * Конструктор
         * @param config объект содержащий конфигурационные данные
         * */
        public Server(Config config)
        {
            this.config = config;
            try 
            {
                IPAddress localAddr = IPAddress.Parse(this.config.getValue("ip_address"));
                // TcpListener server = new TcpListener(port);
                Int32 port = this.config.getIntValue("port");
                this.server = new TcpListener(localAddr, port);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        /**
         * Сравнение двух массивов байт
         * @param data1, data2 сравниваемые массивы байт
         * @return true если совпадают, иначе false
         * */
        private bool cmpData(Byte[] data1, Byte[] data2 )
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
        private void loadDumps()
        {
            /*inicialize dumps array*/
            
            this.request = new Byte[this.config.getIntValue("dump_number")][];
            this.response = new Byte[this.config.getIntValue("dump_number")][];
            
            /*read dumps of requests*/
            string filename = "";
            for (int i = 0; i < this.config.getIntValue("dump_number"); i++)
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
            for (int i = 0; i < this.config.getIntValue("dump_number"); i++)
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
        }

        /**
         * Сравнение массива байт из запроса с имеющимися и 
         * возвращение соответсвующего индекса байтового массива ответа
         * @param bytes входной массив байт
         * @return соответсвующий индекс или -1 
         * */
        private Byte[] selectResponse(Byte[] bytes)
        {
            for (int i = 0; i < this.config.getIntValue("dump_number"); i++)
            {
                if (cmpData(bytes, this.request[i])) return this.response[i];
            }
            return this.response[5];
        }

        /**
         * Старт сервера
         * */
        public void Start()
        {
            this.loadDumps();
            try
            {
                // Start listening for client requests.
                this.server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[this.config.getIntValue("read_buffer")];
                Byte[] resp = null;
                // Enter the listening loop.
                while (true)
                {
                    Console.Write("\nWaiting for a connection on port {0} ... ", config.getValue("port"));

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = this.server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int read;

                    try
                    {
                        // Loop to receive all the data sent by the client.
                        while ((read = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {

                            Console.WriteLine("\nReceived:");
                            for (int i = 0; i < read; i++)
                            {
                                Console.Write("{0} ", bytes[i].ToString("X2"));
                            }

                            // Send back a response.
                            resp = selectResponse(bytes);
                            stream.Write(resp, 0, resp.Length);
                            Console.WriteLine("\nSent:");
                            for (int i = 0; i < resp.Length; i++)
                            {
                                Console.Write("{0} ", resp[i].ToString("X2"));
                            }
                        }

                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine("Удаленный хост разорвал соединение {0}", e.Message);
                        //client.Close();
                    }
                    catch (System.Net.Sockets.SocketException e) 
                    {
                        Console.WriteLine("Удаленный хост разорвал соединение {0}", e.Message);
                        //client.Close();
                    }
                    

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }//end Start

        /**
         * Останов сервера
         * */
        public void Stop()
        {
            this.server.Stop();
        }//end Stop

    }
}

