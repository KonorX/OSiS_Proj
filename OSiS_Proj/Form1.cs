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
using System.Management;
using Microsoft.VisualBasic;

namespace OSiS_Proj
{
    public partial class Form1 : Form
    {
        private List<Process> processes = null;
        public Form1()
        {
            InitializeComponent();                                                          
        }

        private void GetProcesses()
        {
            processes.Clear();
            processes = Process.GetProcesses().ToList<Process>();

        }

        private void RefreshListBox()
        {
            listView1.Items.Clear();

            double memorySize = 0;

            foreach (Process proc in processes)
            {
                memorySize = 0;
                PerformanceCounter pc = new PerformanceCounter();
                pc.CategoryName = "Process";
                pc.CounterName = "Working Set - Private";
                pc.InstanceName = proc.ProcessName;
                memorySize = (double)pc.NextValue() / 1000000;
                string[] row = new string[] { proc.ProcessName, Math.Round(memorySize,1).ToString() };

                listView1.Items.Add(new ListViewItem(row));

                pc.Close();
                pc.Dispose();

            }
            Text = "Запущено процессов: " + processes.Count.ToString();
        }

        private void RefreshListBox(List<Process> processes, string keyword)
        {
            try
            {
                listView1.Items.Clear();

                double memorySize = 0;

                foreach (Process proc in processes)
                {
                    if (proc != null)
                    {
                        memorySize = 0;
                        PerformanceCounter pc = new PerformanceCounter();
                        pc.CategoryName = "Process";
                        pc.CounterName = "Working Set - Private";
                        pc.InstanceName = proc.ProcessName;
                        memorySize = (double)pc.NextValue() / 1000000;
                        string[] row = new string[] { proc.ProcessName, Math.Round(memorySize, 1).ToString() };

                        listView1.Items.Add(new ListViewItem(row));

                        pc.Close();
                        pc.Dispose();
                    }


                }
                Text = $"Запущено процессов {keyword} : " + processes.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            
        }

        private void KillProcess(Process process)
        {
            process.Kill();
            process.WaitForExit();

        }

        private void KillProcessAndChildren(int pID)
        {
            if (pID==0)
            {
                return;
            }

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                "Select * From Win32_Process Where ParentProcessID="+pID);
            ManagementObjectCollection objectCollection = searcher.Get();
            foreach (ManagementObject obj in objectCollection)
            {
                KillProcessAndChildren(Convert.ToInt32(obj["ProcessID"]));
            }

            try
            {
                Process process = Process.GetProcessById(pID);

                process.Kill();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }

        }

        private int GetParentProcessID(Process process)
        {
            int parentID = 0;

            try
            {
                ManagementObject managementObject = new ManagementObject("win32_process.handle='" + process.Id + "'");

                managementObject.Get();

                parentID = Convert.ToInt32(managementObject["ParentProcessId"]);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }

            return parentID;
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            processes = new List<Process>();

            GetProcesses();

            RefreshListBox();




        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            GetProcesses();

            RefreshListBox();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems[0]!=null)
                {
                    Process processToKill = processes.Where((x) => x.ProcessName ==
                      listView1.SelectedItems[0].SubItems[0].Text).ToList()[0];

                    KillProcess(processToKill);

                    GetProcesses();
                    RefreshListBox();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems[0] != null)
                {
                    Process processToKill = processes.Where((x) => x.ProcessName ==
                      listView1.SelectedItems[0].SubItems[0].Text).ToList()[0];

                    KillProcessAndChildren(GetParentProcessID(processToKill));

                    GetProcesses();
                    RefreshListBox();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void завершитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton2_Click(sender, e);
        }

        private void завершитьДеревоПроцессовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton3_Click(sender, e);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            string path = Interaction.InputBox("Введите имя задачи","Запуск");

            try
            {
                Process.Start(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            GetProcesses();

            List<Process> filteredProcesses = processes.Where((x) => x.ProcessName.ToLower().Contains(toolStripTextBox1.Text.ToLower())).ToList<Process>();

            RefreshListBox(filteredProcesses, toolStripTextBox1.Text);
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            plotChoose plot = new plotChoose();
            plot.ShowDialog();
            
            
        }
    }
}
