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
using System.IO;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            renderControl.Focusable = true;
        }

        private void renderControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void MenuItem_Click_FAQ(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(File.ReadAllText("Texts/faq1.txt"));
        }
        private void MenuItem_Click_FAQ2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(File.ReadAllText("Texts/faq2.txt"));
        }
        private void MenuItem_Click_Autors(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(File.ReadAllText("Texts/autors.txt"));
        }
    }
}
