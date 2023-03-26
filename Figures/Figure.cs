using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace viewer3d.Figures
{
    /*Общий астрактный класс для всех фигур */
    public abstract class Figure 
    {
        public enum ftype {cube = 100, cone, cylinder, pyramid, sphere} /* Типы фигур */
        protected int NDetails { get; set; } = 100;
        private Point3D CenterPoint { get; set; }
        private Color ColorFigure { get; set; }
        public Figure(Point3D centerPoint, Color color)
		{
            CenterPoint = centerPoint;
            ColorFigure = color;
        }

        public Point3D getCenterPoint() => CenterPoint;
        public Color getColor() => ColorFigure;
        public virtual double getWDValue() => 0;
        public virtual double getHeight() => 0;
        public abstract int getTypeID();
        public int getDetails() => NDetails;

        /* Создание Mesh объекта */
        protected MeshGeometry3D getMesh(Point3D p0, Point3D p1, Point3D p2)
        {
            return new MeshGeometry3D()
            {
                Positions= new Point3DCollection() { p0, p1, p2 },
                TriangleIndices = new Int32Collection() { 0, 1, 2}
            };
        }
        /* Расчет mesh для построения фигуры */
        public abstract IEnumerable<MeshGeometry3D> meshes { get; }
    }
}
