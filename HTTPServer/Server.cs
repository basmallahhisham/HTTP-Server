using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HTTPServer
{
    class Server
    {
        Socket serverSocket;

        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            //TODO: initialize this.serverSocket
            this.LoadRedirectionRules(redirectionMatrixPath);
            this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, portNumber);
            this.serverSocket.Bind(iep);
        }

        public void StartServer()
        {
            // TODO: Listen to connections, with large backlog.
            this.serverSocket.Listen(10);
            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            while (true)
            {
                //TODO: accept connections and start thread for each accepted connection.
                    Socket clientSock = serverSocket.Accept();
                    Thread thread = new Thread(new ParameterizedThreadStart(HandleConnection));
                    thread.Start(clientSock);               
            }
        }
        public void HandleConnection(object obj)
        {
            // TODO: Create client socket 
            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
            Socket clientSock=(Socket)obj;
            clientSock.ReceiveTimeout = 0;
            // TODO: receive requests in while true until remote client closes the socket.
            while (true)
            {
                try
                {
                    byte[] data = new byte[1024*1024];
                    int receivedBytesLen = clientSock.Receive(data);

                    if (receivedBytesLen == 0)
                        break;

                    string requestData= Encoding.ASCII.GetString(data,0, receivedBytesLen);
                    Request req = new Request(requestData);
                    Response responseToclient= HandleRequest(req);

                   clientSock.Send(Encoding.ASCII.GetBytes(responseToclient.ResponseString));
                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                    Logger.LogException(ex);
                }
            }
            clientSock.Close();
            // TODO: close client socket
        }

        Response HandleRequest(Request request)
        {
            //throw new NotImplementedException();
            string content;
            StatusCode status;
            string physicalPath;
            //IPAddress[] addresses;
            try
            {
                //TODO: check for bad request 
                if (request.ParseRequest() == false)
                {
                    status = StatusCode.BadRequest;
                    content = LoadDefaultPage(Configuration.BadRequestDefaultPageName);
                    return new Response(status, "text/html", content, "");
                }
                //TODO: check for redirect

                if (Configuration.RedirectionRules.ContainsKey(request.relativeURI.Substring(1)))
                {
                    status = StatusCode.Redirect;
                    content = LoadDefaultPage(Configuration.RedirectionDefaultPageName);
                    return new Response(status, "text/html", content, Configuration.RedirectionRules[request.relativeURI.Substring(1)]);
                }

                physicalPath= Path.Combine(Configuration.RootPath, request.relativeURI.Substring(1));
                //TODO: check file exists
                if (!File.Exists(physicalPath))
                {
                    status = StatusCode.NotFound;
                    content = LoadDefaultPage(Configuration.NotFoundDefaultPageName);
                    return new Response(status, "text/html", content, "");
                }

                //TODO: read the physical file
                // Create OK response
                status = StatusCode.OK;
                content = LoadDefaultPage(request.relativeURI.Substring(1));
                return new Response(status, "text/html", content,"");


            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                // TODO: in case of exception, return Internal Server Error. 
                status = StatusCode.InternalServerError;
                content = LoadDefaultPage(Configuration.InternalErrorDefaultPageName);
                return  new Response(status, "text/html", content, "");
            }
            
        }

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty
           bool check=Configuration.RedirectionRules.ContainsKey(relativePath);

           if (check)
           {
              return Configuration.RedirectionRules[relativePath];
           }
            
            return string.Empty;
        }

        private string LoadDefaultPage(string defaultPageName)
        {
           string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
            // TODO: check if filepath not exist log exception using Logger class and return empty string
            if (!File.Exists(filePath))
            {
                Logger.LogException(new FileNotFoundException(filePath));
                return string.Empty;
            }

            // else read file and return its content
            return File.ReadAllText(filePath);
        }

        private void LoadRedirectionRules(string filePath)
        {
            Configuration.RedirectionRules = new Dictionary<string, string>();
            try
            {

                StreamReader fileName = new StreamReader(filePath);
                // TODO: using the filepath paramter read the redirection rules from file 
                // then fill Configuration.RedirectionRules dictionary 
                string line = null;
                while (null != (line = fileName.ReadLine())) {
                    string[] values = line.Split(',');
                    Configuration.RedirectionRules.Add(values[0], values[1]);
                }
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                Environment.Exit(1);
            }
        }
    }
}
