using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
using System.IO.Compression;

namespace UWUVCI_VWII.UI.Frames
{
    /// <summary>
    /// Interaktionslogik für ThemeFrame.xaml
    /// </summary>
    public partial class ThemeFrame : Page, IDisposable
    {
        MainWindow mw;
        int Region;
        string ipurl = "";
        string app = "22";
        VWiiTheme Theme;
        public ThemeFrame(int region, VWiiTheme theme, MainWindow m)
        {
            InitializeComponent();
            string[] regions = new string[3] { "EU", "US", "JP" };
            tn.Text = $"Name:{theme.Name} \tRegion: {regions[region]}";
            mw = m;
            Region = region;
            Theme = theme;
            if(region == 1)
            {
                app = "1f";
            }else if(region == 2)
            {
                app = "1c";
            }
        }

        public void Dispose()
        {
        
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mw.load_frame.Content = new ThemeList(Region, mw);
            this.Dispose();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //is IP entered
            if(!string.IsNullOrEmpty(ip.Text))
            {
                Directory.CreateDirectory("bin/vwii/temp");
                //Check if FTPIIu Everywhere is running
                if (FTPConnect(sender, e))
                {
                    //Download Theme
                    using(var client = new WebClient())
                    {
                        if(Region == 1) 
                        {
                            client.DownloadFile(Theme.downloadUS, "bin/vwii/temp/theme.zip");
                        }
                        else if(Region == 2)
                        {
                            client.DownloadFile(Theme.downloadJP, "bin/vwii/temp/theme.zip");
                        }
                        else
                        {
                            client.DownloadFile(Theme.downloadEU, "bin/vwii/temp/theme.zip");
                        }  
                    }
                    //Extract Theme
                    ZipFile.ExtractToDirectory("bin/vwii/temp/theme.zip", "bin/vwii/temp/theme");
                    DirectoryInfo d = new DirectoryInfo("bin/vwii/temp/theme");
                    FileInfo[] infos = d.GetFiles();
                    foreach(FileInfo f in infos)
                    {
                        if (f.Extension.ToLower().EndsWith("mym"))
                        {
                            File.Move(f.FullName, System.IO.Path.Combine("bin/vwii/temp","theme.mym"));
                        }
                        
                    }
                    
                    //Get Backup if not already gotten
                    if (!File.Exists($"bin/vwii/backup/000000{app}.app"))
                    {
                        DownloadFile($"slccmpt01/title/00000001/00000002/content/000000{app}.app");
                    }
                    //Inject Theme
                    string backupdir = Directory.GetCurrentDirectory();
                    Directory.SetCurrentDirectory(System.IO.Path.Combine(backupdir, "bin", "vwii", "Tools"));
                    Process p = new Process();
                    p.StartInfo.FileName = $"ThemeMii.exe";
                    p.StartInfo.Arguments = $"\"{System.IO.Path.Combine(backupdir, "bin", "vwii", "temp", "theme.mym")}\" \"{System.IO.Path.Combine(backupdir, "bin", "vwii", "backup", $"000000{app}.app ")}\" \"{System.IO.Path.Combine(backupdir, "bin", "vwii", "temp", "injected.csm")}\"";
                    p.Start();
                    p.WaitForExit();
                    Thread.Sleep(10000);
                    Directory.SetCurrentDirectory(backupdir);
                    File.Delete("bin/vwii/Tools/temptemp.mym");
                    File.Move("bin/vwii/temp/injected.csm", $"bin/vwii/temp/000000{app}.app");
                    using (var client = new WebClient())
                    {
                        try
                        {
                            client.Credentials = new NetworkCredential("UWUVCI", "VWII.THEME.INJECTOR");
                            client.UploadFile(ipurl + $"slccmpt01/title/00000001/00000002/content/000000{app}.app", WebRequestMethods.Ftp.UploadFile, $"bin/vwii/temp/000000{app}.app");
                            MessageBox.Show("Successfully Injected Theme");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Something went wrong uploading the file");
                        }


                    }
                    
                    File.Delete($"bin/vwii/temp/000000{app}.app");
                    Directory.Delete("bin/vwii/temp", true);
                    Directory.Delete("bin/vwii/Tools/temp", true);
                }

            }
            else
            {
                MessageBox.Show("Enter your WiiU IP");
            }

            //Done
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //is IP entered
            if (!string.IsNullOrEmpty(ip.Text))
            { //Check if FTPIIu Everywhere is running
                if (FTPConnect(sender, e))
                {
                    //Check if Backup Exists
                    if (File.Exists($"bin/vwii/backup/000000{app}.app"))
                    {
                        //Inject Backup
                        using (var client = new WebClient())
                        {
                            try
                            {
                                client.Credentials = new NetworkCredential("UWUVCI", "VWII.THEME.INJECTOR");
                                client.UploadFile(ipurl + $"slccmpt01/title/00000001/00000002/content/000000{app}.app", WebRequestMethods.Ftp.UploadFile, $"bin/vwii/backup/000000{app}.app");
                                MessageBox.Show("Successfully restored from backup");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("There was an Issue with transfering the file");
                            }


                        }
                    }
                    else
                    {
                        MessageBox.Show($"There is no backup in bin/vwii/backup.\nMake sure to have your backup 000000{app}.app file in said folder");
                    }
                    
                }

            }
            else
            {
                MessageBox.Show("Enter your WiiU IP");
            }


            //Done
        }
        
        private bool FTPConnect(object sender, EventArgs e)
        {
            string ftp_url = "ftp://";
            ftp_url += ip.Text;
            ipurl = ftp_url + ":21/";
            try
            {
                if (DoesFtpDirectoryExist("slccmpt01"))
                {
                    MessageBox.Show("FTP Connection successfull");
                    return true;
                }
                else
                {
                    MessageBox.Show("FTP connection failed.\nMake sure the IP is correct, your WiiU and PC are in the same network and that you are using FTPIIU EVERYWHERE");
                    return false;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("FTP connection failed.\nMake sure the IP is correct, your WiiU and PC are in the same network and that you are using FTPIIU EVERYWHERE");
                return false;
            }
        }
        public bool DoesFtpDirectoryExist(string dirPath)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ipurl + dirPath);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                request.Abort();
                return true;
            }
            catch (WebException ex)
            {
                return false;
            }
        }
        public void DownloadFile(string dirPath)
        {
            int bytesRead = 0;
            byte[] buffer = new byte[2048];
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ipurl + dirPath);
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential("UWUVCI", "VWII.THEME.INJECTOR");

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            Directory.CreateDirectory("bin/vwii/backup");
            using (Stream fileStream = File.Create($"bin/vwii/backup/000000{app}.app"))
            {
                responseStream.CopyTo(fileStream);
                fileStream.Close();
            }
            MessageBox.Show($"Download Complete, status {response.StatusDescription}");
            responseStream.Close();
            response.Close();
            /* 
             BinaryReader reader = new BinaryReader(responseStream);
             File.WriteAllBytes("00000022.app", reader.ReadBytes((int)responseStream.Length));

             

             reader.Close();
             response.Close();*/
        }
    }
}
