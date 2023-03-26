using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace viewer3d.Figures
{
    /* Конус */
    internal class Cone : Figure
    {
        private double Rbottom { get; set; } = 0.5;
        private double Height { get; set; } = 1;
        private double Rtop { get; set; } = 0;

        public override double getWDValue() => Rbottom * 2;
        public override double getHeight() => Height;

        public Cone(Point3D centerPoint, Color color, double height, double bottomDiameter) : base(centerPoint, color)
        {
            Height = height;
            Rbottom = bottomDiameter / 2;
            NDetails = 150;
        }

        /* Расчет mesh для построения фигуры */
        public override IEnumerable<MeshGeometry3D> meshes
        {
            get
            {
                if (NDetails < 2)
                    yield break;

                double h = Height / 2;
                Point3D[,] pts = new Point3D[NDetails + 1, 4];

                for (int i = 0; i < NDetails + 1; i++)
                {
                    pts[i, 0] = GetPosition(Rtop, i * 360 / (NDetails - 1), h);
                    pts[i, 1] = GetPosition(Rbottom, i * 360 / (NDetails - 1), -h);
                    pts[i, 2] = GetPosition(0, i * 360 / (NDetails - 1), -h);
                    pts[i, 3] = GetPosition(0, i * 360 / (NDetails - 1), h);
                }
                for (int i = 0; i < NDetails + 1; i++)
                {
                    for (int j = 0; j < 4; j++)
                        pts[i, j] += (Vector3D)getCenterPoint();
                }

                Point3D[] p = new Point3D[6];
                for (int i = 0; i < NDetails; i++)
                {
                    p[0] = pts[i, 0];
                    p[1] = pts[i, 1];
                    p[2] = pts[i, 2];
                    p[3] = pts[i, 3];
                    p[4] = pts[i + 1, 0];
                    p[5] = pts[i + 1, 1];

                    yield return getMesh(p[0], p[4], p[3]);

                    yield return getMesh(p[1], p[2], p[5]);

                    yield return getMesh(p[0], p[1], p[5]);
                    yield return getMesh(p[0], p[5], p[4]);
                }
            }
        }

        /* Нахождение точки */
        private Point3D GetPosition(double radius, double theta, double y)
        {
            Point3D pt = new Point3D();
            double sn = Math.Sin(theta * Math.PI / 180);
            double cn = Math.Cos(theta * Math.PI / 180);
            pt.X = radius * cn;
            pt.Y = y;
            pt.Z = -radius * sn;
            return pt;
        }

        /* Получение типа фигуры */
        public override int getTypeID() => (int)ftype.cone;
    }
}
