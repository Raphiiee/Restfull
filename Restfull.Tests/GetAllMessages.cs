using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using NUnit.Framework;

namespace Restfull.Tests
{
    class GetAllMessages
    {
        private Server _tcp;
        List<string> _messageQue = new List<string>();

        [SetUp]
        public void Setup()
        {
            // Nachrichten Hinzufügen
            _tcp = new Server();
            _tcp.GetData("POST /messages HTTP/1.1\n Host: 127.0.0.1:13000\n User-Agent: insomnia/2020.4.2\nAccept: */*\nContent-Length: 5\n\nHallo");
            _tcp.AppendMessage(_messageQue);

            _tcp.GetData("POST /messages HTTP/1.1\n Host: 127.0.0.1:13000\n User-Agent: insomnia/2020.4.2\nAccept: */*\nContent-Length: 2\n\ndu");
            _tcp.AppendMessage(_messageQue);

            _tcp.GetData("POST /messages HTTP/1.1\n Host: 127.0.0.1:13000\n User-Agent: insomnia/2020.4.2\nAccept: */*\nContent-Length: 1\n\n!");
            _tcp.AppendMessage(_messageQue);

            // Alle Nachrichten Abfragen
            _tcp.GetAllMessages(_messageQue);
            _tcp.MakeHeader();
        }

        [Test]
        public void NotNull()
        {
            //Assert.Contains("Hallo\ndu\n!", _tcp.GetHeader());
            Assert.NotNull(_messageQue);
            Assert.NotNull(_tcp.GetHeader());
            _messageQue = new List<string>();
        }

        [Test]
        public void Response()
        {
            Assert.AreEqual("HTTP/1.1 200 OK\nServer: MTCG\nContent-Length: 11\nContent-Language: de\nContent-Type: text/plain\nConnection: close\nKeep-Alive: timeout=50, max=0\nCharset: utf-8\n\nHallo\ndu\n!\n", _tcp.GetHeader());
        }

    }
}
