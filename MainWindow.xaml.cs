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
using System.Drawing;
using System.IO;
using System.Media;

namespace TestingWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        MediaPlayer mp = new MediaPlayer();
        double speed = 1.0;
        public MainWindow()
        {
            InitializeComponent();
            GetImages(@"C:\Users\Игорь\Desktop\done\NCE_content\Characters\Monika", images);
            GetImages(@"C:\Users\Игорь\Desktop\done\NCE_content\Backgrounds",backgrounds);
            GetTexts(@"C:\Users\Игорь\Desktop\done\NCE_content\Text\First_scene.txt", texts);
            Background.Source = backgrounds[0];
            Text.Content = texts[0];
            //C:\Users\Игорь\Desktop\done\NCE_content\Backgrounds
            mp.Open(new Uri(@"C:\Users\Игорь\Desktop\Monika\doki_doki_literature_club_ost_ohayou_sayori_-3991409475570679520 (online-audio-converter.com).wav", UriKind.Relative));
            mp.Volume = 25f;
            mp.SpeedRatio = speed;
            mp.MediaEnded += new EventHandler(repeat);
            mp.Play();
            Talking();
            AutoSctroll();

        }
        async void Talking()
        {
            while (true)
            {
                await Task.Delay(75);
                Character.Source = images[++talk % 3];
            }
        }
        async void AutoSctroll()
        {
            while (true)
            {
                for(int i = 0; i < texts.Count; i++)
                {
                    Text.Content = "";
                    char[] buff = texts[i].ToCharArray();
                    for(int j = 0; j < buff.Length; j++)
                    {
                        Text.Content += buff[j].ToString();
                        if(buff[j]!=' ') await Task.Delay(40);

                    }
                    await Task.Delay(500);
                }
                
                //Text_MouseLeftButtonDown(this, null);
                
            }
        }
        void repeat(object sender, EventArgs e)
        {
            mp.Open(new Uri(@"C:\Users\Игорь\Desktop\Monika\doki_doki_literature_club_ost_ohayou_sayori_-3991409475570679520 (online-audio-converter.com).wav", UriKind.Relative));
            mp.Volume = 25f;
            mp.SpeedRatio = speed;
            mp.Play();
        }
        List<BitmapImage> images = new List<BitmapImage>();
        List<BitmapImage> backgrounds = new List<BitmapImage>();
        List<string> texts = new List<string>();
        int curr_image = 0, curr_back = 0, curr_text = 0, talk = 0;

        private void Image_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
        }
        private void GetImages(string path, List<BitmapImage> list)
        {
            DirectoryInfo folder = new DirectoryInfo(path);
            if (folder.Exists)
            {
                foreach(FileInfo fileInfo in folder.GetFiles())
                {
                    if (".jpg|.jpeg|.png".Contains(fileInfo.Extension.ToLower()))
                    {
                        BitmapImage src = new BitmapImage();
                        src.BeginInit();
                        src.UriSource = new Uri(fileInfo.FullName, UriKind.Absolute);
                        src.EndInit();
                        list.Add(src);
                    }
                }
            }
        }
        private void GetTexts(string path, List<string> list)
        {
            string line;
            using(StreamReader stream = new StreamReader(path))
            {
                while ((line = stream.ReadLine()) != null)
                {
                    texts.Add(line);
                }
            }
        }

        private void Text_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Text.Content = texts[++curr_text % (texts.Count)];
        }

        private void Character_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Character.Source = images[++curr_image%(images.Count)];
        }

        private void Background_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Background.Source = backgrounds[++curr_back % (backgrounds.Count)];
        }

    }
}
