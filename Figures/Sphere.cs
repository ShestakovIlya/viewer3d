using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace viewer3d.Figures
{
    /* Сфера */
    internal class Sphere : Figure
    {
        double Radius { get; set; } = 0.5;
        public override double getWDValue() => Radius * 2;
        public Sphere(Point3D centerPoint, Color color, double diameter) : base(centerPoint, color)
        {
            Radius = diameter / 2;
            NDetails = 30;
        }

        /* Расчет mesh для построения фигуры */
        public override IEnumerable<MeshGeometry3D> meshes
        {
            get
            {
                int u = NDetails;
                int v = NDetails - 10;

                if (u < 2 || v < 2)
                    yield break;

                Point3D[,] pts = new Point3D[u, v];
                for (int i = 0; i < u; i++)
                {
                    for (int j = 0; j < v; j++)
                    {
                        pts[i, j] =
                            GetPosition(Radius, i * 180 / (u - 1), j * 360 / (v - 1));
                        pts[i, j] += (Vector3D)getCenterPoint();
                    }
                }

                Point3D[] p = new Point3D[4];
                for (int i = 0; i < u - 1; i++)
                {
                    for (int j = 0; j < v - 1; j++)
                    {
                        p[0] = pts[i, j];
                        p[1] = pts[i + 1, j];
                        p[2] = pts[i + 1, j + 1];
                        p[3] = pts[i, j + 1];

                        yield return getMesh(p[0], p[1], p[2]);
                        yield return getMesh(p[2], p[3], p[0]);
                    }
                }
            }
        }

        /* Нахождение точки */
        private Point3D GetPosition(double radius, double theta, double phi)
        {
            Point3D pt = new Point3D();
            double snt = Math.Sin(theta * Math.PI / 180);
            double cnt = Math.Cos(theta * Math.PI / 180);
            double snp = Math.Sin(phi * Math.PI / 180);
            double cnp = Math.Cos(phi * Math.PI / 180);
            pt.X = radius * snt * cnp;
            pt.Y = radius * cnt;
            pt.Z = -radius * snt * snp;
            return pt;
        }

        /* Получение типа фигуры */
        public override int getTypeID() => (int)ftype.sphere;

    }
}
