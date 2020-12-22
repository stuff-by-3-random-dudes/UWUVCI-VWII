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

namespace UWUVCI_VWII.UI.Frames
{
    /// <summary>
    /// Interaktionslogik für StartFrame.xaml
    /// </summary>
    public partial class StartFrame : Page
    {
        public StartFrame()
        {
            InitializeComponent();
            tb.Content += "\n";
            tb2.Content += "\nTo return to UWUVCI AIO press the WiiU Mode button!";
        }
    }
}
