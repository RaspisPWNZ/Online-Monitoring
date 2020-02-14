using System;
using System.Collections.Generic;
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

namespace Online_monitoring
{
    /// <summary>
    /// Логика взаимодействия для Item.xaml
    /// </summary>
    public partial class Item : UserControl
    {
        double temp;
        DateTime date;
        string fio;
        BitmapImage image;
        public double TemperatureLimit { get; set; }
        public double Temperature
        {
            get
            {
                return temp;
            }
            set
            {
                temp = value;
                lblTemp.Content = String.Format("{0:#.#} °С", temp);
                if (temp < TemperatureLimit)
                {
                    b1.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 170, 0));
                    b2.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 170, 0));
                    b3.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 170, 0));
                }
                else
                {

                    b1.BorderBrush = Brushes.Red;
                    b2.BorderBrush = Brushes.Red;
                    b3.BorderBrush = Brushes.Red;
                }
            }
        }
        public DateTime DateOfCreate
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                lblDateOfCreate.Content = date.ToString();
            }
        }
        public string FIO
        {
            get
            {
                return fio;
            }
            set
            {
                fio = value;
                lblFIO.Content = fio;
            }
        }
        public BitmapImage Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                img.Source = image;
            }
        }
        public Item()
        {
            InitializeComponent();
        }
    }
}
