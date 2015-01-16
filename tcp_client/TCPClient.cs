/**
 * TCP Client
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace tcp_client
{
    class TCPClient
    {

        private Config conf;
        
        public TCPClient(Config config)
        {
            this.conf = config;
        }
        
        public void Send( Byte[] data)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                TcpClient client = new TcpClient(this.conf.ip, this.conf.port);

                // Translate the passed message into ASCII and store it as a Byte array.
                //Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);         

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Console.WriteLine("\n\nSent:");
                for (int i = 0; i < data.Length; i++)
                {
                    Console.Write("{0} ", data[i].ToString("X2"));
                }

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                Byte[] read_buf = new Byte[this.conf.read_buffer];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(read_buf, 0, read_buf.Length);
                //responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("\nReceived:");
                for (Int32 i = 0; i < bytes; i++)
                {
                    Console.Write("{0} ", read_buf[i].ToString("X2"));
                }

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            //Console.WriteLine("\n Press Enter to continue...");
            //Console.Read();
        }
    }
}
