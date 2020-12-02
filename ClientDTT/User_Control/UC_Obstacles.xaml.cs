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

namespace ClientDTT.User_Control
{
    /// <summary>
    /// Interaction logic for UC_Obstacles.xaml
    /// </summary>
    public partial class UC_Obstacles : UserControl
    {
        SupportClass.MediaAct mediaAct = new SupportClass.MediaAct();
        List<TextBlock> txtBlockNameList = new List<TextBlock>();
        List<TextBlock> txtBlockPointList = new List<TextBlock>();
        int time = 0;
        bool IsEliminated = false;
        SupportClass.Client client;
        ExtendedWindow.EW_PointScreen eW_PointScreen;

        public UC_Obstacles(SupportClass.Client _client, ExtendedWindow.EW_PointScreen ew_PointScreen)
        {
            InitializeComponent();
            InitControl();

            client = _client;
            eW_PointScreen = ew_PointScreen;

            StartTime();
        }

        void StartTime()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (time > 0) time--;
            txtBlockClock.Text = time.ToString();
            if (time == 0)
            {
                btnAnswer.IsEnabled = false;
                txtBoxAnswer.IsEnabled = false;
            }
        }

        private void BtnAnswer_Click(object sender, RoutedEventArgs e)
        {
            client.Send(2, "1_" + txtBoxAnswer.Text);
            txtBlockStudentAnswer.Text = txtBoxAnswer.Text;
        }

        private void BtnBell_Click(object sender, RoutedEventArgs e)
        {
            client.Send(2, "0");
            IsEliminated = true;
            btnBell.IsEnabled = false;
            btnAnswer.IsEnabled = false;
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
                    if (!IsEliminated)
                    {
                        txtBoxAnswer.IsEnabled = true;
                        btnAnswer.IsEnabled = true;
                    }
                    break;
                case "3":
                    IsEliminated = false;
                    btnBell.IsEnabled = true;
                    btnAnswer.IsEnabled = true;
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

        private void TxtBoxAnswer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                client.Send(2, "1_" + txtBoxAnswer.Text);
                txtBlockStudentAnswer.Text = txtBoxAnswer.Text;
            }
        }
    }
}