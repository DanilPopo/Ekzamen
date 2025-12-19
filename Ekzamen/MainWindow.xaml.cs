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
using System.Globalization;
using System.IO;

namespace Ekzamen
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Цена за квадратный метр выбранного материала
        double price = 0;
        // Итоговая стоимость покупки
        double total = 0;

        public MainWindow()
        {
            InitializeComponent();
        }
        //Обработчик нажатия кнопки "Рассчитать", который считает
        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            // Проверка корректности введённых данных
            if (!double.TryParse(WidthBox.Text, out double width) ||
                !double.TryParse(HeightBox.Text, out double height) ||
                MaterialBox.SelectedItem == null)
            {
                MessageBox.Show("Введите корректные данные");
                return;
            }

            if (width <= 0 || height <= 0)
            {
                MessageBox.Show("Размеры должны быть положительными");
                return;
            }
            // Получение выбранного материала из ComboBox
            string material = ((ComboBoxItem)MaterialBox.SelectedItem).Content.ToString();

            price = material == "Алюминий" ? 15.50 : 9.90;

            double area = width * height;
            total = area * price;
            // Вывод результата на форму
            ResultText.Text =
                $"Размер: {width} x {height}\n" +
                $"Материал: {material}\n" +
                $"Стоимость: {total:F2} руб.";
        }
        //Обработчик нажатия кнопки "Оформить квитанцию", который формирует квитанцию в txt файле
        private void Receipt_Click(object sender, RoutedEventArgs e)
        {
            if (total == 0)
            {
                MessageBox.Show("Сначала выполните расчёт");
                return;
            }

            string material = ((ComboBoxItem)MaterialBox.SelectedItem).Content.ToString();
            string size = $"{WidthBox.Text}x{HeightBox.Text}";
            string date = DateTime.Now.ToString("dd.MM.yy HH:mm");
            string checkNumber = new Random().Next(100000, 999999).ToString();

            string receipt =
                $@"ООО ""Уютный Дом""
                Добро пожаловать
                ККМ 00075411     #3969
                ИНН 1087746942040
                ЭКЛЗ 3851495566
                Чек № {checkNumber}
                {date} СИС.

                наименование товара
                жалюзи    {size}
                материал  {material}
                Итог      ={total:F2}
                Сдача     =0
                Сумма итого: ={total:F2}

                ************************
                00003751# 059705";

            string fileName = $"{checkNumber}_{DateTime.Now:yyyyMMdd}_{total:F2}.txt";
            File.WriteAllText(fileName, receipt);

            MessageBox.Show($"Квитанция сохранена:\n{fileName}");
        }
    }
}
