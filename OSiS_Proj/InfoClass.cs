using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Runtime.InteropServices;
using System.Diagnostics.PerformanceData;
using System.Diagnostics;


namespace OSiS_Proj
{
    public static class InfoClass
    {
        public static List<double> CpuCores = new List<double>();
        public static Int64 AllRam;
        public static PerformanceCounter ramCounter=new PerformanceCounter("Memory","Available MBytes");
        public static Int64 FreeRam;

        public static void CpuRefresh()
        {
            CpuCores.Clear();

            ManagementObjectSearcher searcherProcUsage = new ManagementObjectSearcher("select * from Win32_PerfFormattedData_PerfOS_Processor");
            foreach (ManagementObject obj in searcherProcUsage.Get())
            {
                CpuCores.Add(Convert.ToInt32(obj["PercentProcessorTime"]));
            }
        }

        public static string RefreshRam()
        {
            return ramCounter.NextValue().ToString()+"МБ";
        }

        public static double GetRam()
        {
            return ramCounter.NextValue();
        }


    }

    
}
