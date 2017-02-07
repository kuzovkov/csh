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
using ArtisteerServer;


namespace ArtisteerServer
{
    class Server
    {
        private Config config;
        private TcpListener server = null;
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
                Byte[] response = null;
                // Enter the listening loop.
                while (true)
                {
                    Console.WriteLine("\nWaiting for a connection on port {0} ... ", config.getValue("port"));

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = this.server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int read;
                    bool req_done = false;
                    Request req = new Request();

                    try
                    {
                        // Loop to receive all the data sent by the client.
                        for ( ; !req_done && ((read = stream.Read(bytes, 0, bytes.Length)) != 0); )
                        {
                            //Console.Write(Encoding.Default.GetString(bytes));
                            req_done = req.parseData(bytes, read);
                        }//end while

                        req.showInfo();
                       
                        //todo response
                        Response res = new Response(req);
                        byte[] res_data = res.getData();
                        stream.Write(res_data, 0, res_data.Length);
                        client.Close();
                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine("Удаленный хост разорвал соединение {0}", e.Message);
                        client.Close();
                    }
                    catch (System.Net.Sockets.SocketException e) 
                    {
                        Console.WriteLine("Удаленный хост разорвал соединение {0}", e.Message);
                        client.Close();
                    }
                    

                    // Shutdown and end connection
                    //client.Close();
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

    }//end class Server

    
}

