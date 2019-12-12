using System;
using System.Collections.Generic;
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
using System.Data.OleDb;
using System.Data;

namespace ExcelAccess
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

        public static DataTable ReadExcelToTable()
        {
            try
            {
                string connstring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=d:\test.xlsx;Extended Properties='Excel 8.0;HDR=YES;'";
                using (OleDbConnection conn = new OleDbConnection(connstring))
                {
                    conn.Open();
                    DataTable sheetsName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" }); //得到所有sheet的名字
                    string firstSheetName = sheetsName.Rows[0][2].ToString(); //得到第一个sheet的名字
                    string sql = string.Format("SELECT * FROM [{0}]", firstSheetName); //查询字符串
                    OleDbDataAdapter ada = new OleDbDataAdapter(sql, connstring);
                    DataSet set = new DataSet();
                    ada.Fill(set);
                    conn.Close();
                    return set.Tables[0];
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = ReadExcelToTable();
            dataGrid1.ItemsSource = dt.DefaultView;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //向工作表中插入数据
            string connstring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=d:\test.xlsx;Extended Properties='Excel 8.0;HDR=YES;'";
            string colList = null;
            string valueList=null;
            Insert(ref colList, ref valueList);
            using (OleDbConnection ole_conn = new OleDbConnection(connstring))
            {
                ole_conn.Open();
                using (OleDbCommand ole_cmd = ole_conn.CreateCommand())
                {
                    ole_cmd.CommandText = "insert into [Sheet1$]"+colList+"values"+valueList;
                    ole_cmd.ExecuteNonQuery();
                    MessageBox.Show("数据插入成功......");
                }
            }
        }
        //收集insert的一行数据
        private void Insert(ref string colList, ref string valueList)
        {
            string columns = "(";
            string values = "(";
            int total = dataGrid1.Columns.Count();
            int currRow = dataGrid1.Items.IndexOf(dataGrid1.SelectedCells.First().Item);   // 单元格当前所在行
            int currCol = dataGrid1.SelectedCells.First().Column.DisplayIndex;   // 单元格当前所在列

            for (int i = 0; i < total; ++i)
            {


                columns += "["+dataGrid1.Columns[i].Header.ToString()+"]";
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
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //更新
            //向工作表中插入数据
            int currRow = dataGrid1.Items.IndexOf(dataGrid1.SelectedCells.First().Item);   // 单元格当前所在行
            int currCol = dataGrid1.SelectedCells.First().Column.DisplayIndex;   // 单元格当前所在列
            string colName = dataGrid1.SelectedCells.First().Column.Header.ToString();
            string value = (dataGrid1.Columns[currCol].GetCellContent(dataGrid1.Items[currRow]) as TextBlock).Text;
            //定位
            string keyValue = (dataGrid1.Columns[0].GetCellContent(dataGrid1.Items[currRow]) as TextBlock).Text;
            string keyName = dataGrid1.Columns[0].Header.ToString();

            string connstring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=d:\test.xlsx;Extended Properties='Excel 8.0;HDR=YES;'";

            using (OleDbConnection ole_conn = new OleDbConnection(connstring))
            {
                ole_conn.Open();
                using (OleDbCommand ole_cmd = ole_conn.CreateCommand())
                {
                    ole_cmd.CommandText = string.Format("update [Sheet1$] set {0} = '{1}' where {2} = '{3}';", colName, value, keyName, keyValue);
                    ole_cmd.ExecuteNonQuery();
                    MessageBox.Show("数据更新成功......");
                }
            }
        }
    }
}
