using System;
using System.IO;
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
using System.Data;
using Microsoft.Office.Interop.Excel;
using System.Runtime.Remoting.Contexts;
using System.Reflection.Emit;
using System.ComponentModel;

namespace IRS_GUI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : System.Windows.Window
    {



        public string filePath { get; set; }
        public int upDownControlValue { get; set; }
        public string comboBoxSelection { get; set; }


        

       

        private System.Data.DataTable dataTable = new System.Data.DataTable();
        bool helperSelected = false, helperSelected2 = false, helperSelected3 = false, helperSelected4 = false,
             helperSelected5 = false;
        private System.Data.DataTable originalDataTable;


        private Selected getData() {

            return new Selected() {};
        
        }




        private void Reset() {

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                dataTable.Rows[i]["POSITION"] = originalDataTable.Rows[i]["POSITION"];
                dataTable.Rows[i]["SELECT"] = originalDataTable.Rows[i]["SELECT"];
                dataTable.Rows[i]["NAME"] = originalDataTable.Rows[i]["NAME"];
                dataTable.Rows[i]["TYPE"] = originalDataTable.Rows[i]["TYPE"];
                dataTable.Rows[i]["LENGTH"] = originalDataTable.Rows[i]["LENGTH"];
            }

        }

        private void MyButton1(object sender, RoutedEventArgs e) {
            Reset();
        }

        private void MyButton2(object sender, RoutedEventArgs e) {
            for (int i = 0; i < myDataGrid.Columns.Count; i++) {

                myDataGrid.Columns[i].IsReadOnly = false;
            
            }
        }

        private void MyButton3(object sender, RoutedEventArgs e)
        {
            mainWindowCopy.dataTable = dataTable;
            this.Close();
        }


        public MainWindow mainWindowCopy { get; set; }
        public Window1(string filePath, int upDownControlValue, string comboBoxSelection, MainWindow mainWindowCopy)
        {
            InitializeComponent();
            this.filePath = filePath;
            this.upDownControlValue = upDownControlValue;
            this.comboBoxSelection = comboBoxSelection;
            this.mainWindowCopy = mainWindowCopy;
            string fileNameExtracted = System.IO.Path.GetFileNameWithoutExtension(filePath);
            string fileExtension = System.IO.Path.GetExtension(filePath);
    
            recordName.Text += (" " + fileNameExtracted);


            dataTable.Columns.Add("POSITION", typeof(int));
            dataTable.Columns.Add("SELECT", typeof(bool));
            dataTable.Columns.Add("NAME", typeof(string));
            dataTable.Columns.Add("TYPE", typeof(string));
            dataTable.Columns.Add("LENGTH", typeof(int));
    


            if (fileExtension == ".txt"){
                Console.WriteLine(upDownControlValue);
                ReadTXT();
            }
            else if (fileExtension == ".xlsx") {
                Console.WriteLine(comboBoxSelection);
                ReadEXCEL();
            }
        }

        private void ReadEXCEL() {

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Workbook wb;
            Worksheet ws;

            wb = excel.Workbooks.Open(filePath);
            ws = wb.Worksheets[upDownControlValue];

            int rowIndex = 1, numOfValidRows = 0;

            while (!string.IsNullOrEmpty(Convert.ToString(ws.Cells[rowIndex, 1].Value)))
            {
                rowIndex++;
                numOfValidRows++;
            }
          
            for(int i = 1; i <= numOfValidRows; i++)
            {
                dataTable.Rows.Add(i, false, Convert.ToString(ws.Cells[i, 1].Value), Convert.ToString(ws.Cells[i, 3].Value), Convert.ToString(ws.Cells[i, 2].Value));
            }

            myDataGrid.ItemsSource = dataTable.DefaultView;
            originalDataTable = dataTable.Copy();


        }



        private void ReadTXT()
        {
            string[] lines = File.ReadAllLines(filePath);
            string[,] values = new string[lines.Length, 4];


            for (int i = 0; i < lines.Length; i++)
            {
                string[] columns = lines[i].Split('\t'); // Assuming tab-separated data

                for (int j = 0; j < 4; j++)
                {
                    if (j < columns.Length && !string.IsNullOrWhiteSpace(columns[j])) // Check if column exists and is not empty
                    {
                        values[i, j] = columns[j].Trim();
                    }
                    else
                    {
                        values[i, j] = "empty"; // Replace missing values with "empty"
                    }
                }
            }



            for (int i = 0; i <  values.GetLength(0); i++)
            {
                try
                {
                    dataTable.Rows.Add(i, false, values[i, 0], values[i, 2], values[i, 1]);
                }
                catch (Exception ex)
                {
                    
                    MessageBox.Show("Data is in incorrect format" + "\n"+ "Error: "+ $"{ex.Message}");
                    this.Close();
                    break;

                }
            }

            myDataGrid.ItemsSource = dataTable.DefaultView;
            originalDataTable = dataTable.Copy();
        }

  
       private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
       {
            
            string selectedItem = ((ComboBoxItem)comboBoxSort.SelectedItem)?.Content?.ToString();
          
            if (selectedItem == "Position")
            {
                comboBoxSort.SelectedIndex = -1;

                if (helperSelected == false)
                {

                    helperSelected = true;
                    var sortedTable = dataTable
                          .AsEnumerable()
                          .OrderByDescending(row => row.Field<int>("POSITION"))
                          .CopyToDataTable();

                    for (int i = 0; i < sortedTable.Rows.Count; i++)
                    {
                        dataTable.Rows[i]["POSITION"] = sortedTable.Rows[i]["POSITION"];
                        dataTable.Rows[i]["SELECT"] = sortedTable.Rows[i]["SELECT"];
                        dataTable.Rows[i]["NAME"] = sortedTable.Rows[i]["NAME"];
                        dataTable.Rows[i]["TYPE"] = sortedTable.Rows[i]["TYPE"];
                        dataTable.Rows[i]["LENGTH"] = sortedTable.Rows[i]["LENGTH"];
                    }

                }
                else if (helperSelected == true) {

                    var sortedTable = dataTable
                    .AsEnumerable()
                    .OrderBy(row => row.Field<int>("POSITION"))
                    .CopyToDataTable();

                    for (int i = 0; i < sortedTable.Rows.Count; i++)
                    {
                        dataTable.Rows[i]["POSITION"] = sortedTable.Rows[i]["POSITION"];
                        dataTable.Rows[i]["SELECT"] = sortedTable.Rows[i]["SELECT"];
                        dataTable.Rows[i]["NAME"] = sortedTable.Rows[i]["NAME"];
                        dataTable.Rows[i]["TYPE"] = sortedTable.Rows[i]["TYPE"];
                        dataTable.Rows[i]["LENGTH"] = sortedTable.Rows[i]["LENGTH"];
                    }

                    helperSelected = false;
                    
                }

                
            }
            else if (selectedItem == "Length") {

                comboBoxSort.SelectedIndex = -1;

                if (helperSelected2 == false)
                {

                    helperSelected2 = true;
                    var sortedTable = dataTable
                     .AsEnumerable()
                     .OrderBy(row => row.Field<int>("LENGTH"))
                     .CopyToDataTable();


                    for (int i = 0; i < sortedTable.Rows.Count; i++)
                    {
                        dataTable.Rows[i]["POSITION"] = sortedTable.Rows[i]["POSITION"];
                        dataTable.Rows[i]["SELECT"] = sortedTable.Rows[i]["SELECT"];
                        dataTable.Rows[i]["NAME"] = sortedTable.Rows[i]["NAME"];
                        dataTable.Rows[i]["TYPE"] = sortedTable.Rows[i]["TYPE"];
                        dataTable.Rows[i]["LENGTH"] = sortedTable.Rows[i]["LENGTH"];
                    }

                }
                else if (helperSelected2 == true) {

                    var sortedTable = dataTable
                    .AsEnumerable()
                    .OrderByDescending(row => row.Field<int>("LENGTH"))
                    .CopyToDataTable();

                    for (int i = 0; i < sortedTable.Rows.Count; i++)
                    {
                        dataTable.Rows[i]["POSITION"] = sortedTable.Rows[i]["POSITION"];
                        dataTable.Rows[i]["SELECT"] = sortedTable.Rows[i]["SELECT"];
                        dataTable.Rows[i]["NAME"] = sortedTable.Rows[i]["NAME"];
                        dataTable.Rows[i]["TYPE"] = sortedTable.Rows[i]["TYPE"];
                        dataTable.Rows[i]["LENGTH"] = sortedTable.Rows[i]["LENGTH"];
                    }


                    helperSelected2 = false;
                
                }
               
            }
            else if (selectedItem == "Name")
            {
                comboBoxSort.SelectedIndex = -1;

                if (helperSelected3 == false)
                {

                    var sortedTable = dataTable
                      .AsEnumerable()
                      .OrderBy(row => row.Field<string>("NAME"))
                      .CopyToDataTable();


                    for (int i = 0; i < sortedTable.Rows.Count; i++)
                    {
                        dataTable.Rows[i]["POSITION"] = sortedTable.Rows[i]["POSITION"];
                        dataTable.Rows[i]["SELECT"] = sortedTable.Rows[i]["SELECT"];
                        dataTable.Rows[i]["NAME"] = sortedTable.Rows[i]["NAME"];
                        dataTable.Rows[i]["TYPE"] = sortedTable.Rows[i]["TYPE"];
                        dataTable.Rows[i]["LENGTH"] = sortedTable.Rows[i]["LENGTH"];
                    }

                    helperSelected3 = true;

                }
                else if (helperSelected3 == true) {

                    var sortedTable = dataTable
                        .AsEnumerable()
                        .OrderByDescending(row => row.Field<string>("NAME"))
                        .CopyToDataTable();

                    for (int i = 0; i < sortedTable.Rows.Count; i++)
                    {
                        dataTable.Rows[i]["POSITION"] = sortedTable.Rows[i]["POSITION"];
                        dataTable.Rows[i]["SELECT"] = sortedTable.Rows[i]["SELECT"];
                        dataTable.Rows[i]["NAME"] = sortedTable.Rows[i]["NAME"];
                        dataTable.Rows[i]["TYPE"] = sortedTable.Rows[i]["TYPE"];
                        dataTable.Rows[i]["LENGTH"] = sortedTable.Rows[i]["LENGTH"];
                    }

                    helperSelected3 = false;


                }

            }
            else if (selectedItem == "Type")
            {
                comboBoxSort.SelectedIndex = -1;

                if (helperSelected4 == false)
                {
                    var sortedTable = dataTable
                         .AsEnumerable()
                         .OrderBy(row => row.Field<string>("TYPE"))
                         .CopyToDataTable();


                    for (int i = 0; i < sortedTable.Rows.Count; i++)
                    {
                        dataTable.Rows[i]["POSITION"] = sortedTable.Rows[i]["POSITION"];
                        dataTable.Rows[i]["SELECT"] = sortedTable.Rows[i]["SELECT"];
                        dataTable.Rows[i]["NAME"] = sortedTable.Rows[i]["NAME"];
                        dataTable.Rows[i]["TYPE"] = sortedTable.Rows[i]["TYPE"];
                        dataTable.Rows[i]["LENGTH"] = sortedTable.Rows[i]["LENGTH"];
                    }

                    helperSelected4 = true;
                }
                else if (helperSelected4 == true) { 
                
                        var sortedTable = dataTable
                        .AsEnumerable()
                        .OrderByDescending(row => row.Field<string>("TYPE"))
                        .CopyToDataTable();

                    for (int i = 0; i < sortedTable.Rows.Count; i++)
                    {
                        dataTable.Rows[i]["POSITION"] = sortedTable.Rows[i]["POSITION"];
                        dataTable.Rows[i]["SELECT"] = sortedTable.Rows[i]["SELECT"];
                        dataTable.Rows[i]["NAME"] = sortedTable.Rows[i]["NAME"];
                        dataTable.Rows[i]["TYPE"] = sortedTable.Rows[i]["TYPE"];
                        dataTable.Rows[i]["LENGTH"] = sortedTable.Rows[i]["LENGTH"];
                    }

                    helperSelected4 = false;


                }

            }
            else if (selectedItem == "Selected")
            {
                comboBoxSort.SelectedIndex = -1;

                if (helperSelected5 == false)
                {
                    var sortedTable = dataTable
                         .AsEnumerable()
                         .OrderByDescending(row => row.Field<bool>("SELECT"))
                         .CopyToDataTable();


                    for (int i = 0; i < sortedTable.Rows.Count; i++)
                    {
                        dataTable.Rows[i]["POSITION"] = sortedTable.Rows[i]["POSITION"];
                        dataTable.Rows[i]["SELECT"] = sortedTable.Rows[i]["SELECT"];
                        dataTable.Rows[i]["NAME"] = sortedTable.Rows[i]["NAME"];
                        dataTable.Rows[i]["TYPE"] = sortedTable.Rows[i]["TYPE"];
                        dataTable.Rows[i]["LENGTH"] = sortedTable.Rows[i]["LENGTH"];
                    }

                    helperSelected5 = true;
                }
                else if (helperSelected5 == true)
                {

                    var sortedTable = dataTable
                    .AsEnumerable()
                    .OrderBy(row => row.Field<bool>("SELECT"))
                    .CopyToDataTable();

                    for (int i = 0; i < sortedTable.Rows.Count; i++)
                    {
                        dataTable.Rows[i]["POSITION"] = sortedTable.Rows[i]["POSITION"];
                        dataTable.Rows[i]["SELECT"] = sortedTable.Rows[i]["SELECT"];
                        dataTable.Rows[i]["NAME"] = sortedTable.Rows[i]["NAME"];
                        dataTable.Rows[i]["TYPE"] = sortedTable.Rows[i]["TYPE"];
                        dataTable.Rows[i]["LENGTH"] = sortedTable.Rows[i]["LENGTH"];
                    }

                    helperSelected5 = false;


                }

            }

        }
    }


    public class Selected
    {

        public bool IsSelected { get; set; }

    }



}

