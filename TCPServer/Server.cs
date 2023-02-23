using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    internal class Server
    {
        public delegate void Message(string Message);
        public event Message? MessageReceived;
        public string MyIP {
            get 
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return "No network adapters with an IPv4 address in the system!";
            } 
        }
        public async void Start()
        {
            TcpListener? server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Any;

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String? data = null;

                // Enter the listening loop.
                await Task.Run(() => 
                {
                    while (true)
                    {

                        // Perform a blocking call to accept requests.
                        // You could also use server.AcceptSocket() here.
                        using TcpClient client = server.AcceptTcpClient();

                        data = null;

                        // Get a stream object for reading and writing
                        NetworkStream stream = client.GetStream();

                        int i;

                        // Loop to receive all the data sent by the client.
                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            string sAndR = "";
                            // Translate data bytes to a ASCII string.
                            data = System.Text.Encoding.UTF8.GetString(bytes, 0, i);
                            sAndR += ($"Принято: {data}\n");

                            // Process the data sent by the client.
                            data = data.ToUpper();

                            byte[] msg = System.Text.Encoding.UTF8.GetBytes(data);

                            // Send back a response.
                            stream.Write(msg, 0, msg.Length);
                            sAndR += ($"Отправлено: {data}");
                            MessageReceived?.Invoke(sAndR);
                        }
                    }
                });
            }
            catch (SocketException e)
            {
                MessageReceived?.Invoke($"SocketException: {e}");
            }
            finally
            {
                server.Stop();
            }
        }
    }
}
