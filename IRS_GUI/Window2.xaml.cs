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
using System.Windows.Shapes;

namespace IRS_GUI
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public System.Data.DataTable DataTable { get; set; }

        public Window2()
        {
            InitializeComponent();
        }

        public Window2(System.Data.DataTable dataTable): this()
        {
            DataTable = dataTable;

            for (int i = 0; i < DataTable.Rows.Count; i++)
            {
                if ( Convert.ToBoolean(DataTable.Rows[i]["SELECT"]) == true)
                {
                    block1.Text += "\n" + DataTable.Rows[i]["NAME"].ToString();
                }

            }

           
            
        }
    }
}

