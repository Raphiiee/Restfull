using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Restfull.Structs;

namespace Restfull
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                List<string> messageQue = new List<string>();
                TcpClass tcp = new TcpClass();

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        

                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        data = data.ToUpper();
                        Console.WriteLine("Received: {0}", data);

                        // Process the data sent by the client.
                        

                        tcp.GetData(data);

                        if (tcp.GetPath().Contains("/MESSAGES"))
                        {
                            
                            if (tcp.GetMethod() == "GET")
                            {
                                
                                if (tcp.GetPath() == "/MESSAGES")
                                {
                                    tcp.GetAllMessages(messageQue);
                                }
                                else
                                {
                                    tcp.GetMessage(messageQue);
                                }
                                
                            }
                            else if (tcp.GetMethod() == "POST")
                            {
                                tcp.AppendMessage(messageQue);
                                Console.WriteLine("POST THAT!");
                            }
                            else if (tcp.GetMethod() == "PUT")
                            {
                                if (tcp.GetPath().Contains("/MESSAGES/"))
                                {
                                    tcp.UpdateMessage(messageQue);
                                    Console.WriteLine("PUT THAT !");
                                }
                                else
                                {
                                    Console.WriteLine("Not");
                                }
                                
                            }
                            else if (tcp.GetMethod() == "DELETE")
                            {
                                tcp.DeleteMessage(messageQue);
                                Console.WriteLine("DELETE THAT !");
                            }

                        }
                        else
                        {
                            Console.WriteLine("Falscher Pfad");
                        }

                        tcp.MakeHeader();

                        Byte[] reply = System.Text.Encoding.ASCII.GetBytes(tcp.GetHeader());
                        stream.Write(reply, 0, reply.Length);
                        //writer.Write("\r\n");
                        stream.Flush();
                        Console.WriteLine("Sent: {0}", tcp.GetHeader());

                    }

                    

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }
}
