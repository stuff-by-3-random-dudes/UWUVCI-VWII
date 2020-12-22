using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using UWUVCI_VWII.UI.Frames;
using UWUVCI_VWII.UI.Windows;

namespace UWUVCI_VWII
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WaitWindow wait;
        public MainWindow(WaitWindow w)
        {
            InitializeComponent();
            load_frame.Content = new StartFrame();
            wait = w;
        }
        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }
        private void Window_Close(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Window_Minimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            min.Background = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));
        }

        private void close_MouseEnter(object sender, MouseEventArgs e)
        {
            close.Background = new SolidColorBrush(Color.FromArgb(150, 255, 100, 100));
        }

        private void close_MouseLeave(object sender, MouseEventArgs e)
        {
            close.Background = new SolidColorBrush(Color.FromArgb(0, 250, 250, 250));
        }

        private void min_MouseLeave(object sender, MouseEventArgs e)
        {
            min.Background = new SolidColorBrush(Color.FromArgb(0, 250, 250, 250));
        }
        private void sett_MouseEnter(object sender, MouseEventArgs e)
        {
            //settings.Background = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));
        }
        private void sett_MouseLeave(object sender, MouseEventArgs e)
        {
           // settings.Background = new SolidColorBrush(Color.FromArgb(0, 250, 250, 250));
        }
        private void settings_Click(object sender, RoutedEventArgs e)
        {
           
        }
        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                   
                    this.DragMove();
                }


            }
            catch (Exception)
            {

            }
       
        }

        private void listCONS_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch ((sender as ListView).SelectedIndex)
            {
                case 0:

                    tbTitleBar.Text = "UWUVCI VWII - THEME INJECT";


                    load_frame.Content = new ThemeList(0,this);

                   
                    
                    break;
            }
        }

        private void vwiiMode_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("UWUVCI AIO.exe");
            Environment.Exit(0);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                wait.Close();
            }catch(Exception ex)
            {

            }
        }
    }
}
