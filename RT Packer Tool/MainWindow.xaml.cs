using Microsoft.Win32;
using System;
using System.IO;
using System.IO.Compression;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RT_Packer_Tool
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string PackPath;
        public MainWindow()
        {
            InitializeComponent();
            if (!Directory.Exists(@"Temp"))
            {
                new DirectoryInfo(@"Temp").Create();
                
            }
        }

        private void MExit_Click(object sender, RoutedEventArgs e)
        {
            ClearTemp("");
            Application.Current.Shutdown();
        }

        private void MOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog PackCh = new OpenFileDialog();
            PackCh.DefaultExt = ".nw";
            PackCh.Filter = "Pack File|*.nw";
            if (PackCh.ShowDialog() == true)
            {
                if (PackCh.FileName.EndsWith(".nw"))
                {
                    ImageBox.Items.Clear();
                    SoundBox.Items.Clear();
                    PackPath = PackCh.FileName;
                    LoadFiles();
                }
                else
                    MessageBox.Show("Выбран неверный файл, выберете вайл .nw");
            }
        }

        public void LoadFiles()
        {
            using (ZipArchive archive = ZipFile.OpenRead(PackPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.Name.EndsWith(".ogg"))
                    {
                        SoundBox.Items.Add(entry.Name);
                    }
                    if (entry.Name.EndsWith(".png"))
                    {
                        ImageBox.Items.Add(entry.Name);
                    }
                }
            }
        }

        private void ImageBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            using (ZipArchive archive = ZipFile.OpenRead(PackPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.Name == ImageBox.SelectedItem.ToString())
                    {
                        if (!File.Exists($@"Temp/{entry.Name}"))
                        {
                            entry.ExtractToFile($@"Temp/{entry.Name}");
                        }
                        ImageSourceConverter imgs = new ImageSourceConverter();
                        Sprite.SetValue(Image.SourceProperty, imgs.ConvertFromString($@"Temp/{entry.Name}"));
                        break;
                    }
                }
            }
        }

        public void ClearTemp(string excep) 
        {
            foreach (FileInfo file in new DirectoryInfo(@"Temp").EnumerateFiles())
            {
                if (file.Name != excep)
                {
                    file.Delete();
                }
            }
        }
    }
}
