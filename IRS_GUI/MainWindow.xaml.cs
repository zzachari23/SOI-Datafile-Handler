using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml;
using Microsoft.Office.Interop.Excel;
using System.Security.Cryptography;

namespace IRS_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {

        public System.Data.DataTable dataTable { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private string filePath = string.Empty;



        private void MyButton1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "txt files (*.txt)|*.txt|Data files (*.dat)|*.dat|Excel files (*.xlsx, *.xls)|*.xlsx;*.xls\"";
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                TextBox1.Text = filePath;
                string fileExtension = System.IO.Path.GetExtension(filePath);

                if (fileExtension != ".xlsx")
                {

                    if (CheckXMLFile(filePath))
                    {
                        MessageBox.Show("File content is in valid XML format");
                    }
                    else
                    {
                        MessageBox.Show("Warning \u26A0: File content not in XML format");
                        readDataFile();
                    }
                }
            }
        }

        private void readDataFile()
        {
            string[] lines = File.ReadAllLines(filePath);
            string[,] fieldData = new string[2, 4]; //I will know how many columns there are based on how many 


            for (int i = 0; i < lines.Length; i++) {

                string[] splitData = lines[i].Split('+');

                for (int j = 0; j < splitData.Length; j++) {

                    fieldData[i,j] = splitData[j];
                }
            }

            for (int i = 0; i < fieldData.GetLength(1); i++)
            {
                for (int j = 0; j < fieldData.GetLength(0); j++)
                {
                    Console.WriteLine(fieldData[j, i]);
                }
            }




          





        }


        private void MyButton2(object sender, RoutedEventArgs e)
        {

            if (comboBox1.SelectedItem != null) {

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|Data files (*.dat)|*.dat|Excel files (*.xlsx, *.xls)|*.xlsx;*.xls\"";
                openFileDialog.FilterIndex = 0;
                openFileDialog.RestoreDirectory = true;


                if (openFileDialog.ShowDialog() == true)
                {
                    filePath = openFileDialog.FileName;
                    TextBox2.Text = filePath;
                    string fileExtension = System.IO.Path.GetExtension(filePath);

                    if (fileExtension != ".xlsx")
                    {

                        if (CheckXMLFile(filePath))
                        {
                            MessageBox.Show("File content is in valid XML format");
                        }
                        else
                        {
                            MessageBox.Show("Warning \u26A0: File content not in XML format");
                        }
                    }
                }
                

            }
            else
            {

                MessageBox.Show("Record Layout file type not chosen!");

            }


        }

        private bool CheckXMLFile(string filePath)
        {
            try
            {
                XmlDocument xmlDocObj = new XmlDocument();
                xmlDocObj.Load(filePath);
                return true;

            }
            catch (XmlException)
            {
                return false;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }


        private void OpenWindow1(object sender, RoutedEventArgs e)
        {

            if (comboBox1.SelectedItem != null && TextBox2.Text != "")
            {
                try
                {
                    Window1 newWindow = new Window1(filePath, (myUpDownControl.Value ?? 1), ((ComboBoxItem)comboBox1.SelectedItem).Content.ToString(), this);
                    newWindow.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(" \U0000274C Data is in incorrect format" + "\n" + "Error message: " + ex.Message);
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                if (comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("Please select a file type option!!");
                }
                else if (TextBox2.Text == "")
                {
                    MessageBox.Show("Please upload a record layout file!!");
                }
            }
        }

        private void OpenWindow2(object sender, RoutedEventArgs e)
        {
            if (dataTable != null)
            {
                helloBlock.Text = (string)(dataTable.Rows[5]["NAME"]);
                Window2 window2 = new Window2(dataTable);
                window2.ShowDialog();
            }
            else {

                MessageBox.Show("Record Description is not loaded in!");
               
            }
        }
    }
}
