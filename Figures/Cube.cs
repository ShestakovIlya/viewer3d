using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace viewer3d.Figures
{
    /* Куб */
    public class Cube : Figure
    {
        double Side { get; set; }
        public override double getWDValue() => Side;

        public Cube(Point3D centerPoint, Color color, double side) : base(centerPoint, color)
        {
            Side = side;
            NDetails = 8;
        }

        /* Расчет mesh для построения фигуры */
        public override IEnumerable<MeshGeometry3D> meshes
        {
            get
            {
                double a = Side / 2.0;
                Point3D[] p = new Point3D[8];
                p[0] = new Point3D(-a, a, a);
                p[1] = new Point3D(a, a, a);
                p[2] = new Point3D(a, a, -a);
                p[3] = new Point3D(-a, a, -a);
                p[4] = new Point3D(-a, -a, a);
                p[5] = new Point3D(a, -a, a);
                p[6] = new Point3D(a, -a, -a);
                p[7] = new Point3D(-a, -a, -a);

                for (int i = 0; i < NDetails; i++)
                    p[i] += (Vector3D)getCenterPoint();

                yield return getMesh(p[0], p[1], p[2]);
                yield return getMesh(p[2], p[3], p[0]);

                yield return getMesh(p[4], p[7], p[6]);
                yield return getMesh(p[6], p[5], p[4]);

                yield return getMesh(p[0], p[4], p[5]);
                yield return getMesh(p[5], p[1], p[0]);

                yield return getMesh(p[1], p[5], p[6]);
                yield return getMesh(p[6], p[2], p[1]);

                yield return getMesh(p[2], p[6], p[7]);
                yield return getMesh(p[7], p[3], p[2]);

                yield return getMesh(p[0], p[3], p[7]);
                yield return getMesh(p[7], p[4], p[0]);
            }
        }

        /* Получение типа фигуры */
        public override int getTypeID() => (int)ftype.cube;
    }
}
