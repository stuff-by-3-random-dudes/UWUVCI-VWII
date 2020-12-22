using System;
using System.Collections.Generic;
using System.IO;
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
using WiiTheme;
using Path = System.IO.Path;

namespace UWUVCI_VWII.UI.Frames
{
    /// <summary>
    /// Interaktionslogik für ThemeList.xaml
    /// </summary>
    public partial class ThemeList : Page, IDisposable
    {
        List<VWiiTheme> Themes = new List<VWiiTheme>();
        int Left = 10;
        int Top = 10;
        int Region;
        MainWindow mw = new MainWindow(null);
        public ThemeList(int region, MainWindow m)
        {
           
            InitializeComponent();
            mw = m;
            switch (region)
            {
                case 1:
                    usR.IsChecked = true;
                    break;
                case 2:
                    jpR.IsChecked = true;
                    break;
            }
            ReadAllThemes(region);
            doThemesExist();
            Region = region;
        }

        private void doThemesExist()
        {
            if(Themes.Count <= 0)
            {
                NoTheme.Visibility = Visibility.Visible;
                ThemeDisplay.Visibility = Visibility.Hidden;
            }
            else
            {
                foreach(VWiiTheme t in Themes)
                {
                    drawThemeButton(t);
                }
            }
        }

        

        private void ReadAllThemes(int region)
        {
            int eu = 0, us = 1, jp = 2;
            foreach (string file in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "bin", "vwii", "json")))
            {
                var theme = VWiiTheme.LoadFromJSON(file);
                if (eu == region) if (theme.worksEU) Themes.Add(theme);
                if (us == region) if (theme.worksUS) Themes.Add(theme); 
                if (jp == region) if (theme.worksJP) Themes.Add(theme); 
            }
        }

        private void drawThemeButton(VWiiTheme theme)
        {
            if (Left >= 1130)
            {
                Top += 170;
                Left = 10;
            }
            Button b = new Button();
            StackPanel s = new StackPanel();
            Image a = new Image();

            a.Source = new BitmapImage(new Uri($"{theme.IMGURL}", UriKind.RelativeOrAbsolute));
            TextBlock ba = new TextBlock();
            ba.Text = theme.Name;
            ba.HorizontalAlignment = HorizontalAlignment.Center;
            s.Orientation = Orientation.Vertical;
            s.Children.Add(a);
            s.Children.Add(ba);
            b.Margin = new Thickness(Left, Top, 0, 0);
            Left += 280;
            b.Content = s;
            b.Width = 260;
            b.Height = 160;
            b.VerticalAlignment = VerticalAlignment.Top;
            b.HorizontalAlignment = HorizontalAlignment.Left;
            b.Click += B_Click;
            stuff.Children.Add(b);
        }
        private void B_Click(object sender, RoutedEventArgs e)
        {
            string themename = (((sender as Button).Content as StackPanel).Children[1] as TextBlock).Text;
            IEnumerable<VWiiTheme> toUse = Themes.Where(t => t.Name == themename);
            if(toUse.Count() == 1)
            {
                foreach(VWiiTheme t in toUse)
                {
                    mw.load_frame.Content = new ThemeFrame(Region, t, mw);
                    break;
                }
                this.Dispose();
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            mw.load_frame.Content = new ThemeList(1, mw);
            this.Dispose();
        }

        private void RadioButton_Click_1(object sender, RoutedEventArgs e)
        {
            mw.load_frame.Content = new ThemeList(0, mw);
            this.Dispose();
        }

        private void jpR_Click(object sender, RoutedEventArgs e)
        {
            mw.load_frame.Content = new ThemeList(2, mw);
            this.Dispose();
        }

        public void Dispose()
        {
           
        }
    }
}
