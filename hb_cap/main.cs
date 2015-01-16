/**
 * HonorBuddy cap server
 **/
using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace hb_cap
{
    class main
    {
        static void Main(string[] args)
        {
            Config conf = (args == null || args.Length == 0)? new Config() : new Config(args[0]);
            Console.WriteLine("ip: {0}; port: {1}", conf.ip, conf.port.ToString());
            Server server = new Server(conf);
            server.Start();

        }
    }
}
