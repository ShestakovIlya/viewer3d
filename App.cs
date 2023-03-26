using System;
using System.Globalization;
using System.Windows;

namespace viewer3d
{
    public class App : Application
    {
        private MainWindow _mainWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                if (args.ExceptionObject is Exception ex)
                {
                    MessageBox.Show(ex.Message, "ВНИМАНИЕ");
                }
                Environment.Exit(1);
            };

            MainViewModel mvm = new MainViewModel();
            _mainWindow = new MainWindow()
            {
                DataContext = mvm
            };
            _mainWindow.Title = "Создание 3D примитивов";
            _mainWindow.Show();  

        }
    }
}
