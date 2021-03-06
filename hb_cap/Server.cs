﻿/**
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
using MyDump;


namespace hb_cap
{
    class Server
    {
        private Config config;
        private TcpListener server = null;
        private Dump dump = null;
        /**
         * Конструктор
         * @param config объект содержащий конфигурационные данные
         * */
        public Server(Config config)
        {
            this.config = config;
            this.dump = new Dump(config);

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
         * Старт сервера
         * */
        public void Start()
        {
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
                            resp = this.dump.select(bytes, read);
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

