using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ClientDTT.User_Control
{
    /// <summary>
    /// Interaction logic for UC_SubQuestions.xaml
    /// </summary>
    public partial class UC_SubQuestions : UserControl
    {
        SupportClass.MediaAct mediaAct = new SupportClass.MediaAct();
        List<TextBlock> txtBlockNameList = new List<TextBlock>();
        List<TextBlock> txtBlockPointList = new List<TextBlock>();
        int time = 0;
        bool isEliminated = false;
        Thread thread;
        SupportClass.Client client;
        ExtendedWindow.EW_PointScreen eW_PointScreen;
        public UC_SubQuestions(SupportClass.Client _client, ExtendedWindow.EW_PointScreen ew_PointScreen)
        {
            InitializeComponent();
            InitControl();

            client = _client;
            eW_PointScreen = ew_PointScreen;
            btnBell.IsEnabled = false;
            //StartTime();
        }

        public void SolveMessage(string message)
        {
            string[] messageList = message.Split('_');
            switch (messageList[0])
            {
                case "0":
                    System.IO.StreamReader file = new System.IO.StreamReader("IP.txt");
                    string ipString = file.ReadLine();
                    int position = int.Parse(file.ReadLine());
                    file.Close();

                    for (int i = 0; i < 4; i++)
                    {
                        txtBlockNameList[i].Text = messageList[i * 2 + 1];
                        txtBlockPointList[i].Text = messageList[i * 2 + 2];
                        if (i == position)
                        {
                            eW_PointScreen.txtBlockMainPoint.Text = txtBlockPoint.Text = txtBlockPointList[i].Text;
                            eW_PointScreen.txtBlockMainStudent.Text = txtBlockNameList[i].Text;
                        }
                    }

                    break;
                case "1":
                    txtBlockQuestion.Text = messageList[1];
                    txtBoxAnswer.Text = string.Empty;
                    break;
                case "2":
                    txtBlockClock.Text = 15.ToString();
                    time = 15;
                    isEliminated = false;
                    thread = OpenBell();
                    thread.Start();
                    break;
                case "3":
                    thread.Abort();
                    btnBell.IsEnabled = false;
                    break;
                case "4":
                    thread = OpenBell();
                    thread.Start();
                    break;
            }
        }

        private void InitControl()
        {
            mediaAct.Upload(imgBackground, "Background.png");
            txtBlockNameList.Add(txtBlockName1);
            txtBlockNameList.Add(txtBlockName2);
            txtBlockNameList.Add(txtBlockName3);
            txtBlockNameList.Add(txtBlockName4);

            txtBlockPointList.Add(txtBlockPoint1);
            txtBlockPointList.Add(txtBlockPoint2);
            txtBlockPointList.Add(txtBlockPoint3);
            txtBlockPointList.Add(txtBlockPoint4);
        }

        public Thread OpenBell()
        {
            if (!isEliminated)
            {
                btnBell.IsEnabled = true;
            }
            
            Thread thread = new Thread(() =>
            {
                while (time != 0)
                {
                    Thread.Sleep(1000);
                    time--;
                    this.Dispatcher.Invoke(() => txtBlockClock.Text = time + "");
                }
                this.Dispatcher.Invoke(() => btnBell.IsEnabled = false);
            });

            thread.IsBackground = true;
            return thread;
        }

        private void btnBell_Click(object sender, RoutedEventArgs e)
        {
            client.Send(6, "0");
            isEliminated = true;
            btnBell.IsEnabled = false;
        }
    }
}
