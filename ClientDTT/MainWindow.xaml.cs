using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClientDTT.SupportClass;
using ClientDTT.User_Control;

namespace ClientDTT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UC_Background uC_Background = new UC_Background();
        UC_Start uC_Start;
        UC_Decode uC_Decode;
        Client client;
        UC_Finish uC_Finish;
        UC_Accelerate uC_Accelerate;
        UC_Obstacles uC_Obstacles;
        UC_SubQuestions uC_SubQuestions;
        ExtendedWindow.EW_PointScreen eW_PointScreen = new ExtendedWindow.EW_PointScreen();
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.Content = uC_Background;
            client = new Client(this);
            uC_Start = new UC_Start(eW_PointScreen);
            uC_Decode = new UC_Decode(client, eW_PointScreen);
            uC_Finish = new UC_Finish(client, eW_PointScreen);
            uC_Accelerate = new UC_Accelerate(client, eW_PointScreen);
            uC_Obstacles = new UC_Obstacles(client, eW_PointScreen);
            uC_SubQuestions = new UC_SubQuestions(client, eW_PointScreen);
        }
        public void SolveMessage(string message)
        {
            switch (message[0])
            {
                case '0':
                    System.IO.StreamReader file = new System.IO.StreamReader("IP.txt");
                    string ipString = file.ReadLine();
                    int position = int.Parse(file.ReadLine());
                    file.Close();
                    string[] messageList = (message.Substring(2)).Split('_');
                    for (int i = 0; i < 4; i++)
                    {
                        if (i == position)
                        {
                            eW_PointScreen.txtBlockMainPoint.Text = messageList[i * 2 + 2];
                            eW_PointScreen.txtBlockMainStudent.Text = messageList[i * 2 + 1];
                        }
                    }
                    this.Content = uC_Background;
                    break;
                case '1':
                    this.Content = uC_Start;
                    uC_Start.SolveMessage(message.Substring(2));
                    break;
                case '2':
                    this.Content = uC_Obstacles;
                    uC_Obstacles.SolveMessage(message.Substring(2));
                    break;
                case '3':
                    this.Content = uC_Accelerate;
                    uC_Accelerate.SolveMessage(message.Substring(2));
                    break;
                case '4':
                    this.Content = uC_Finish;
                    uC_Finish.SolveMessage(message.Substring(2));
                    break;
                case '5':
                    this.Content = uC_Decode;
                    uC_Decode.SolveMessage(message.Substring(2));
                    break;
                case '6':
                    this.Content = uC_SubQuestions;
                    uC_SubQuestions.SolveMessage(message.Substring(2));
                    break;
            }
        }
    }
}
