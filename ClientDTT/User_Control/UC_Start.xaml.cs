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

namespace ClientDTT.User_Control
{
    /// <summary>
    /// Interaction logic for UC_Start.xaml
    /// </summary>
    public partial class UC_Start : UserControl
    {
        MediaAct mediaAct = new MediaAct();
        List<TextBlock> txtBlockNameList = new List<TextBlock>();
        List<TextBlock> txtBlockPointList = new List<TextBlock>();
        bool IsQuestionShown = false;
        ExtendedWindow.EW_PointScreen eW_PointScreen;
        public UC_Start(ExtendedWindow.EW_PointScreen ew_PointScreen)
        {
            InitializeComponent();
            eW_PointScreen = ew_PointScreen;
            mediaAct.Upload(TimeVideo, "Start_TimeVideo.mp4");
            mediaAct.Upload(BackgroundImg, "Background.png");
            txtBlockNameList.Add(txtBlockName1);
            txtBlockNameList.Add(txtBlockName2);
            txtBlockNameList.Add(txtBlockName3);
            txtBlockNameList.Add(txtBlockName4);

            txtBlockPointList.Add(txtBlockPoint1);
            txtBlockPointList.Add(txtBlockPoint2);
            txtBlockPointList.Add(txtBlockPoint3);
            txtBlockPointList.Add(txtBlockPoint4);
        }

        public void SolveMessage(string message)
        {
            string[] messageList= message.Split('_');
            int point = 0;
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
                    if (!IsQuestionShown)
                    {
                        mediaAct.Stop(TimeVideo);
                        mediaAct.Play(TimeVideo);
                        IsQuestionShown = true;
                    }
                    point = int.Parse(txtBlockPoint.Text);
                    if (messageList[1] == "1")
                        point += 10;
                    txtBlockPoint.Text = point.ToString();
                    txtBlockQuestion.Text = messageList[2];
                    
                    if (messageList[3] != string.Empty)
                    {
                        mediaAct.Upload(imgQuestion, messageList[3]);
                        imgQuestion.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        imgQuestion.Visibility = Visibility.Hidden;
                    }
                    break;
                case "2":
                    IsQuestionShown = false;
                    mediaAct.Stop(TimeVideo);
                    break;
            }
        }
    }
}
