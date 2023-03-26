using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace viewer3d
{
    /* Класс формы для определения параметров фигур  */
    public partial class SettingPrimitive : Window
    {
        public SettingPrimitive()
        {
            InitializeComponent();
            this.DataContext = this;
        }
              
        /* Параметры для создания фигур*/
        public int SelectType { get; set; } = 0;
        public Color getColor() {
            try
            {
                return EnumColors.First(x => x.Key == ((KeyValuePair<String, Color>)ColorsBox.SelectedValue).Key).Value;
            }
			catch (Exception ex)
			{
                MessageBox.Show( $"Ошибка выбора цвета:{ex.Message}", "Внимание");
			}
            return Colors.Gray;
        }
        public Point3D GetCenterPoint => new Point3D() { X = PStartX, Y = PStartY, Z = PStartZ };
        public double PStartX { get; set; } = 0;
        public double PStartY { get; set; } = 0;
        public double PStartZ { get; set; } = 0;
        public double FHeight { get; set; } = 0.5;
        public double FWDValue { get; set; } = 0.6;
        public int FDetails { get; set; } = 4;

        /* Коллекция цветов */
        public static IEnumerable<KeyValuePair<String, Color>> EnumColors
        {
            get
            {
                return typeof(Colors)
                    .GetProperties()
                    .Select(x => new KeyValuePair<String, Color>(x.Name, (Color)x.GetValue(null)))
                    .Where(x => !x.Equals("Transparent"));
            }
        }

        /* Проверка на валидность полей TextBox */
        private void ValidTextBox(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, @"^([0-9])|(\.)|(\-)"))
            {
                e.Handled = true;
                return;
            }
            string str_out = (e.Source as TextBox).Text + e.Text;
            if (str_out.Count() == 1) return;
            if (e.Text == "." && str_out.IndexOf(".") == str_out.Length - 1) return;
            e.Handled = double.TryParse(str_out, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out double value) ? false : true;
        }       

        /* Закрытие формы с пометкой на создание фигуры */
		private void Build_Click(object sender, RoutedEventArgs e)
		{
            SelectType = TypesFigure.SelectedIndex;
            this.DialogResult = true;
        }

        /* Закрытие формы с пометкой отмены */
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
