// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// LineChartExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Data;
using SciChartExport;
namespace SciChart.Examples.Examples.CreateSimpleChart
  
{
   public partial class LineChartExampleView : UserControl
    {
        
        public DataTable wp_dt_asciinfo = new DataTable();
        
        public LineChartExampleView()
        {
            InitializeComponent();
        }

        private void LineChartExampleView_OnLoaded(object sender, RoutedEventArgs e)
        {            
            // Create a DataSeries of type X=double, Y=double
            var dataSeries = new XyDataSeries<double, double>();
            string ob = dataSeries.XRange.ToString();
            lineRenderSeries.DataSeries = dataSeries;

            var data = DataManager.Instance.GetFourierSeries(2000, 0.5,10000);
            Random rnd = new Random();
            //int[,] lala = new int[3, 5];
            //for (int i = 0; i < 3; i++)
            //{
            //    for (int j = 0; j < 5; j++)
            //    {
            //        lala[i, j] = rnd.Next(1, 10);
            //        Console.WriteLine("[{0}, {1}] = {2}", i, j, lala[i, j]);
            //    }
            //}
            double[] y = new double[100000];
            for (int xc = 0; xc < 100000; xc++)
                y[xc] = xc + 5 / 5;// rnd.Next(1,100);

            double[] x = new double[100000];
            for (int xc = 0; xc < 100000; xc++)
                x[xc] = xc;// rnd.Next(1, 100000);
                           // Append data to series. SciChart automatically redraws
                           // dataSeries.Append(data.XData, data.YData);
            wp_dt_asciinfo = SciChartExport.Global.GlobalVar;
            double[] Xmehvar = new double[wp_dt_asciinfo.Rows.Count];
            double[] Ymehvar = new double[wp_dt_asciinfo.Rows.Count];
            for (int i= 0; i<wp_dt_asciinfo.Rows.Count;i++)
            {
                if (wp_dt_asciinfo.Rows[i][Global.GlobalYmehvar].ToString().Trim().Length>0)
                Xmehvar[i] = double.Parse(wp_dt_asciinfo.Rows[i][Global.GlobalYmehvar].ToString());
                if (wp_dt_asciinfo.Rows[i][0].ToString().Trim().Length > 0)
                    Ymehvar[i] = double.Parse(wp_dt_asciinfo.Rows[i][0].ToString());
            }
            dataSeries.AcceptsUnsortedData = true;
            dataSeries.Append(Xmehvar, Ymehvar);
            Brush color;
            
          //  sciChart.SciChartSurface = "#FF0000FF";
            sciChart.ZoomExtents();
            //sciChart.Print("NHI Charting");
        }
    }
}
