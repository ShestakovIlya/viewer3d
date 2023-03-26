using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using viewer3d.Figures;

namespace viewer3d
{
    /* Класс для обработки главного представления */
    class MainViewModel
    {
        /* Описание собственных команд */
        public ICommand CreateFigure => new RelayCommand(cf => create_figure());
        public ICommand ClearScene => new RelayCommand(cf => Scene.ClearScene());
        public ICommand SaveDataBase => new RelayCommand(cf => saveDB());
        public ICommand LoadDataBase => new RelayCommand(cf => loadDB());
        public ICommand LoadTestItems => new RelayCommand(cf => loadTItems());

        public MainViewModel()
        {
            Scene = new DataScene();
        }

        public DataScene Scene { get; set; }
        
        /* Сохранение данных в БД */
        public void saveDB()
        {
            FormSaveDB fromSDB = new FormSaveDB();
            if (fromSDB.ShowDialog() != true)
                return;

            try
            {
                Scene.name_scene = fromSDB.NameSceneDB;
                WorkDB work1 = new WorkDB();
                work1.SaveScene(Scene);
                MessageBox.Show("Сохранено", "Внимание");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание");
            }
        }

        /* Загрузка данных из БД */
        public void loadDB()
        {
            FormLoadDB fromLDB = new FormLoadDB();
            if (fromLDB.ShowDialog() != true)
                return;

            if (MessageBox.Show("Загрузить указанную сцену?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            try
            {
                Scene.ClearScene();
                WorkDB wDB = new WorkDB();
                wDB.LoadScene(fromLDB.SelectID, Scene);
                Scene.figures.ForEach(async f =>
                {
                    await Task.Run(() => Scene.Show3DFigure(f));
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание");
            }
        }

        /* Создание новой фигуры */
        public async void create_figure()
        {
            SettingPrimitive settingFigure = new SettingPrimitive();
            if (settingFigure.ShowDialog() != true)
                return;
            
            try
            {
                Figure figure = null;
                switch (settingFigure.SelectType)
                {
                    case 0:
                        figure = new Cube(settingFigure.GetCenterPoint, settingFigure.getColor(), settingFigure.FWDValue);
                        break;
                    case 1:
                        figure = new Cylinder(settingFigure.GetCenterPoint, settingFigure.getColor(), settingFigure.FHeight, settingFigure.FWDValue);
                        break;
                    case 2:
                        figure = new Cone(settingFigure.GetCenterPoint, settingFigure.getColor(), settingFigure.FHeight, settingFigure.FWDValue);
                        break;
                    case 3:
                        figure = new Pyramid(settingFigure.GetCenterPoint, settingFigure.getColor(), settingFigure.FHeight, settingFigure.FWDValue, settingFigure.FDetails);
                        break;
                    case 4:
                        figure = new Sphere(settingFigure.GetCenterPoint, settingFigure.getColor(), settingFigure.FWDValue);
                        break;
                    default:
                        break;
                }
                Scene.figures.Add(figure);
                await Task.Run(() => Scene.Show3DFigure(figure));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Внимание");
            }            
        }

        /* Демонстрационный набор примитивов */
        public async void loadTItems()
        {
            Scene.ClearScene();
            Scene.figures.Add(new Cube(new Point3D(-1, 0, 0.4), Colors.Blue, 0.4));
            Scene.figures.Add(new Cylinder(new Point3D(-0.5, 0, -0.6), Colors.Red, 0.6, 0.6));
            Scene.figures.Add(new Cone(new Point3D(0, 0, -1), Colors.Yellow, 0.7, 0.6));
            Scene.figures.Add(new Pyramid(new Point3D(0.5, 0, 0), Colors.Violet, 0.7, 0.6, 6));
            Scene.figures.Add(new Sphere(new Point3D(0.3, 0, 1.1), Colors.Olive, 0.7));
            foreach (Figure f in Scene.figures) await Task.Run(() => Scene.Show3DFigure(f));
        }
    }
}
