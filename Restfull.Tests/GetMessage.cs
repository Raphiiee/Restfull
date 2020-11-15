using System.Collections.Generic;
using NUnit.Framework;

namespace Restfull.Tests
{
    [TestFixture]
    public class GetMessage
    {
        private Server _tcp;
        List<string> _messageQue = new List<string>();
        List<string> Result = new List<string>();

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

            // Nachrichten Abfragen 1 bis 3
            _tcp.GetData("GET /messages/1 HTTP/1.1\n Host: 127.0.0.1:13000\n User-Agent: insomnia/2020.4.2\nAccept: */*\nContent-Length: 0\n\n");
            _tcp.GetMessage(_messageQue);
            _tcp.MakeHeader();
            Result.Add(_tcp.GetHeader());

            _tcp.GetData("GET /messages/2 HTTP/1.1\n Host: 127.0.0.1:13000\n User-Agent: insomnia/2020.4.2\nAccept: */*\nContent-Length: 0\n\n");
            _tcp.GetMessage(_messageQue);
            _tcp.MakeHeader();
            Result.Add(_tcp.GetHeader());

            _tcp.GetData("GET /messages/3 HTTP/1.1\n Host: 127.0.0.1:13000\n User-Agent: insomnia/2020.4.2\nAccept: */*\nContent-Length: 0\n\n");
            _tcp.GetMessage(_messageQue);
            _tcp.MakeHeader();
            Result.Add(_tcp.GetHeader());

            // Abfrage einer nicht Existenten Nachricht
            _tcp.GetData("GET /messages/5 HTTP/1.1\n Host: 127.0.0.1:13000\n User-Agent: insomnia/2020.4.2\nAccept: */*\nContent-Length: 0\n\n");
            _tcp.GetMessage(_messageQue);
            _tcp.MakeHeader();
            Result.Add(_tcp.GetHeader());

        }

        [Test]
        public void NotNull()
        {
            //Assert.Contains("Hallo\ndu\n!", _tcp.GetHeader());
            Assert.NotNull(_messageQue);
            Assert.NotNull(_tcp.GetHeader());
            _messageQue = new List<string>();
            Result = new List<string>();
        }

        [Test]
        public void Response()
        {
            Assert.AreEqual("HTTP/1.1 200 OK\nServer: MTCG\nContent-Length: 5\nContent-Language: de\nContent-Type: text/plain\nConnection: close\nKeep-Alive: timeout=50, max=0\nCharset: utf-8\n\nHallo", Result[0]);
            Assert.AreEqual("HTTP/1.1 200 OK\nServer: MTCG\nContent-Length: 2\nContent-Language: de\nContent-Type: text/plain\nConnection: close\nKeep-Alive: timeout=50, max=0\nCharset: utf-8\n\ndu", Result[1]);
            Assert.AreEqual("HTTP/1.1 200 OK\nServer: MTCG\nContent-Length: 1\nContent-Language: de\nContent-Type: text/plain\nConnection: close\nKeep-Alive: timeout=50, max=0\nCharset: utf-8\n\n!", Result[2]);
            _messageQue = new List<string>();
            Result = new List<string>();
        }

        [Test]
        public void WrongIndex()
        {
            Assert.AreEqual("HTTP/1.1 404 Not Found\nServer: MTCG\nContent-Length: 36\nContent-Language: de\nContent-Type: text/plain\nConnection: close\nKeep-Alive: timeout=50, max=0\nCharset: utf-8\n\nEs gibt noch nicht so viele Messages", Result[3]);
        }
    }
}