using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Online_monitoring
{
    /// <summary>
    /// Логика взаимодействия для SettingsWin.xaml
    /// </summary>
    public partial class SettingsWin : Window
    {
        double t_lim;
        string dir;
        int max_count;
        public double TemperatureLimit
        {
            get
            {
                return t_lim;
            }
            set
            {
                t_lim = value;
                tbLimit.Text = t_lim.ToString();
            }
        }

        public string Directory
        {
            get
            {
                return dir;
            }
            set
            {
                dir = value;
                tbDir.Text = dir;
            }
        }

        public int MaxCount
        {
            get
            {
                return max_count;
            }
            set
            {
                max_count = value;
                tbMaxCount.Text = max_count.ToString();
            }
        }
        public SettingsWin()
        {
            InitializeComponent();
        }
        //Сохранение настроек (При вводе дробного числа через точку происходит замена точки на запятую)
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tbLimit.Text = tbLimit.Text.Replace('.', ',');

                if (!System.IO.Directory.Exists(tbDir.Text))
                {
                    System.Windows.MessageBox.Show("Введеной директории не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }



                else
                {
                    Directory = tbDir.Text;
                    TemperatureLimit = Convert.ToDouble(tbLimit.Text);
                    MaxCount = Convert.ToInt32(tbMaxCount.Text);
                    Close();
                }
                //System.IO.Directory.Exists(Directory)
            }
            catch (FormatException ex)
            {
                switch (ex.TargetSite.Name)
                {

                    case "ParseDouble":
                        System.Windows.MessageBox.Show("Пороговое значение температуры введено неправильно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    case "StringToNumber":
                        System.Windows.MessageBox.Show("Максимальное кол-во элементов введено неправильно", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;

                }
            }
        }
        
    }
}
