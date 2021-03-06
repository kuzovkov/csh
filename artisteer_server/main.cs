﻿/**
 * Artisteer server
 **/
using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using MyConfig;
using ArtisteerServer;

namespace artisteer_server
{
    class main
    {
        static void Main(string[] args)
        {
            Config conf = (args == null || args.Length == 0) ? new Config("artisteer_server.xml") : new Config(args[0]);
            Console.WriteLine("ip: {0}; port: {1}", conf.getValue("ip_address"), conf.getValue("port"));
            Server server = new Server(conf);
            server.Start();

        }
    }
}