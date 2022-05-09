using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OxyPlot.WindowsForms;

namespace OSiS_Proj
{
    public partial class NetForm : Form
    {
        PlotModel myModel;
        PlotModel refreshModel;
        LineSeries lineSeries = new LineSeries()
        {
            Color = OxyColor.FromRgb(0,192,0),
            MarkerSize = 3,
            MarkerFill = OxyColors.Violet,
            MarkerStroke = OxyColors.DarkViolet,
            MarkerStrokeThickness = 0.75
        };
        long netCounter = 0;
        long NCounter = 0;
        public double currentX = 0;
        public double firstY;
        public NetForm()
        {
            InitializeComponent();
            myModel = new PlotModel { Title = "Скорость интернета входящая КБ в c" };
            myModel.Axes.Add(new LinearAxis { Title = "", Position = AxisPosition.Left, Minimum = 0, Maximum = 15000 });

            //firstY = InfoClass.GetRam();
            //lineSeries.Points.Add(new DataPoint(currentX, firstY));
            //currentX++;

            //lineSeries.Points.Add(new DataPoint(currentX, InfoClass.GetRam()));
            myModel.Series.Add(lineSeries);
            this.plot.Model = myModel;
            DrawNet();
        }

        private async Task<double> GetSpeed()
        {
            PerformanceCounterCategory category = new PerformanceCounterCategory("Network Interface");
            foreach (string networkInterface in category.GetInstanceNames())
            {
                if (networkInterface == "MS TCP Loopback interface")
                    continue;
                PerformanceCounter counter = new PerformanceCounter("Network Interface", "Bytes Received/sec", networkInterface);
                netCounter = counter.NextSample().RawValue;
                
            }
            Thread.Sleep(1000);
            foreach (string networkInterface in category.GetInstanceNames())
            {
                if (networkInterface == "MS TCP Loopback interface")
                    continue;
                PerformanceCounter counter = new PerformanceCounter("Network Interface", "Bytes Received/sec", networkInterface);
                NCounter = counter.NextSample().RawValue;
                
            }


            return (NCounter-netCounter)/1024D;
        }

        private void DrawNet()
        {
            Task<double> firstSpeedTask = GetSpeed();
            //firstSpeedTask.Start();
            double SpeedFirst = firstSpeedTask.Result;
            double SpeedSecond = firstSpeedTask.Result;
            if (firstSpeedTask.Status==TaskStatus.RanToCompletion)
            {
                Task<double> secondSpeedTask = GetSpeed();
                //secondSpeedTask.Start();
                SpeedSecond = secondSpeedTask.Result;

            }
            currentX++;
            lineSeries.Points.Add(new DataPoint(currentX, SpeedFirst));
            currentX++;
            lineSeries.Points.Add(new DataPoint(currentX, SpeedSecond));
            myModel.Series.Clear();
            myModel.Series.Add(lineSeries);
            this.plot.Model = refreshModel;
            this.plot.Model = myModel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task<double> speedTask = GetSpeed();
            currentX++;
            button2.Visible = true;
            lineSeries.Points.Add(new DataPoint(currentX, speedTask.Result));
            myModel.Series.Clear();
            myModel.Series.Add(lineSeries);
            this.plot.Model = refreshModel;
            this.plot.Model = myModel;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Картинки|*.png";
            if (save.ShowDialog() == DialogResult.OK)
            {


                using (var stream = File.Create(save.FileName)) ;

                var pngExporter = new PngExporter { Width = 800, Height = 450 };
                pngExporter.ExportToFile(myModel, save.FileName);


            }
        }
    }
}
