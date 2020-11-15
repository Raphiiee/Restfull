using Restfull.Structs;
using System;
using System.Collections.Generic;

namespace Restfull
{
    public class Server
    {
        private ResponseContext _response;
        private RequestContext _request;

        public string GetPath()
        {
            return _request.Path;
        }

        public string GetMethod()
        {
            return _request.Method;
        }

        public string GetHeader()
        {
            return _response.Header;
        }


        public void GetData(string data)
        {
            Console.WriteLine(data);
            _response = default(ResponseContext);
            _request = default(RequestContext);

            _response.Version = "HTTP/1.1 ";
            _response.Status = "200 OK\n";
            _response.Server = "Server: MTCG\n";
            _response.Message = "Something went wrong";
            _response.ContentLength = $"Content-Length: {_response.Message.Length}\n";
            _response.ContentLanguage = "Content-Language: de\n";
            _response.ContentType = "Content-Type: text/plain\n";
            _response.Connection = "Connection: close\nKeep-Alive: timeout=50, max=0\n";
            _response.Charset = "Charset: utf-8\n\n";

            if (data.Contains("POST") || data.Contains("GET") || data.Contains("PUT") || data.Contains("DELETE"))
            {
                string[] dataStrings = data.Split("\n");
                string[] firstSplit = dataStrings[0].Split(" ");


                _request.Method = firstSplit[0];
                _request.Path = firstSplit[1];
                _request.Version = firstSplit[2];

                for (int i = 1; i < 4; i++)
                {
                    _request.Header += dataStrings[i];
                    _request.Header += "\n";
                }

                for (int i = 6; i < dataStrings.Length; i++)
                {
                    _request.Message += dataStrings[i];
                    _request.Message += "\n";
                }
                return;
            }
            else
            {
                _request.Path = "Nicht gesetzt";
                return;
            }




            

        }


        public void AppendMessage(List<string> messageQue)
        {
            messageQue.Add(_request.Message.Trim());

            _response.Status = "201 Created\n";
            _response.Message = "Anfrage wurde Erfolgreich bearbeitet";
            _response.ContentLength = $"Content-Length: {_response.Message.Length}\n";

        }


        public void GetAllMessages(List<string> messageQue)
        {
            string allMessages = "";

            if (messageQue.Count <= 0)
            {
                _response.Status = "404 Not Found\n";
                _response.Message = "Es sind keine Nachrichten vorhanden!";
                _response.ContentLength = $"Content-Length: {_response.Message.Length}\n";

                Console.WriteLine("Es sind keine Nachrichten vorhanden!");
                return;
            }

            foreach (var message in messageQue)
            {
                allMessages += message + "\n";
            }

            _response.Status = "200 OK\n";
            _response.Message = allMessages;
            _response.ContentLength = $"Content-Length: {_response.Message.Length}\n";

        }


        public void GetMessage(List<string> messageQue)
        {
            string[] pathArray = _request.Path.Split("/");

            if (pathArray[2].Length == 0)
            {
                _response.Status = "406 Not Acceptable\n";
                _response.Message = "Kein Index";
                _response.ContentLength = $"Content-Length: {_response.Message.Length}\n";

                Console.WriteLine("Kein Index");
                return;
            }

            int messageNumber = int.Parse(pathArray[2]) - 1;

            if (messageQue.Count <= 0)
            {
                _response.Status = "404 Not Found\n";
                _response.Message = "Es sind keine Nachrichten vorhanden!";
                _response.ContentLength = $"Content-Length: {_response.Message.Length}\n";

                Console.WriteLine("Es sind keine Nachrichten vorhanden!");
                return;
            }
            if (messageQue.Count <= messageNumber)
            {
                _response.Status = "404 Not Found\n";
                _response.Message = "Es gibt noch nicht so viele Messages";
                _response.ContentLength = $"Content-Length: {_response.Message.Length}\n";

                Console.WriteLine("Es gibt noch nicht so viele Messages");
                return;
            }
            if (messageNumber < 0)
            {
                _response.Status = "404 Not Found\n";
                _response.Message = "Index nicht vorhanden";
                _response.ContentLength = $"Content-Length: {_response.Message.Length}\n";

                Console.WriteLine("Index nicht vorhanden");
                return;
            }

            _response.Status = "200 OK\n";
            _response.Message = messageQue[messageNumber];
            _response.ContentLength = $"Content-Length: {_response.Message.Length}\n";

            Console.WriteLine("GET erfolgreich gegettet");
        }


        public void UpdateMessage(List<string> messageQue)
        {
            string[] pathArray = _request.Path.Split("/");

            if (pathArray[2].Length == 0)
            {
                _response.Status = "406 Not Acceptable\n";
                _response.Message = "Kein Index";
                _response.ContentLength = $"Content-Length: {_response.Message.Length}\n";

                Console.WriteLine("Kein Index");
                return;
            }

            int messageNumber = int.Parse(pathArray[2]) - 1;

            if (messageQue.Count <= 0)
            {
                _response.Status = "404 Not Found\n";
                _response.Message = "Es sind keine Nachrichten vorhanden!";
                _response.ContentLength = $"Content-Length: {_response.Message.Length}\n";

                Console.WriteLine("Es sind keine Nachrichten vorhanden!");
                return;
            }

            if (messageQue.Count <= messageNumber)
            {
                _response.Status = "404 Not Found\n";
                _response.Message = "Es gibt noch nicht so viele Messages";
                _response.ContentLength = $"Content-Length: {_response.Message.Length}\n";

                Console.WriteLine("Es gibt noch nicht so viele Messages");
                return;
            }
            if (messageNumber < 0)
            {
                _response.Status = "404 Not Found\n";
                _response.Message = "Index nicht vorhanden";
                _response.ContentLength = $"Content-Length: {_response.Message.Length}\n";

                Console.WriteLine("Index Null nicht vorhanden");
                return;
            }

            messageQue[messageNumber] = _request.Message.Trim();

            _response.Status = "200 OK\n";
            _response.Message = "Message erfolgreich Aktualisiert";
            _response.ContentLength = $"Content-Length: {_response.Message.Length}\n";

        }


        public void DeleteMessage(List<string> messageQue)
        {
            string[] pathArray = _request.Path.Split("/");

            if (pathArray[2].Length == 0)
            {
                _response.Status = "406 Not Acceptable\n";
                _response.Message = "Kein Index";
                _response.ContentLength = $"Content-Length: {_response.Message.Length}\n";

                Console.WriteLine("Kein Index");
                return;
            }

            int messageNumber = int.Parse(pathArray[2]) - 1;

            if (messageQue.Count <= 0)
            {
                _response.Status = "404 Not Found\n";
                _response.Message = "Es sind keine Nachrichten vorhanden!";
                _response.ContentLength = $"Content-Length: {_response.Message.Length}\n";

                Console.WriteLine("Es sind keine Nachrichten vorhanden!");
                return;
            }

            if (messageQue.Count <= messageNumber)
            {
                _response.Status = "404 Not Found\n";
                _response.Message = "Es gibt noch nicht so viele Messages";
                _response.ContentLength = $"Content-Length: {_response.Message.Length}\n";

                Console.WriteLine("Es gibt noch nicht so viele Messages");
                return;
            }
            if (messageNumber < 0)
            {
                _response.Status = "404 Not Found\n";
                _response.Message = "Index nicht vorhanden";
                _response.ContentLength = $"Content-Length: {_response.Message.Length}\n";

                Console.WriteLine("Index Null nicht vorhanden");
                return;
            }

            messageQue.RemoveAt(messageNumber);

            _response.Status = "200 OK\n";
            _response.Message = "Erfolgreich gelöscht";
            _response.ContentLength = $"Content-Length: {_response.Message.Length}\n";

        }


        public void MakeHeader()
        {
            _response.Header = _response.Version 
                               + _response.Status 
                               + _response.Server
                               + _response.ContentLength 
                               + _response.ContentLanguage 
                               + _response.ContentType
                               + _response.Connection 
                               + _response.Charset
                               + _response.Message;
        }


    }
}