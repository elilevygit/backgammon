using Contracts;
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

namespace client
{
    /// <summary>
    /// Interaction logic for Sign_in.xaml
    /// </summary>
    public partial class Sign_in : UserControl
    {
       
        public Sign_in()
        {
            InitializeComponent();
           
        }

        private void rgs_Click(object sender, RoutedEventArgs e)
        {
            User u = new User();
            try
            {
                u.UserName = TextBoxname.Text;
            }
            catch (Exception)
            {
                MessageBox.Show("");
                return;
            }
           
         

            ((MainWindow)System.Windows.Application.Current.MainWindow).register(u);
          


        }

        private void signout_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).sign_out();

        }
    }
}
