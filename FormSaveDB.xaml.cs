using System.Windows;

namespace viewer3d
{
	/* Получение параметров для сохранение в БД */
	public partial class FormSaveDB : Window
	{
		public FormSaveDB()
		{
			InitializeComponent();
			DataContext = this;
		}

		public string NameSceneDB { get; set; } = "";

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
		}
	}
}
