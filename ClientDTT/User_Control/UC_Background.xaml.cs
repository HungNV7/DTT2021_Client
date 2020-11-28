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

namespace ClientDTT.User_Control
{
    /// <summary>
    /// Interaction logic for UC_Background.xaml
    /// </summary>
    public partial class UC_Background : UserControl
    {
        SupportClass.MediaAct mediaAct = new SupportClass.MediaAct();
        public UC_Background()
        {
            InitializeComponent();
            mediaAct.Upload(BackgroundImg, "Background.png");
        }
    }
}
