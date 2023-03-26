using System.Collections.Generic;
using System.Data;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using viewer3d.Figures;

namespace viewer3d
{
    /* Класс для обработки операций с БД */
    public class WorkDB 
	{
        private IDataBase IDB { get; set; }
		public WorkDB()
		{
			IDB = new DBMSServer();           
        }

        /* Чтение данных из БД */
        public DataSet readData(string sql) => IDB.RequestToData(sql);

        /* Сохранение сцены в БД */
        public void SaveScene(DataScene ds)
		{
			int id_ = WriteScene(ds.name_scene);
			if (id_ > 0) WriteFigures(id_, ds.figures);
		}

        /* Запись сцены в БД */
        private int WriteScene(string name_scene) => IDB.Insert($"INSERT INTO dbo.scenes (name) VALUES ('{name_scene}');select SCOPE_IDENTITY(); ");

        /* Запись всех фигур в БД */
		private void WriteFigures(int scene_id, List<Figure> figures)
		{
            figures.ForEach(f =>
            {                		
                IDB.Insert($"INSERT INTO dbo.figures (scene_id, typefigure_id, cX, cY, cZ, color, wdvalue, height, details) " +
               $@"VALUES ({scene_id},{f.getTypeID()},{f.getCenterPoint().X},{f.getCenterPoint().Y},{f.getCenterPoint().Z},'{f.getColor()}',{f.getWDValue()},{f.getHeight()},{f.getDetails()});");
            });           
		}        

        /* Загрузка сцены из БД */
        public void LoadScene(int scene_id, DataScene ds)
        {            
            ds.name_scene = ReadScene(scene_id).Tables[0]?.Rows[0]?.ItemArray[0]?.ToString();
            ReadFigures(scene_id, ds.figures);
        }

        /* Чтение сцены из БД */
        private DataSet ReadScene(int scene_id) => readData($"SELECT name FROM dbo.scenes WHERE id = {scene_id}");

        /* Чтение всех фигур из БД */
        private void ReadFigures(int scene_id, List<Figure> figures)
        {
            foreach (DataTable table in readData($"SELECT * FROM dbo.figures WHERE scene_id = {scene_id}").Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    Point3D fPoint = new Point3D(row.Field<double>("cX"), row.Field<double>("cY"), row.Field<double>("cZ"));
                    Color fColor = (Color)ColorConverter.ConvertFromString(row.Field<string>("color"));
                    Figure.ftype ftyped = (Figure.ftype) row["typefigure_id"];
                    switch (ftyped)
                    {   
                        case Figure.ftype.cube:
                            figures.Add( new Cube(fPoint, fColor, row.Field<double>("wdvalue")));
                            break;
                        case Figure.ftype.cylinder:
                            figures.Add(new Cylinder(fPoint, fColor, row.Field<double>("height"), row.Field<double>("wdvalue")));
                            break;
                        case Figure.ftype.cone:
                            figures.Add(new Cone(fPoint, fColor, row.Field<double>("height"), row.Field<double>("wdvalue")));
                            break;
                        case Figure.ftype.pyramid:
                            figures.Add(new Pyramid(fPoint, fColor, row.Field<double>("height"), row.Field<double>("wdvalue"), row.Field<int>("details")));
                            break;
                        case Figure.ftype.sphere:
                            figures.Add(new Sphere(fPoint, fColor, row.Field<double>("wdvalue")));
                            break;
                        default: break;
                    }                  
                }
            }
        }
    }
}