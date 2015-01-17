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
using MyConfig;

namespace tcp_client
{
    
    class main
    {
       
        static void Main(string[] args)
        {
            Config conf = new Config("hb_cap.conf.xml");
            TCPClient client = new TCPClient(conf);
            
            
            /*чтение данных*/
            string data_file = "";
            try 
            {
                if (args == null || args.Length == 0)
                {
                    for (int i = 0; i < 11; i++)
                    {
                        data_file = "tcp_dump/req." + i.ToString() + ".bin";
                        Byte[] data = File.ReadAllBytes(data_file);
                        client.Send(data);
                        data = null;
                    }
                }
                else 
                {
                    data_file = args[0];
                    Byte[] data = File.ReadAllBytes(data_file);
                    Console.WriteLine(data_file);
                    client.Send(data); 
                }   
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        
        }
    }
}
