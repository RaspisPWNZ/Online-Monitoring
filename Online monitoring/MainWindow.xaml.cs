using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace Online_monitoring
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Item> item_list = new List<Item>();

        int max_count;
        double t_lim;
        string dir;
        public MainWindow()
        {
            InitializeComponent();


            //Загрузка настроек из файла, если файл с настройками имеется
            try
            {
                if (File.Exists("config.xml"))
                {
                    XmlDocument cfg = new XmlDocument();
                    cfg.Load("config.xml");
                    XmlElement root = cfg.DocumentElement;
                    foreach (XmlNode node in root)
                    {
                        switch (node.Name)
                        {
                            case "dir":
                                dir = node.Attributes.GetNamedItem("value").Value;
                                break;
                            case "t_lim":
                                t_lim = Convert.ToDouble(node.Attributes.GetNamedItem("value").Value);
                                break;
                            case "max_count":
                                max_count = Convert.ToInt32(node.Attributes.GetNamedItem("value").Value);
                                break;
                        }
                    }

                }
            }
            //При неправильной записи в xml файле 
            catch (Exception)
            {
                MessageBox.Show("Проверьте файл config.xml", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //Если файл не существует, вызывается окно с настройками 
            if (!File.Exists("config.xml"))
            {
                SettingsWin win = new SettingsWin();
                win.ShowDialog();
                max_count = win.MaxCount;
                t_lim = win.TemperatureLimit;
                this.dir = win.Directory;
                XmlSave(dir, t_lim, max_count);

            }

            Start();
        }
        //Вызов окна настроек, их применение и сохранение в xml
        private void Sett_Click(object sender, RoutedEventArgs e)
        {

            SettingsWin win = new SettingsWin() { Directory = dir, TemperatureLimit = t_lim, MaxCount = max_count };
            win.Owner = this;
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.ShowDialog();
            max_count = win.MaxCount;
            t_lim = win.TemperatureLimit;
            dir = win.Directory;
            XmlSave(dir, t_lim, max_count);
            Start();
        }
        //Подписка на эвенты для FileSystemWatcher и отображение записей
        private void Start()
        {
            if (Directory.Exists(dir))
            {
                FileSystemWatcher watcher = new FileSystemWatcher(dir);
                if (!watcher.EnableRaisingEvents)
                {
                    watcher.Changed += Watcher_Created;
                    watcher.Created += Watcher_Created;
                    watcher.EnableRaisingEvents = true;
                }
                Update();
            }
        }
        //Метод вызывающий обновление отображаемых записей
        private void Update()
        {
            int counter = 0;
            item_list.Clear();
            DirectoryInfo directory = new DirectoryInfo(dir);
            //Получает все файлы соответвующие заданному шаблону, сортируя их по убыванию даты создания
            FileInfo[] files = (directory.GetFiles("*_*_*_*.jpg", SearchOption.TopDirectoryOnly)).OrderByDescending(f => f.CreationTime).ToArray();

            foreach (FileInfo file in files)
            {
                try
                {
                    Item item = new Item();
                    item.TemperatureLimit = t_lim;

                    string[] fileinf = file.Name.Split('_');
                    item.FIO = String.Format("{0} {1} {2}", fileinf[0], fileinf[1], fileinf[2]);
                    var f = fileinf[3].Remove(fileinf[3].Length - 3);

                    item.Temperature = Convert.ToDouble(fileinf[3].Split('.')[0] + "," + fileinf[3].Split('.')[1]);
                    item.Image = new BitmapImage(new Uri(file.FullName));
                    item.DateOfCreate = file.CreationTime;

                    item_list.Add(item);
                    counter++;
                    if (counter == max_count)
                    {
                        break;
                    }
                }

                catch (Exception e)
                {

                }
            }
            lbFeed.ItemsSource = item_list;
            lbFeed.Items.Refresh();
        }
        //Делегат и вызываемый им метод для работы эвента
        delegate void WD();
        private void UpdateD()
        {
            if (!Dispatcher.CheckAccess())
            {
                WD d = UpdateD;
                Dispatcher.Invoke(d);
            }
            else
            {
                Update();
            }
        }
        //Эвент на добавление файла в каталог
        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            UpdateD();
        }
        //Сохранение настроек в файл *.xml
        private void XmlSave(string dir, double t_lim, int max_count)
        {
            XmlDocument cfg = new XmlDocument();
            cfg.AppendChild(cfg.CreateXmlDeclaration("1.0", "utf-8", ""));
            XmlElement data = cfg.CreateElement("data");
            cfg.AppendChild(data);


            XmlElement dr = cfg.CreateElement("dir");
            XmlAttribute value = cfg.CreateAttribute("value");
            value.Value = dir;
            dr.Attributes.Append(value);
            data.AppendChild(dr);

            XmlElement tlim = cfg.CreateElement("t_lim");
            value = cfg.CreateAttribute("value");
            value.Value = t_lim.ToString();
            tlim.Attributes.Append(value);
            data.AppendChild(tlim);

            XmlElement mc = cfg.CreateElement("max_count");
            value = cfg.CreateAttribute("value");
            value.Value = max_count.ToString();
            mc.Attributes.Append(value);
            data.AppendChild(mc);
            cfg.Save("config.xml");

        }
    }
}
