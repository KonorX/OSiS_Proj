using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.IO;
using OxyPlot.WindowsForms;

namespace OSiS_Proj
{
    public partial class CpuForm : Form
    {
        private bool isRefreshingEvery5Sec = false;
        
        PlotModel myModel;
        PlotModel refreshModel;
        LineSeries lineSeries = new LineSeries()
        {
            Color = OxyColor.FromRgb(60, 115, 232),
            MarkerSize = 3,
            MarkerFill = OxyColors.LightBlue,
            MarkerStroke = OxyColors.Blue,
            MarkerStrokeThickness = 0.75
        };
        public double currentX=0;
        public double firstY;
        public CpuForm()
        {
            
            
            
            InitializeComponent();
            button2.Visible = false;
            button1.Visible = true;
            
            myModel = new PlotModel { Title = "Нагрузка процессора" };
            myModel.Axes.Add(new LinearAxis {Title="", Position=AxisPosition.Left, Minimum=0, Maximum=100 });
            InfoClass.CpuRefresh();
            firstY = InfoClass.CpuCores.Last<double>();
            lineSeries.Points.Add(new DataPoint(currentX, firstY));
            currentX++;
            InfoClass.CpuRefresh();
            lineSeries.Points.Add(new DataPoint(currentX, InfoClass.CpuCores.Last<double>()));
            myModel.Series.Add(lineSeries);
            this.plot.Model = myModel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            currentX += 1;
            button2.Visible = true;
            InfoClass.CpuRefresh();
            lineSeries.Points.Add(new DataPoint(currentX, InfoClass.CpuCores.Last<double>()));
            myModel.Series.Clear();
            myModel.Series.Add(lineSeries);
            this.plot.Model = refreshModel;
            this.plot.Model = myModel;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Картинки|*.png";
            if (save.ShowDialog()==DialogResult.OK)
            {


                using (var stream = File.Create(save.FileName)) ;

                var pngExporter = new PngExporter { Width = 800, Height = 450 };
                pngExporter.ExportToFile(myModel, save.FileName);
                
                
            }
        }

        private void RefreshInfo()
        {
            currentX += 1;
            InfoClass.CpuRefresh();
            lineSeries.Points.Add(new DataPoint(currentX, InfoClass.CpuCores.Last<double>()));
            myModel.Series.Clear();
            myModel.Series.Add(lineSeries);
            this.plot.Model = refreshModel;
            this.plot.Model = myModel;
            Thread.Sleep(5000);
        }
    }
}
