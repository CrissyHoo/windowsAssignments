using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace DBTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();

        }
        private string tableName = "weather";
        
        string connString = "Host=localhost;Port=5432;Username=postgres;Password=hsc1209;Database=whu";

        public void getPK(ref string pkName,ref string pkValue)
        {
            DataSet ds = new DataSet();
            using (var conn = new NpgsqlConnection(connString))
            {
                int currRow = dataGrid1.Items.IndexOf(dataGrid1.SelectedCells.First().Item);   // 单元格当前所在行
                int currCol = dataGrid1.SelectedCells.First().Column.DisplayIndex;   // 单元格当前所在列
                using (NpgsqlDataAdapter da = new NpgsqlDataAdapter("select * from weather;", conn))
                {
                    conn.Open();
                    da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    da.Fill(ds);
                    pkName = ds.Tables[0].PrimaryKey[0].ColumnName;

                    // 获取主键所在列
                    int keyCol = 0;
                    for (int i = 0; i < ds.Tables[0].Columns.Count; ++i)
                    {
                        if (ds.Tables[0].Columns[i].ColumnName == pkName)
                        {
                            keyCol = i;
                            break;
                        }
                    }

                    // 获取keyValue
                    pkValue = ds.Tables[0].Rows[currRow][keyCol].ToString();

                }

            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {//查
            
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                DataSet ds = new DataSet();
                // Retrieve all rows
                try
                {
                    using (NpgsqlDataAdapter da = new NpgsqlDataAdapter("SELECT * FROM weather", conn))
                    {
                        
                        //显示在datagrid
                        da.Fill(ds);
                        dataGrid1.ItemsSource = null;
                        dataGrid1.ItemsSource = ds.Tables[0].DefaultView;
                       

                    }
                }
                catch (Exception ex)
                {
                    conn.Close();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //删除
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string keyName = null;
            string keyValue = null;
            getPK(ref keyName, ref keyValue);
            string deleteStr = string.Format("delete from {0}  where {1} = '{2}' ;",
                           tableName, keyName, keyValue);
           
                using (var cmd = new NpgsqlCommand(deleteStr, conn))
                {

                    cmd.ExecuteNonQuery();

                }


            }

        }

        
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //改
            int currRow = dataGrid1.Items.IndexOf(dataGrid1.SelectedCells.First().Item);   // 单元格当前所在行
            int currCol = dataGrid1.SelectedCells.First().Column.DisplayIndex;   // 单元格当前所在列
            string colName = dataGrid1.SelectedCells.First().Column.Header.ToString();
            string value = (dataGrid1.Columns[currCol].GetCellContent(dataGrid1.Items[currRow]) as TextBlock).Text;
            string keyName = null;
            string keyValue = null;
            getPK(ref keyName, ref keyValue);

            string updateStr = string.Format("update {0} set {1} = '{2}' where {3} = '{4}' ;",
                        tableName, colName, value, keyName, keyValue);

            using (var conn = new NpgsqlConnection(connString))
            {

                conn.Open();
                using (var cmd = new NpgsqlCommand(updateStr, conn))
                {

                    cmd.ExecuteNonQuery();

                }


            }
        }

        
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //增  insert
            string colList = null;
            string valueList = null;
            Insert(ref colList, ref valueList);//从表中获取修改了的值
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string insertStr = string.Format("insert into {0} {1} values {2};",
                        tableName, colList, valueList);
                using (var cmd = new NpgsqlCommand(insertStr, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }




        }
        private void Insert(ref string colList, ref string valueList)
        {
            string columns = "(";
            string values = "(";
            int total = dataGrid1.Columns.Count();
            int currRow = dataGrid1.Items.IndexOf(dataGrid1.SelectedCells.First().Item);   // 单元格当前所在行
            int currCol = dataGrid1.SelectedCells.First().Column.DisplayIndex;   // 单元格当前所在列

            for (int i = 0; i < total ; ++i)
            {
                

                    columns += dataGrid1.Columns[i].Header.ToString();
                    columns += ",";

                    values += "'";
                    values += (dataGrid1.Columns[i].GetCellContent(dataGrid1.Items[currRow]) as TextBlock).Text;
                values += "',";
                
            }
            string subCols = columns.Substring(0, columns.Length - 1);
            string subVals = values.Substring(0, values.Length - 1);

            columns = subCols + ")";

            values = subVals + ")";

            colList = columns;
            valueList = values;
        }
        

    }
}