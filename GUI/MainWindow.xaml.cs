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

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // commands don't work properly with GLWpfControl, so all viewmodel commands need to be triggered manually
        private RenderViewModel renderViewModel = new RenderViewModel();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenTKControl_Render(TimeSpan obj)
        {
            if (renderViewModel.UpdateCommand.CanExecute(obj))
                renderViewModel.UpdateCommand.Execute(obj);
        }

        private void OpenTKControl_Initialized(object sender, EventArgs e)
        {
            OpenTKControl.DataContext = renderViewModel;

            if (renderViewModel.InitializedCommand.CanExecute(OpenTKControl))
                renderViewModel.InitializedCommand.Execute(OpenTKControl);
        }
    }
}
