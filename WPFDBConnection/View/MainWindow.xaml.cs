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
using WPFDBConnection.ViewModel;

namespace WPFDBConnection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /*    public partial class MainWindow : Window
        {
            public MainWindow()
            {
                InitializeComponent();
                //Which window to start running the app from
                //Ideally this must be a WPF page and not a window
                this.DataContext = new StudentViewModel();
            }
        }*/
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new StudentViewModel();
        }
    }
}
