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
using System.Windows.Threading;
using System.Threading;

namespace ClientDTT.User_Control
{
    /// <summary>
    /// Interaction logic for UC_Decode.xaml
    /// </summary>
    public partial class UC_Decode : UserControl
    {
        double time = 0;
        SupportClass.Client client;
        List<TextBlock> txtBlockNameList = new List<TextBlock>();
        List<TextBlock> txtBlockPointList = new List<TextBlock>();
        SupportClass.MediaAct mediaAct = new SupportClass.MediaAct();
        ExtendedWindow.EW_PointScreen eW_PointScreen;
        public UC_Decode(SupportClass.Client _client,ExtendedWindow.EW_PointScreen ew_PointScreen)
        {
            InitializeComponent();
            client = _client;
            eW_PointScreen = ew_PointScreen;
            mediaAct.Upload(imgBackground, "Background.png");
            StartTime();
            txtBlockNameList.Add(txtBlockName1);
            txtBlockNameList.Add(txtBlockName2);
            txtBlockNameList.Add(txtBlockName3);
            txtBlockNameList.Add(txtBlockName4);

            txtBlockPointList.Add(txtBlockPoint1);
            txtBlockPointList.Add(txtBlockPoint2);
            txtBlockPointList.Add(txtBlockPoint3);
            txtBlockPointList.Add(txtBlockPoint4);
        }
        void StartTime()
        {
            Thread thread = new Thread(TimeEvent);
            thread.Start();
        }

        void TimeEvent()
        {
            while (true)
            {
                DateTime start = DateTime.Now;
                if (time > 0)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        txtBlockClock.Text = Math.Round(time, 0).ToString();
                        btnAnswer.IsEnabled = true;
                        txtBoxAnswer.IsEnabled = true;
                    });
                    DateTime end = DateTime.Now;
                    time = time - (double)(end.Ticks - start.Ticks) / 10000000;
                }
                else
                {
                    time = 0;
                    this.Dispatcher.Invoke(() =>
                    {
                        txtBlockClock.Text = Math.Round(time, 0).ToString();
                        btnAnswer.IsEnabled = false;
                        txtBoxAnswer.IsEnabled = false;
                    });
                }
            }
        }

        private void BtnAnswer_Click(object sender, RoutedEventArgs e)
        {
            string message = "0_" + Math.Round(time, 3).ToString() + "_" + txtBoxAnswer.Text;
            txtBlockStudentAnswer.Text = txtBoxAnswer.Text;
            client.Send(5, message);
        }

        public void SolveMessage(string message)
        {
            string[] messageList = message.Split('_');
            switch(messageList[0])
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
                    txtBlockClock.Text = messageList[1];
                    time = int.Parse(messageList[1]);
                    btnAnswer.IsEnabled = true;
                    txtBoxAnswer.IsEnabled = true;
                    break;
            }
        }

        private void TxtBoxAnswer_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                string message = "0_" + Math.Round(time, 3).ToString() + "_" + txtBoxAnswer.Text;
                client.Send(5, message);
                txtBlockStudentAnswer.Text = txtBoxAnswer.Text;
            }
        }
    }
}
