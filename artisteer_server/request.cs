using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtisteerServer
{

    struct param
    {
        public string name;
        public string value;

        public param(string n, string val)
        {
            name = n;
            value = val;
        }
    };
    
    
    class Request
    {
        /*структура для хранения полей запроса*/

        public List<param> headers = null;
        public string method = null;
        public string uri = null;
        public List<byte> body = null;
        public string protocol = null;
        private int begin_body = 0;
        private int content_len = 0;


        public Request()
        {
            this.headers = new List<param>();
            this.body = new List<byte>();
            this.begin_body = 0;
        }
        
        public bool parseData(byte[] bytes, int read)
        {
            bool done = false;
            int startLine = 0;
            int endLine = 0;
            
            for (int i = 0; i < read - 1; i++)
            {
                if (bytes[i] == 0xD && bytes[i + 1] == 0xA)
                {
                    startLine = endLine;
                    endLine = i;
                    string line = Encoding.Default.GetString(bytes, startLine, (endLine - startLine)).Trim();
                    //Console.WriteLine("line={0}", line);
                    if (line.Length > 3 && (line.Substring(0, 3) == "GET" || line.Substring(0, 4) == "POST"))
                    {
                        string[] words = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        this.method = words[0];
                        this.uri = words[1];
                        this.protocol = words[2];
                    }
                    else if (line.Length > 0 && line.IndexOf(":") != -1)
                    {
                        
                        string[] words = line.Split(new string[] { ": " }, StringSplitOptions.None);
                        if (words.Length > 1) this.headers.Add(new param(words[0].Trim(), words[1].Trim()));
                    }
                    else 
                    {
                        if (this.method == "GET")
                        {
                            done = true;
                        }
                        else if (this.method == "POST")
                        {
                            this.begin_body = i+2;
                        }
                        string contentLength = this.getHeader("Content-Length");
                        if (contentLength != null) this.content_len = Int32.Parse(contentLength);
                            
                    }
                }

                if (this.begin_body > 0 && i >= begin_body)
                {
                    this.body.Add(bytes[i]);
                    if ( i == read - 1) this.body.Add(bytes[i+1]);
                    if (this.content_len == 0)
                    {
                        done = true;
                    }
                    else
                    {
                        if (this.body.Count >= content_len - 1)
                        {
                            done = true;
                        }
                    }
                }
            }
            return done;
        }


        /**
       * получение строкового значения
       * @param name имя значения
       * @return значение параметра
       * */
        public string getHeader(string name)
        {
            foreach (param pair in this.headers)
            {
                if (pair.name == name) return pair.value;
            }
            return null;
        }

        public List<param> getHeaders()
        {
            return this.headers;
        }

        public string getBody()
        {
            if (this.body.Count == 0)
                return null;
            return Encoding.Default.GetString(this.body.ToArray());
        }

        public byte[] getRawBody()
        {
            if (this.body.Count == 0)
                return null;
            return this.body.ToArray();
        }
    }
}
