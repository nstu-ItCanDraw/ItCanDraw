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
using Geometry;
using LinearAlgebra;

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
            fuu();
            renderControl.Focusable = true;
            KeyDown += (sender, args) => { if (renderControl.IsFocused) renderControl.OnKeyDown(sender, args); };
            KeyUp += (sender, args) => { if (renderControl.IsFocused) renderControl.OnKeyUp(sender, args); };
        }

        void fuu()
        {
            Vector2 p1 = new Vector2(0, 0);
            Vector2 p2 = new Vector2(1, 0);

            IGeometry obj = FigureFactory.CreatePolyline(new List<Vector2>() { p1, p2 });
            obj.Transform.RotationDegrees = 45;

            int y = 0;
        }
    }
}
