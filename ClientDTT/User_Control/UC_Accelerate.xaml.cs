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
    /// Interaction logic for UC_Accelerate.xaml
    /// </summary>
    public partial class UC_Accelerate : UserControl
    {
        double time = 0;
        SupportClass.Client client;
        List<TextBlock> txtBlockNameList = new List<TextBlock>();
        List<TextBlock> txtBlockPointList = new List<TextBlock>();
        SupportClass.MediaAct mediaAct = new SupportClass.MediaAct();
        ExtendedWindow.EW_PointScreen eW_PointScreen;
        public UC_Accelerate(SupportClass.Client _client,ExtendedWindow.EW_PointScreen ew_PointScreen)
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
            string message = time.ToString() + "_" + txtBoxAnswer.Text.ToUpper();

            System.IO.StreamWriter streamWriter = System.IO.File.AppendText("Log.txt");
            streamWriter.Write(message + "\n");
            streamWriter.Close();

            txtBlockStudentAnswer.Text = txtBoxAnswer.Text.ToUpper();
            client.Send(3, message);
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
                    txtBlockClock.Text = 0.ToString();
                    time = 30;
                    txtBoxAnswer.Text = string.Empty;
                    txtBoxAnswer.IsEnabled = true;
                    btnAnswer.IsEnabled = true;
                    break;
            }
        }

        private void TxtBoxAnswer_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                string message = time.ToString() + "_" + txtBoxAnswer.Text.ToUpper();

                txtBlockStudentAnswer.Text = txtBoxAnswer.Text.ToUpper();
                client.Send(3, message);
            }
        }
    }
}
