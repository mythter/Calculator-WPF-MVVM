using System;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace Calculator_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            DragMove();
        }
    }
}
