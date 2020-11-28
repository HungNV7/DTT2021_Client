using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Threading;
using System.IO;
using System.Windows;

namespace ClientDTT.SupportClass
{
    public class Client
    {
        IPEndPoint IP;
        Socket client;
        MainWindow mainWindow;
        string position = "0";
        public Client(MainWindow main)
        {
            mainWindow = main;
            Connect();
        }
        void Connect()
        {
            StreamReader file = new StreamReader("IP.txt");
            string ipString = file.ReadLine();
            position = file.ReadLine();
            file.Close();
            IP = new IPEndPoint(IPAddress.Parse(ipString), 9999);
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                client.Connect(IP);
                Send(0, "");
            }
            catch
            {
                MessageBox.Show("Không thể kết nối! Vui lòng kiểm tra kết nối hoặc khởi động lại!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Connect();
                return;
            }
            Thread listen = new Thread(Receive);
            listen.IsBackground = true;
            listen.Start();
        }

        public void Send(int round, string message)
        {
            string code = "Send: " + round.ToString() + "_" + message + "\n";
            System.IO.StreamWriter streamWriter = System.IO.File.AppendText("Log.txt");
            streamWriter.Write(code);
            streamWriter.Close();

            client.Send(Serialize(round.ToString() + "_" + position + "_" + message));
        }

        void Receive()
        {
            try
            {
                while(true)
                {
                    byte[] data = new byte[1024 * 5000];
                    client.Receive(data);
                    string message = (string)DeSerialize(data);

                    System.IO.StreamWriter streamWriter = System.IO.File.AppendText("Log.txt");
                    streamWriter.Write("Receive: " + message + "\n");
                    streamWriter.Close();

                    mainWindow.Dispatcher.Invoke(() => mainWindow.SolveMessage(message));
                }
            }
            catch
            {
                MessageBox.Show("Lỗi! Vui lòng kiểm tra kết nối hoặc khởi động lại!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Connect();
                return;
            }
        }

        byte[] Serialize(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            return stream.ToArray();
        }

        object DeSerialize(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(stream);
        }
    }
}
