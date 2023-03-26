using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace viewer3d.Figures
{
    /* Пирамида */
    /* Расчет mesh аналогичен фигуре конус, т.к. пирамида является частным случаем конуса */
    internal class Pyramid: Cone
    {
        public Pyramid(Point3D centerPoint, Color color, double height, double bottomDiameter, int garani) : 
            base(centerPoint, color,  height,  bottomDiameter)
        {
            NDetails = garani;
        }
        public override int getTypeID() => (int)ftype.pyramid;
    }
}
