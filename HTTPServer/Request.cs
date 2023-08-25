using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    public enum RequestMethod
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {
        string[] requestLines;
        RequestMethod method;
        public string relativeURI;
        Dictionary<string, string> headerLines;

        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }

        HTTPVersion httpVersion;
        string requestString;
        string[] contentLines;

        public Request(string requestString)
        {
            this.requestString = requestString;
        }
        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>
        public bool ParseRequest()
        {


            //TODO: parse the receivedRequest using the \r\n delimeter   
            requestLines = requestString.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)
            if (requestLines.Length < 3)

            {
                return false;
            }// Parse Request line
            if (ParseRequestLine() == false)
                return false;

            // Validate blank line exists
            if (ValidateBlankLine() == false)
            {
                return false;
            }
            // Load header lines into HeaderLines dictionary
            if (LoadHeaderLines() == false)
            {
                return false;
            }
            return true;
        }

        private bool ParseRequestLine()
        {
            string[] words;
            words = requestLines[0].Split(' ');
            if (words[0] == "GET")
                method = RequestMethod.GET;

            else if (words[0] == "POST")
                method = RequestMethod.POST;

            else if (words[0] == "HEAD")
                method = RequestMethod.HEAD;
            else
                return false;
            if (ValidateIsURI(words[1]) == false)
            {
                return false;
            }
            else 
            {
                relativeURI = words[1];
            
            }
            if (words[2] == "HTTP/0.9")
            {
                httpVersion = HTTPVersion.HTTP09;
            }
            else if (words[2] == "HTTP/1.0")
            {
                httpVersion = HTTPVersion.HTTP10;
            }
            else if (words[2] == "HTTP/1.1")
            {
                httpVersion = HTTPVersion.HTTP11;
            }
            else
                return false;
            return true;
        }

        private bool ValidateIsURI(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines()
        {
            headerLines = new Dictionary<string, string>();
            string[] Lines;

            for (int i = 1; requestLines[i] != ""; i++)
            {
                try
                {
                    Lines = requestLines[i].Split(':');
                    headerLines[Lines[0]] = Lines[1];
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;

        }

        private bool ValidateBlankLine()
        {
            return requestString.Contains("\r\n\r\n");
        }

    }
}
