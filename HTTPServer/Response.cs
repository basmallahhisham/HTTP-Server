using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{

    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        NotFound = 404,
        BadRequest = 400,
        Redirect = 301
    }

    class Response
    {
        string responseString;
        public string ResponseString
        {
            get
            {
                return responseString;
            }
        }
        StatusCode code;
        List<string> headerLines = new List<string>();
        public Response(StatusCode code, string contentType, string content, string redirectoinPath)
        {
            //throw new NotImplementedException();
            // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])
            headerLines = new List<string>()
            {
                "Content-Type: " + contentType ,
                "Content-Length: " + content.Length ,
                "Date: " + DateTime.Now

            };
            if (redirectoinPath != "")
            {
                headerLines.Add("Location: " + redirectoinPath);
            }
            // TODO: Create the response string
            responseString = GetStatusLine(code);
            foreach (string element in headerLines)
            {
                responseString += element + "\r\n";
            }
            responseString += "\r\n" + content;
        }

        private string GetStatusLine(StatusCode code)
        {
            // TODO: Create the response status line and return it
            string statusLine = "HTTP/1.1 " + (int)code + " " + code + "\r\n";

            return statusLine;
        }
    }
}
