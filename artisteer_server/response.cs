using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtisteerServer;

namespace ArtisteerServer
{
    
    
    class Response
    {
        private Request request;
        private string protocol = "HTTP/1.1";
        private int code = 200;
        private string title = "OK";
        private List<param> headers = null;
        private List<byte> body = null;
        
        public Response(Request request)
        { 
            this.request = request;
            this.headers = new List<param>();
            this.body = new List<byte>();
        }

        public byte[] getData()
        {
            List<string> head = new List<string>();
            head.Add(this.protocol);
            head.Add(" ");
            head.Add(this.code.ToString());
            head.Add(" ");
            head.Add(this.title);
            head.Add("\r\n");

            foreach (param header in this.headers)
            {
                head.Add(header.name);
                head.Add(": ");
                head.Add(header.value);
                head.Add("\r\n");
            }
            head.Add("\r\n");
            byte[] head_bytes = Encoding.Default.GetBytes(String.Join("", head.ToArray()));
            List<byte> res_list_bytes = new List<byte>();
            for (int i = 0; i < head_bytes.Length; i++)
            {
                res_list_bytes.Add(head_bytes[i]);
            }

            foreach (byte b in this.body)
            {
                res_list_bytes.Add(b);
            }

            return res_list_bytes.ToArray();

        }

        private void setHeader(string key, string value)
        {
            this.headers.Add(new param(key, value));
        }

        private void setBody(byte[] data) 
        {
            this.body.Clear();
            for (int i = 0; i < data.Length; i++)
                this.body.Add(data[i]);
        }

        private void setBody(string str)
        {
            this.body.Clear();
            byte[] data = Encoding.Default.GetBytes(str);
            for (int i = 0; i < data.Length; i++)
                this.body.Add(data[i]);
        }

        private void setBody(string[] str_arr)
        {
            this.body.Clear();
            string str = String.Join("", str_arr);
            byte[] data = Encoding.Default.GetBytes(str);
            for (int i = 0; i < data.Length; i++)
                this.body.Add(data[i]);
        }


        private void makeResponse()
        {
            
        }

    }
}
