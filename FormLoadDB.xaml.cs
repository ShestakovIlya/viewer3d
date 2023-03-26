using System;
using System.Collections;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace viewer3d
{
    /* Форма для загрузки из БД */
    public partial class FormLoadDB : Window
    {
        string sqlFill = "SELECT TOP(20) sc.id as 'id', sc.name as 'Сцена',FORMAT(date_create, 'dd.MM.yyyy HH:mm') as 'Дата сохранения', count(fg.id) as 'Количество приметивов' "
                            + "FROM dbo.scenes sc LEFT JOIN dbo.figures  fg ON fg.scene_id=sc.id "
                            + "group by sc.id,sc.name,sc.date_create order by date_create desc ";

        public FormLoadDB()
        {
            InitializeComponent();
            DataContext = this;
        }

        private int _selectID = 0;
        public int SelectID { get { return _selectID; } }

        public IEnumerable dbSource /* привязан к таблице с данными */
        {
            get
            {
                try
                {
                    return new WorkDB().readData(sqlFill)?.Tables[0]?.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Внимание");
                    this.DialogResult = false;
                }
                return null;
            }
        }

        private void SceneGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (SceneGrid.SelectedItem == null)
                    return;
                DataRowView selectedItem = SceneGrid.SelectedItem as DataRowView;
                DataRow dr1 =  selectedItem.Row;
                int.TryParse(dr1.ItemArray[0].ToString(), out _selectID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            this.DialogResult = true;
        }

    }
}
