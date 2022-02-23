using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Wpf;

namespace GUI
{
    internal class RenderViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private RelayCommand initializedCommand;
        public RelayCommand InitializedCommand
        {
            get
            {
                return initializedCommand ?? (initializedCommand = new RelayCommand(obj => init(obj as GLWpfControl), 
                                                                                        obj => obj is GLWpfControl));
            }
        }
        private RelayCommand updateCommand;
        public RelayCommand UpdateCommand
        {
            get
            {
                return updateCommand ?? (updateCommand = new RelayCommand(obj => update((TimeSpan)obj),
                                                                          obj => obj is TimeSpan));
            }
        }
        private void init(GLWpfControl control)
        {
            GLWpfControlSettings settings = new GLWpfControlSettings
            {
                MajorVersion = 4,
                MinorVersion = 0
            };
            control.Start(settings);
        }
        private void update(TimeSpan deltaTime)
        {
            render(deltaTime);
        }
        private void render(TimeSpan deltaTime)
        {
            GL.ClearColor(Color4.Blue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
    }
}
