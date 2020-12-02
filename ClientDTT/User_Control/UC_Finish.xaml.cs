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
using System.Windows.Threading;

namespace ClientDTT.User_Control
{
    /// <summary>
    /// Interaction logic for UC_Finish.xaml
    /// </summary>
    public partial class UC_Finish : UserControl
    {
        SupportClass.MediaAct mediaAct = new SupportClass.MediaAct();
        List<TextBlock> txtBlockNameList = new List<TextBlock>();
        List<TextBlock> txtBlockPointList = new List<TextBlock>();
        int time = 0;
        SupportClass.Client client;
        ExtendedWindow.EW_PointScreen eW_PointScreen;
        public UC_Finish(SupportClass.Client _client,ExtendedWindow.EW_PointScreen ew_PointScreen)
        {
            InitializeComponent();
            client = _client;
            eW_PointScreen = ew_PointScreen;
            mediaAct.Upload(BackgroundImg, "Background.png");

            txtBlockNameList.Add(txtBlockName1);
            txtBlockPointList.Add(txtBlockPoint1);
            txtBlockNameList.Add(txtBlockName2);
            txtBlockPointList.Add(txtBlockPoint2);
            txtBlockNameList.Add(txtBlockName3);
            txtBlockPointList.Add(txtBlockPoint3);
            txtBlockNameList.Add(txtBlockName4);
            txtBlockPointList.Add(txtBlockPoint4);
            btnAnswer.IsEnabled = false;
            //StartTime();

        }

        //void StartTime()
        //{
        //    DispatcherTimer time = new DispatcherTimer();
        //    time.Interval = TimeSpan.FromSeconds(1);
        //    time.Tick += TimeEvent;
        //    time.Start();
        //}
        //void TimeEvent(object sender, EventArgs e)
        //{
        //    if (time > 0)
        //        time--;
        //    txtBlockClock.Text = time.ToString();
        //    if (time == 0)
        //    {
        //        btnAnswer.IsEnabled = false;
        //    }
        //    else btnAnswer.IsEnabled = true;
        //}

        private void BtnAnswer_Click(object sender, RoutedEventArgs e)
        {
            client.Send(4, "");
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
                    break;
                case "2":
                    time = int.Parse(messageList[1]);
                    txtBlockClock.Text = messageList[1];
                    OpenBell();
                    break;
            }
        }

        public void OpenBell()
        {
            btnAnswer.IsEnabled = true;
            Thread thread = new Thread(() =>
            {
                while (time != 0)
                {
                    Thread.Sleep(1000);
                    time--;
                    this.Dispatcher.Invoke(() => txtBlockClock.Text = time + "");
                }
                this.Dispatcher.Invoke(() => btnAnswer.IsEnabled = false);        
            });
            
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
