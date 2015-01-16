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


namespace tcp_listener
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server=null;
            string config_file = @"tcp_server.cfg";
            if (!File.Exists(config_file))
            {
                Console.WriteLine("File {0} not exists", config_file);
            }
            StreamReader sr = File.OpenText(config_file);

             // Set the TcpListener on port 13000.
            Int32 port = 13000;
            string s = "";
            if ((s = sr.ReadLine()) != null)
            {
                port = Convert.ToInt32(s);
            }
            
            try
            {
             
              IPAddress localAddr = IPAddress.Parse("127.0.0.1");
              // TcpListener server = new TcpListener(port);
              server = new TcpListener(localAddr, port);

              // Start listening for client requests.
              server.Start();

              // Buffer for reading data
              Byte[] bytes = new Byte[4096];
              String data = null;

              // Enter the listening loop.
              while(true) 
              {
                Console.Write("Waiting for a connection on port {0} ... ", port);

                // Perform a blocking call to accept requests.
                // You could also user server.AcceptSocket() here.
                TcpClient client = server.AcceptTcpClient();            
                Console.WriteLine("Connected!");

                data = null;

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                int read;

                // Loop to receive all the data sent by the client.
                while((read = stream.Read(bytes, 0, bytes.Length))!=0) 
                {   
                  // Translate data bytes to a ASCII string.
                  //data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received:");
                    for (int i = 0; i < read; i++)
                    {
                        Console.Write("{0} ", bytes[i].ToString("X2"));
                    }

                        // Process the data sent by the client.
                        //data = data.ToUpper();

                        //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        stream.Write(bytes, 0, read);
                        Console.WriteLine("Sent:");
                        for (int i = 0; i < read; i++)
                        {
                            Console.Write("{0} ", bytes[i].ToString("X2"));
                        }
            
                }

                // Shutdown and end connection
                client.Close();
              }
            }
            catch(SocketException e)
            {
              Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
               // Stop listening for new clients.
               server.Stop();
            }


            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
          }   
      }
 }

