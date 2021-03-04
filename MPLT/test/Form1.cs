using System;
using System.Threading;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
//using Petroware.LogIo.Las;
using ElencySolutions.CsvHelper;
using Microsoft.VisualBasic.FileIO;
using System.Windows.Forms.Integration;
using SciChartExport;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MPLT.test
{
    public partial class Form1 : Form
    {
        public static string FilePath = "";
        public static DataTable dt_verinfo = new DataTable();
        public static DataTable dt_wellinfo = new DataTable();
        public static DataTable dt_curlinfo = new DataTable();
        public static  DataTable dt_asciinfo = new DataTable();
        public delegate DataTable asci_rows();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            //
            // Create LasFile instance
            //
            LasFile lasFile = new LasFile("WLC_COMPOSITE", LasVersion.VERSION_2_0);
            //
            // Create the mandatory WELL section
            //
            LasSection wellSection = new LasSection("Well");
            wellSection.AddRecord(new LasParameterRecord("WELL", null, "16/2-16", "Well name"));
            wellSection.AddRecord(new LasParameterRecord("CTRY", null, "Norway", "Country"));
            wellSection.AddRecord(new LasParameterRecord("COMP", null, "Lundin", "Company"));
            wellSection.AddRecord(new LasParameterRecord("FLD", null, "Johan Sverdrup", "Field name"));
            wellSection.AddRecord(new LasParameterRecord("SRVC", null, "Schlumberger", "Service company"));
            lasFile.AddSection(wellSection);
            //
            // Create an optional parameter section
            //
            LasSection parameterSection = new LasSection("Parameters");
            parameterSection.AddRecord(new LasParameterRecord("Rshl", "OHMM", "2.0000", "Resistivity shale"));
            parameterSection.AddRecord(new LasParameterRecord("TLI", null, "149.5000", "Top Logged Interval"));
            parameterSection.AddRecord(new LasParameterRecord("VshCutoff", "V/V", "0.2500", "Shale Volume Cutoff"));
            lasFile.AddSection(parameterSection);
            //
            // Create and populate curves
            //
            LasCurve depthCurve = new LasCurve("DEPT", "m", "Measured depth", typeof(double));
            depthCurve.AddValue(4350.00);
            depthCurve.AddValue(4350.50);
            depthCurve.AddValue(4351.00);
            depthCurve.AddValue(4351.50);
            depthCurve.AddValue(4352.00);
            lasFile.AddCurve(depthCurve);
            LasCurve gammaRayCurve = new LasCurve("GR", "gAPI", "Gamma ray", typeof(double));
            gammaRayCurve.AddValue(12.3);
            gammaRayCurve.AddValue(8.43);
            gammaRayCurve.AddValue(null);  // No-value
            gammaRayCurve.AddValue(4.1);
            gammaRayCurve.AddValue(7.29);
            lasFile.AddCurve(gammaRayCurve);
            //
            // Write instance to disk
            //
            LasFileWriter.Write(new FileInfo("/path/to/file.LAS"), lasFile);
            */
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //dt_asciinfo.Rows.Clear();
            //dt_asciinfo.Columns.Clear();
            //dt_asciinfo.Dispose();
            //dt_asciinfo.Reset();
            dt_verinfo = new DataTable();
            dt_wellinfo = new DataTable();
            dt_curlinfo = new DataTable();
            dt_asciinfo = new DataTable();
            dataGridView1.Refresh();
            dataGridView2.Refresh();
            dataGridView3.Refresh();
            dataGridView4.Refresh();
            comboBox1.Items.Clear();
            button4.Visible = false;
            label5.Text = "Rows count:";
            openFileDialog1.InitialDirectory = @"D:\project\nhi\mplt\lac doc";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Title = "Browse LAS Files";
            openFileDialog1.Filter = "LAS files (*.LAS)|*.LAS|txt files (*.txt)|*.txt|All files (*.*)|*.*";
            this.openFileDialog1.Multiselect = true;
            // openFileDialog1.ShowDialog();
            //string FilePath = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FilePath = openFileDialog1.FileName;
            }

          //  Thread th = new Thread(READ_lAS);
            //th.Join();// = true;
           Stopwatch st = new Stopwatch();
            st.Start();
           // READ_lAS();
           // th.Start();
            
            asci_rows dtg_asci = new asci_rows(READ_lAS);
            dtg_asci();
            dataGridView1.Invoke((Action)(() => dataGridView1.DataSource = dt_asciinfo));
            dataGridView2.Invoke((Action)(() => dataGridView2.DataSource = dt_verinfo));
            dataGridView3.Invoke((Action)(() => dataGridView3.DataSource = dt_wellinfo));
            dataGridView4.Invoke((Action)(() => dataGridView4.DataSource = dt_curlinfo));
             dataGridView2.Columns[0].Width = 130;
             dataGridView2.Columns[1].Width = 220;
             dataGridView3.Columns[0].Width = 150;
             dataGridView3.Columns[1].Width = 220;
             dataGridView4.Columns[0].Width = 140;
            dataGridView4.Columns[1].Width = 250;
            //dataGridView2.Invoke((Action)(() => dataGridView2.Columns[0].Width = 130));
            label5.Invoke((Action)(() => label5.Text=label5.Text + string.Format("{0:0,0}", dt_asciinfo.Rows.Count)));
            label3.Invoke((Action)(() => label3.Text = label3.Text + string.Format("{0:0,0}", dt_curlinfo.Rows.Count)));
            label2.Invoke((Action)(() => label2.Text = label2.Text + string.Format("{0:0,0}", dt_wellinfo.Rows.Count)));
            label1.Invoke((Action)(() => label1.Text = label1.Text + string.Format("{0:0,0}", dt_verinfo.Rows.Count)));
            loadXMehver();
            TimeSpan tmst = new TimeSpan(st.Elapsed.Ticks);
            this.Text = string.Format("{0:00}:{1:00}:{2:00}:{3:000} Read Duration", tmst.Hours, tmst.Minutes, tmst.Seconds, tmst.Milliseconds);
        }

        private static DataTable READ_lAS( )
        {
           

            List<List<string>> records = new List<List<string>>();

            using (CsvReader reader = new CsvReader(FilePath, Encoding.Default))
            {
                while (reader.ReadNextRecord())
                    records.Add(reader.Fields);

            }
            bool verinfo = false;
            bool wellinfo = false;
            bool curinfo = false;
            bool ascinfo = false;
            int cols = 0;

            foreach (List<string> list in records)
            {
                foreach (string s in list)
                {
                    //add Version Info
                    if (s.StartsWith("~VERSION") | s.StartsWith("~V"))
                    {
                        verinfo = true;
                        dt_verinfo.Columns.Add("KeyValue");
                        // dt_verinfo.Columns.Add("Value");
                        dt_verinfo.Columns.Add("Description");
                        // dt_verinfo.Columns.Add("Reserved");
                        continue;
                    }


                    if (verinfo & !s.StartsWith("~WEL") & !s.StartsWith("~W"))
                    {

                        //textBox1.Text = textBox1.Text + s + "\r\n";
                        char[] delimiterChars = { ':' };
                        //s.Replace(":", " ");
                        string[] st = s.Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);


                        dt_verinfo.Rows.Add(st);

                    }
                    //add well info
                    if (s.StartsWith("~WEL") | s.StartsWith("~W"))
                    {
                        verinfo = false;
                        wellinfo = true;
                        dt_wellinfo.Columns.Add("KeyValue");
                        // dt_wellinfo.Columns.Add("Value");
                        dt_wellinfo.Columns.Add("Description");
                        // dt_wellinfo.Columns.Add("Reserved");
                        continue;
                    }

                    if (wellinfo & !s.StartsWith("~C") & !s.StartsWith("#"))
                    {
                        // textBox2.Text = textBox2.Text + s + "\r\n";
                        char[] delimiterChars = { ':' };
                        //s.Replace(":", " ");
                        string[] st = s.Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);
                        dt_wellinfo.Rows.Add(st);

                    }
                    //add curl info
                    if (s.StartsWith("~C"))
                    {
                        verinfo = false;
                        wellinfo = false;
                        curinfo = true;
                        dt_curlinfo.Columns.Add("KeyValue");
                        // dt_curlinfo.Columns.Add("Value");
                        dt_curlinfo.Columns.Add("Description");
                        // dt_curlinfo.Columns.Add("Reserved");
                        continue;
                    }

                    if (curinfo & (!s.StartsWith("~ASC") | !s.StartsWith("~A") | !s.StartsWith("# ") | !s.StartsWith("#-")))
                    {
                        if (s.StartsWith("#-") | s.StartsWith("~P") | s.StartsWith("#M"))
                            continue;
                        if (s.StartsWith("# ") | s.StartsWith("~A"))
                        {
                            //string[] t=s.Replace("# ", "");

                            char[] delimiterChars = { ' ', ',', ':', '\t' };
                            string[] dg_rows = s.Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 1; i < dg_rows.Count(); i++)
                            {

                                dt_asciinfo.Columns.Add(dg_rows[i]);

                            }

                        }
                        if (!s.StartsWith("~A"))
                        {
                            //textBox3.Text = textBox3.Text + s + "\r\n";
                            char[] delimiterChars2 = { ':' };
                            //s.Replace(":", " ");
                            string[] st = s.Split(delimiterChars2, System.StringSplitOptions.RemoveEmptyEntries);
                            dt_curlinfo.Rows.Add(st);
                        }
                    }

                    //add acci info
                    if (s.StartsWith("~ASCII") | s.StartsWith("~A"))
                    {
                        verinfo = false;
                        wellinfo = false;
                        curinfo = false;
                        ascinfo = true;
                        continue;
                    }

                    if (ascinfo & !s.StartsWith("~ASCII") & !s.StartsWith("#") & !s.Trim().StartsWith(".") & !s.StartsWith("~A"))
                    {
                        char[] delimiterChars = { ' ', ',', ':', '\t' };
                        string[] st = s.Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);
                        //for (Int32 i = 0; i < st.Length; ++i)
                        //{
                        if (st.Length > 1)
                            dt_asciinfo.Rows.Add(st);
                        //}
                        //ListViewItem itm;                       
                        //itm = new ListViewItem(st);
                        //listView1.Items.Add(itm) ;
                        //listView1.Visible = true;
                        //textBox4.Text = textBox4.Text + st.ToString() + "\r\n";


                    }




                }
            }
            try
            {
               // dataGridView4.DataSource = dt_curlinfo;
               // dataGridView3.DataSource = dt_wellinfo;
               // dataGridView2.DataSource = dt_verinfo;
               // dataGridView2.Columns[0].Width = 130;
               // dataGridView2.Columns[1].Width = 220;
               // dataGridView3.Columns[0].Width = 150;
               // dataGridView3.Columns[1].Width = 220;
               // dataGridView4.Columns[0].Width = 140;
               // dataGridView4.Columns[1].Width = 250;
               // dataGridView1.DataSource = dt_asciinfo;
               //// asci_rows = RowS();
               // label5.Text = label5.Text + string.Format("{0:0,0}", dt_asciinfo.Rows.Count); 
               // label3.Text = label3.Text + string.Format("{0:0,0}", dt_curlinfo.Rows.Count);
               // label2.Text = label2.Text + string.Format("{0:0,0}", dt_wellinfo.Rows.Count);
               // label1.Text = label1.Text + string.Format("{0:0,0}", dt_verinfo.Rows.Count);
               // loadXMehver();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return dt_asciinfo;
        }
        
        private delegate void RowS();
        //{
        //    label5.Text + string.Format("{0:0,0}", dt_asciinfo.Rows.Count);
        //}

        private void loadXMehver()
        {
            for (int i = 1; i < dt_asciinfo.Columns.Count-1; i++)
            {
                //(dt_asciinfo.Columns[i].ToString())
                comboBox1.Items.Add ( dt_asciinfo.Columns[i].ToString());
               
            }
        }

        private static DataTable GenerateDataTable1(string fileName, bool firstRowContainsFieldNames = true)
        {
            DataTable result = new DataTable();

            if (fileName == "")
            {
                return result;
            }

            string delimiters = ",";
            string extension = Path.GetExtension(fileName);

            if (extension.ToLower() == ".txt")
                delimiters = "\t";
            else if (extension.ToLower() == "csv")
                delimiters = ",";

            using (TextFieldParser tfp = new TextFieldParser(fileName))
            {
                tfp.SetDelimiters(delimiters);

                // Get The Column Names
                if (!tfp.EndOfData)
                {
                    string[] fields = tfp.ReadFields();
                    char[] delimiterChars = { ' ', ',', ':', '\t' };
                    string[] st = fields[0].Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);


                    for (int i = 0; i < st.Count(); i++)
                    {
                        if (firstRowContainsFieldNames)
                            result.Columns.Add(st[i]);
                        else
                            result.Columns.Add("Col" + i);
                    }

                    // If first line is data then add it
                    if (!firstRowContainsFieldNames)
                        result.Rows.Add(st);
                }

                // Get Remaining Rows from the CSV
                //
                while (!tfp.EndOfData)
                {
                    char[] delimiterChars = { ' ', ',', ':', '\t' };
                    string[] recordsarray = tfp.ReadFields();
                    recordsarray = recordsarray[0].Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries); ;
                    result.Rows.Add(recordsarray);
                }
            }

            return result;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("c1", 220);
            listView1.Columns.Add("c2", 220);
        }

        private void button3_Click(object sender, EventArgs e)
        {
          

        }
        private static DataTable GenerateDataTable(string fileName, bool firstRowContainsFieldNames = true)
        {
            DataTable result = new DataTable();

            if (fileName == "")
            {
                return result;
            }

            string delimiters = ",";
            string extension = Path.GetExtension(fileName);

            if (extension.ToLower() == ".txt")
                delimiters = "\t";
            else if (extension.ToLower() == "csv")
                delimiters = ",";

            using (TextFieldParser tfp = new TextFieldParser(fileName))
            {
                tfp.SetDelimiters(delimiters);

                // Get The Column Names
                if (!tfp.EndOfData)
                {
                    string[] fields = tfp.ReadFields();
                    char[] delimiterChars = { ' ', ',', ':', '\t' };
                    string[] st = fields[0].Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);
                    
                    
                    for (int i = 0; i < st.Count(); i++)
                    {
                        if (firstRowContainsFieldNames)
                            result.Columns.Add(st[i]);
                        else
                            result.Columns.Add("Col" + i);
                    }

                    // If first line is data then add it
                    if (!firstRowContainsFieldNames)
                        result.Rows.Add(st);
                }

                // Get Remaining Rows from the CSV
               //
                while (!tfp.EndOfData)
                {
                    char[] delimiterChars = { ' ', ',', ':', '\t' };
                    string[] recordsarray = tfp.ReadFields();
                     recordsarray= recordsarray[0].Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries); ;
                    result.Rows.Add(recordsarray);
                }
            }

            return result;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //var wpfwindow = new WPFWindow.Window1();
            //ElementHost.EnableModelessKeyboardInterop(wpfwindow);
            //wpfwindow.Show();
            //create the winform Hoster, which contains a ElementHost on it
            // test.Form1 MyForm = new test.Form1();

            //Create the instance of your WPF control
            // UserControl1 MyWPF = new UserControl1();

            //Add the WPF control to the elementHost  (it is publicly accessible...i know, it's bad..)
            // MyForm.elementHost1.child = MyWPF;

            //show the WinForm : 
            //MyForm.ShowDialog();

            //ElementHost host = new ElementHost();
            //System.Windows.Controls.ListBox wpfListBox = new System.Windows.Controls.ListBox();
            //for (int i = 0; i < 10; i++)
            //{
            //    wpfListBox.Items.Add("Item " + i.ToString());
            //}
            //host.Dock = DockStyle.Fill;
            //host.Controls.Add(wpfListBox);
            //this.panel1.Controls.Add(host);



            //ElementHost control as in the above example:
            //ElementHost host = new ElementHost();
            //UserControl1 uc1 = new UserControl1();
            //host.Controls.Add(uc1);
            //host.Dock = DockStyle.Fill;
            //this.panel1.Controls.Add(host);


            var wpfwindow = new SciChartExport.MainWindow();
            ElementHost.EnableModelessKeyboardInterop(wpfwindow);
            wpfwindow.wpf_dt_asciinfo = dt_asciinfo;
             
           SciChartExport.Global.GlobalVar = dt_asciinfo;
            SciChartExport.Global.GlobalYmehvar = comboBox1.SelectedIndex+1;
            wpfwindow.Show();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString().Length > 0)
                button4.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            dataGridView1.DataSource = "";
            dataGridView1.Refresh();
            generate_fake_ascci();
        }

        private void generate_fake_ascci()
        {
            dt_asciinfo = new DataTable();
            for (int i = 0; i < dt_curlinfo.Rows.Count ; i++)
            {
               // char[] delimiterChars = { ' ', ',', ':', '\t' };
                //string[] dg_rows = dt_curlinfo.Rows[i][0].ToString().Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);
                if (!dt_curlinfo.Rows[i][0].ToString().StartsWith("#")) { 
                string[] col = dt_curlinfo.Rows[i][0].ToString().Split('.');
                                dt_asciinfo.Columns.Add(col[0].Trim());// dt_curlinfo.Rows[i].ToString());
                                                                       //comboBox1.Items.Add(dt_asciinfo.Columns[i].ToString());
                                                                       //MessageBox.Show(dt_curlinfo.Rows[i][0].ToString());
                }
            }
            for (int i = 0; i < dt_wellinfo.Rows.Count; i++)
            {
                 char[] delimiterChars = { ' ', ',', ':', '\t' };
                string[] dg_rows = dt_wellinfo.Rows[i][0].ToString().Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);
                string[] col = dt_wellinfo.Rows[i][0].ToString().Split('.');
                var regex = new Regex(".*my (.*) is.*");
                var myCapturedText = regex.Match("This is an example string and my data is here").Groups[1].Value;
               var  resultString = Regex.Match(dt_wellinfo.Rows[i][0].ToString(), @"\d+").Value;
                //dt_asciinfo.Columns.Add(col[0].Trim());// dt_curlinfo.Rows[i].ToString());
                if (col[0].StartsWith("ST"))
                    MessageBox.Show(Regex.Match(dt_wellinfo.Rows[i][0].ToString(), @"\d+").Value);
                if (col[0].StartsWith("STOP"))
                    MessageBox.Show(Regex.Match(dt_wellinfo.Rows[i][0].ToString(), @"\d+").Value);
              //  MessageBox.Show(dt_wellinfo.Rows[i][0].ToString());
            }

            dataGridView1.DataSource = dt_asciinfo;
                dataGridView1.Refresh();
            
        }
    }
}
