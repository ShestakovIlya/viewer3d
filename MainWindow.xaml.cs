using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace viewer3d
{
    public partial class MainWindow : Window
	{
        public MainWindow()
		{
			InitializeComponent();
        }
        
        /* Вычисляем угол поворота  при движении мыши */
        private Point oldPosition;
        Quaternion _rotation;
        Quaternion _rotationDelta;
        private void myViewport_MouseMove(object sender, MouseEventArgs e)
		{
            if (e.RightButton == MouseButtonState.Pressed)
            {
                Point vecLoc = e.GetPosition(myViewport);
                Vector vecDiff = oldPosition - vecLoc;
                Vector3D vecMouse = new Vector3D(vecDiff.X, -vecDiff.Y, 0);
                Vector3D axis = Vector3D.CrossProduct(vecMouse, new Vector3D(0, 0, 1));
                double len = axis.Length;

                _rotationDelta = len < 0.01 ? new Quaternion(new Vector3D(0, 0, 1), 0) : _rotationDelta = new Quaternion(axis, len);

                UpdateScene(_rotationDelta * _rotation);
            }
        }

        /* Фиксируем начальное положение при повороте */
		private void myViewport_MouseDown(object sender, MouseButtonEventArgs e)
		{
            oldPosition = e.MouseDevice.GetPosition(myViewport);
        }

        /* Корректируем угол поворота, чтоб после повторного поворота не было провалов */
        private void myViewport_MouseUp(object sender, MouseButtonEventArgs e)
		{
            _rotation = _rotationDelta * _rotation;
        }

        /* Сброс изменения вида камеры */
        private void resetScene_Click(object sender, RoutedEventArgs e)
		{
            _rotation = new Quaternion(0, 0, 0, 1);
            UpdateScene(_rotation);
        }

        /* Обновление сцены */
        private void UpdateScene(Quaternion quater)
		{
            myViewport.Camera.Transform = new RotateTransform3D(new AxisAngleRotation3D(quater.Axis, quater.Angle));
        }

        /* Масштабирование фигур в сцене, колесико + Control */
        private void myViewport_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                scale.ScaleX += e.Delta > 0 ? -0.03 : 0.03;
                scale.ScaleY += e.Delta > 0 ? -0.03 : 0.03;
                scale.ScaleZ += e.Delta > 0 ? -0.03 : 0.03;
            }
        }       
    }
}
