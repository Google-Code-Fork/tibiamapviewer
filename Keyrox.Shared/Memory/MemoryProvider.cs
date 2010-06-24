using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Keyrox.Shared.Memory {
    public class MemoryProvider {

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryProvider"/> class.
        /// </summary>
        /// <param name="process">The process.</param>
        public MemoryProvider(Process process) {
            this.Process = process;
            Writer = new Writer(ProcessHandle);
            Reader = new Reader(ProcessHandle);
        }

        #region "[rgn] Private Variables "
        private IntPtr hprocess;
        #endregion

        #region "[rgn] Public Properties "
        public Writer Writer { get; private set; }
        public Reader Reader { get; private set; }
        public Process Process { get; private set; }
        public IntPtr ProcessHandle {
            get {
                if (hprocess == IntPtr.Zero) {
                    OpenProcess();
                    return hprocess;
                }
                else { return hprocess; }
            }
        }
        #endregion

        #region "[rgn] Userfull Methods  "
        /// <summary>
        /// Define the Active Window.
        /// </summary>
        public void SetActiveWindow() {
            IntPtr hWnd = Process.MainWindowHandle;
            WindowsAPI.SetActiveWindow(hWnd);
            WindowsAPI.SwitchToThisWindow(hWnd, true);
        }

        /// <summary>
        /// Change the Window Title Text of the Client.
        /// </summary>
        /// <param name="text">New Window Title.</param>
        public void SetWindowText(string text) {
            WindowsAPI.SetWindowText(Process.MainWindowHandle, text);
        }
        #endregion
        
        /// <summary>
        /// Injects the DLL.
        /// </summary>
        /// <param name="dllPath">The DLL path.</param>
        /// <returns></returns>
        public bool InjectDLL(string dllPath) {

            if (!File.Exists(dllPath)) return false;
            IntPtr remoteAddress = Keyrox.Shared.WindowsAPI.VirtualAllocEx(Process.Handle, IntPtr.Zero, (uint)dllPath.Length, Keyrox.Shared.WindowsAPI.MEM_COMMIT | Keyrox.Shared.WindowsAPI.MEM_RESERVE, Keyrox.Shared.WindowsAPI.PAGE_READWRITE);
            Writer.StringNoEncoding(remoteAddress, dllPath); ;

            IntPtr thread = Keyrox.Shared.WindowsAPI.CreateRemoteThread(Process.Handle, IntPtr.Zero, 0, Keyrox.Shared.WindowsAPI.GetProcAddress(Keyrox.Shared.WindowsAPI.GetModuleHandle("Kernel32"), "LoadLibraryA"), remoteAddress, 0, IntPtr.Zero);
            Keyrox.Shared.WindowsAPI.VirtualFreeEx(Process.Handle, remoteAddress, (uint)dllPath.Length, Keyrox.Shared.WindowsAPI.MEM_RELEASE);
            return thread.ToInt32() > 0 && remoteAddress.ToInt32() > 0;
        }

        /// <summary>
        /// Open the Process and update the Handle Process.
        /// </summary>
        public void OpenProcess() {
            if (!Process.HasExited) {
                var access = WindowsAPI.ProcessAccessRights.PROCESS_VM_READ | WindowsAPI.ProcessAccessRights.PROCESS_VM_WRITE | WindowsAPI.ProcessAccessRights.PROCESS_VM_OPERATION;
                hprocess = WindowsAPI.OpenProcess((uint)access, 0, (uint)Process.Id);
                if (hprocess.ToInt32() == 0) { throw new Exception("OpenProcess Failed!"); }
            }
            else { Process = null; hprocess = IntPtr.Zero; }
        }

        /// <summary>
        /// Close the Opened Handle of Process from Client.
        /// </summary>
        public void CloseHandle() {
            if (hprocess == IntPtr.Zero) { return; }
            int returnVal = WindowsAPI.CloseHandle(hprocess);
            if (returnVal == 0) { throw new Exception("CloseHandle Failed!"); }
        }
    }
}
