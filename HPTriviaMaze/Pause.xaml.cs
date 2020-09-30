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
using System.Windows.Shapes;

namespace HPTriviaMaze
{
    /// <summary>
    /// Interaction logic for Pause.xaml
    /// </summary>
    public partial class Pause : Window
    {
        public Pause()
        {
            InitializeComponent();
        }

        private void btnResume_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
