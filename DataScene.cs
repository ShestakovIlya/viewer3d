using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using viewer3d.Figures;

namespace viewer3d
{
    /* Класс для работы со сценой */
    public class DataScene
    {
        public Model3DGroup SceneModelGroup { get; } /* Объект модели */
        public List<Figure> figures { get; set; } /* List всех фгур */
        public string name_scene { get; set; } = ""; /* Название сцены */

        public DataScene()
        {
            SceneModelGroup = new Model3DGroup();
            figures = new List<Figure>();
            lightScene();
        }

        /* Источник света сцены*/
        private void lightScene()
        {
            DirectionalLight myDirectionalLight = new DirectionalLight();
            myDirectionalLight.Color = Colors.White;
            myDirectionalLight.Direction = new Vector3D(-0.61, -0.5, -0.61);

            SceneModelGroup.Children.Add(myDirectionalLight);
        }

        /* Очистить сцену */
        public void ClearScene()
        {
            SceneModelGroup.Children.Clear();
            figures.Clear();
            lightScene();
        }

        /* Отобразит фигуру из mesh */
        public void Show3DFigure(Figure figure)
        {
            SceneModelGroup.Dispatcher.Invoke(() =>
            {
                foreach (MeshGeometry3D mesh in figure.meshes)
                {
                    SolidColorBrush brush = new SolidColorBrush()
                    {
                        Color = figure.getColor()
                    };
                    Material material = new DiffuseMaterial(brush);
                    SceneModelGroup.Children.Add(new GeometryModel3D(mesh, material));
                }
            });
        }
    }
}
