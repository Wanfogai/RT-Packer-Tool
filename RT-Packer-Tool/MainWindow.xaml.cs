using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
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

namespace RT_Packer_Tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string PackPath ;
        public MainWindow()
        {
            InitializeComponent();
            if (!Directory.Exists(@"Temp"))
                new DirectoryInfo(@"Temp").Create();
            else
                ClearTemp();
        }

        private void MOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".nw";
            fileDialog.Filter = "Пак файл (*.nw)|*.nw";
            if (fileDialog.ShowDialog() == true)
            {
                if (fileDialog.FileName.EndsWith(".nw"))
                {
                    PackPath = fileDialog.FileName;
                    LoadPack();
                }
                else 
                {
                    MessageBox.Show("Формат файла не верен выберете файл .nw");
                }
            }
        }

        public void ClearTemp() 
        {
            foreach (FileInfo file in new DirectoryInfo(@"Temp/").GetFiles())
            {
                file.Delete();
            }
        }

        public void LoadPack() 
        {
            ImageBox.Items.Clear();
            SoundBox.Items.Clear();
            using (ZipArchive archive = ZipFile.OpenRead(PackPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.Name.EndsWith(".png"))
                    {
                        ImageBox.Items.Add(entry.Name);
                    }
                    if (entry.Name.EndsWith(".ogg"))
                    {
                        SoundBox.Items.Add(entry.Name);
                    }
                }
            }
        }

        public void ChangeImage() 
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
                        ImageSourceConverter converter = new ImageSourceConverter();
                        Sprite.SetValue(Image.SourceProperty, converter.ConvertFromString($@"Temp/{entry.Name}"));
                    }
                }
            }
        }

        private void ImageBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeImage();
        }
    }
}
