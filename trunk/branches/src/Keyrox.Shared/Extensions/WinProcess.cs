using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Management;
using Keyrox.Shared;
using System.Drawing;

namespace System {
    public static class WinProcess {

        /// <summary>
        /// Gets the process owner.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <returns></returns>
        public static string GetProcessOwner(this Process process) {
            int processId = process.Id;
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            var searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            foreach (ManagementObject obj in processList) {
                string[] argList = new string[] { string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                    return argList[0];
            }
            return "Unknow";
        }

        /// <summary>
        /// Determines whether this instance [can read main module] of the specified process.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <returns>
        /// 	<c>true</c> if this instance [can read main module] the specified process; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanReadMainModule(this Process process) {
            try {
                var module = process.MainModule.BaseAddress;
                return true;
            }
            catch (Exception) { return false; }
        }

        /// <summary>
        /// Gets the process icon.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <returns></returns>
        public static Image GetProcessIcon(this Process process) {
            if (process.CanReadMainModule()) {
                var path = process.MainModule.FileName.Replace("\\??\\", "");
                return WindowsAPI.GetFileIcon(path, WindowsAPI.IconSize.Small).ToBitmap();
            }
            return null;
        }

        /// <summary>
        /// Gets the process icon.
        /// </summary>
        /// <param name="processId">The process id.</param>
        /// <returns></returns>
        public static Image GetProcessIcon(int processId) {
            return GetProcessIcon(GetProcessByPID(processId));
        }

        /// <summary>
        /// Gets the process by PID.
        /// </summary>
        /// <param name="processId">The process id.</param>
        /// <returns></returns>
        public static Process GetProcessByPID(int processId) {
            var procs = Process.GetProcesses();
            foreach (var proc in procs) {
                if (proc.Id == processId) return proc;
            }
            return null;
        }
    }
}
