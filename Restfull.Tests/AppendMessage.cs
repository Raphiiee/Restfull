 using System.Collections.Generic;
 using NUnit.Framework;
 using Restfull;

namespace Restfull.Tests
{
    [TestFixture]
    public class AppendMessage
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

            _tcp.MakeHeader();
        }

        [Test]
        public void NotNull()
        {
            Assert.NotNull(_tcp); 
            Assert.NotNull(_messageQue);
        }

        [Test]
        public void Store()
        {
            Assert.AreEqual("Hallo", _messageQue[0]);
            Assert.AreEqual("du", _messageQue[1]);
            Assert.AreEqual("!", _messageQue[2]);
        }

        [Test]
        public void Response()
        {
            Assert.AreEqual("HTTP/1.1 201 Created\nServer: MTCG\nContent-Length: 36\nContent-Language: de\nContent-Type: text/plain\nConnection: close\nKeep-Alive: timeout=50, max=0\nCharset: utf-8\n\nAnfrage wurde Erfolgreich bearbeitet", _tcp.GetHeader());
        }

    }
}