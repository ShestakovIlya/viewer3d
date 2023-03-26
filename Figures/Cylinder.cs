using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace viewer3d.Figures
{
    /* Цилиндр */
    public class Cylinder : Figure
    {
        double Rin { get; set; } = 0;
        double Rout { get; set; } = 0.4;
        double Height { get; set; } = 0.7;
        public override double getWDValue() => Rout * 2;
        public override double getHeight() => Height;
        public Cylinder(Point3D center, Color color, double height, double diameter) : base(center, color)
        {
            Height = height;
            Rout = diameter / 2;
            NDetails = 100;
        }

        /* Расчет mesh для построения фигуры */
        public override IEnumerable<MeshGeometry3D> meshes
        {
            get
            {
                if (NDetails < 2 || Rin == Rout)
                    yield break;

                double radius = Rin;
                if (Rin > Rout)
                {
                    Rin = Rout;
                    Rout = radius;
                }

                double h = Height / 2;
                Point3D[,] pts = new Point3D[NDetails, 4];

                for (int i = 0; i < NDetails; i++)
                {
                    pts[i, 0] = GetPosition(Rout, i * 360 / (NDetails - 1), h);
                    pts[i, 1] = GetPosition(Rout, i * 360 / (NDetails - 1), -h);
                    pts[i, 2] = GetPosition(Rin, i * 360 / (NDetails - 1), -h);
                    pts[i, 3] = GetPosition(Rin, i * 360 / (NDetails - 1), h);
                }
                for (int i = 0; i < NDetails; i++)
                {
                    for (int j = 0; j < 4; j++)
                        pts[i, j] += (Vector3D)getCenterPoint();
                }

                Point3D[] p = new Point3D[8];
                for (int i = 0; i < NDetails - 1; i++)
                {
                    p[0] = pts[i, 0];
                    p[1] = pts[i, 1];
                    p[2] = pts[i, 2];
                    p[3] = pts[i, 3];
                    p[4] = pts[i + 1, 0];
                    p[5] = pts[i + 1, 1];
                    p[6] = pts[i + 1, 2];
                    p[7] = pts[i + 1, 3];

                    yield return getMesh(p[0], p[4], p[3]);
                    yield return getMesh(p[4], p[7], p[3]);

                    yield return getMesh(p[5], p[1], p[2]);
                    yield return getMesh(p[6], p[5], p[2]);

                    yield return getMesh(p[0], p[1], p[4]);
                    yield return getMesh(p[1], p[5], p[4]);

                    yield return getMesh(p[2], p[7], p[6]);
                    yield return getMesh(p[2], p[3], p[7]);
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
        public override int getTypeID() => (int)ftype.cylinder;
    }
}
