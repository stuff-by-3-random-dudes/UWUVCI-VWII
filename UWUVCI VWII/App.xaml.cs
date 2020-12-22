using Octokit;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using UWUVCI_VWII.UI.Windows;
using System.IO;
namespace UWUVCI_VWII
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            WaitWindow w = new WaitWindow("Searching for Themes. Please Wait...");
            w.Show();
            GetThemes();
            MainWindow m = new MainWindow(w);
            m.Show();
        }
        private async void GetThemes()
        {
            var github = new GitHubClient(new ProductHeaderValue("UWUVCI-VWII"));
            try
            {
                if (!Directory.Exists("bin"))
                {
                    Directory.CreateDirectory("bin");
                }
                if (!Directory.Exists("bin/vwii"))
                {
                    Directory.CreateDirectory("bin/vwii");
                }
                if (!Directory.Exists("bin/vwii/json"))
                {
                    Directory.CreateDirectory("bin/vwii/json");
                }
                else
                {
                    Directory.Delete("bin/vwii/json", true);
                    Directory.CreateDirectory("bin/vwii/json");
                }
                var content =  await github.Repository.Content.GetAllContents("Hotbrawl20", "UWUVCI-VWII-THEMES", "json");
               foreach(var c in content)
                {
                   
                    using (var client = new WebClient())

                    {
                        var currDir = Directory.GetCurrentDirectory();
                        
                        Directory.SetCurrentDirectory(Path.Combine(currDir, "bin", "vwii", "json"));
                        client.DownloadFileAsync(new Uri(c.DownloadUrl), c.Name);
                        Directory.SetCurrentDirectory(currDir);
                        
                    }
                }
            }catch(Exception e)
            {

            }

            
        }
    }
}
