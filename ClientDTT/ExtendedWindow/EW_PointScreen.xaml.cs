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

namespace ClientDTT.ExtendedWindow
{
    /// <summary>
    /// Interaction logic for EW_PointScreen.xaml
    /// </summary>
    public partial class EW_PointScreen : Window
    {
        SupportClass.MediaAct mediaAct = new SupportClass.MediaAct();
        public EW_PointScreen()
        {
            InitializeComponent();
            ImageBrush Background1 = new ImageBrush();
            mediaAct.Upload(Background1, "Background.png");
            this.Background = Background1;
            this.Show();
        }
    }
}
