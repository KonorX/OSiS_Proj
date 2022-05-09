using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using OxyPlot.WindowsForms;
using System.IO;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot;


namespace OSiS_Proj
{
    public partial class Ram : Form
    {
        PlotModel myModel;
        PlotModel refreshModel;
        LineSeries lineSeries = new LineSeries()
        {
            Color = OxyColor.FromRgb(139, 0, 255),
            MarkerSize = 3,
            MarkerFill = OxyColors.Violet,
            MarkerStroke = OxyColors.DarkViolet,
            MarkerStrokeThickness = 0.75
        };
        public double currentX = 0;
        public double firstY;
        public Ram()
        {
            InitializeComponent();
            button1.Visible = false;
            
            myModel = new PlotModel { Title = "Оперативной памяти доступно" };
            myModel.Axes.Add(new LinearAxis { Title = "", Position = AxisPosition.Left, Minimum = 0, Maximum = 32768 });

            firstY = InfoClass.GetRam();
            lineSeries.Points.Add(new DataPoint(currentX, firstY));
            currentX++;
            
            lineSeries.Points.Add(new DataPoint(currentX, InfoClass.GetRam()));
            myModel.Series.Add(lineSeries);
            this.plot.Model = myModel;
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            currentX += 1;
            button1.Visible = true;
            lineSeries.Points.Add(new DataPoint(currentX, InfoClass.GetRam()));
            myModel.Series.Clear();
            myModel.Series.Add(lineSeries);
            this.plot.Model = refreshModel;
            this.plot.Model = myModel;
        }
    }
}
