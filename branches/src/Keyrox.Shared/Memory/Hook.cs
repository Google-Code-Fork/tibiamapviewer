using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Keyrox.Shared.Extensions {
    public static class Hook {

        /// <summary>
        /// Injects the DLL.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public static bool InjectDLL(string filename) {

            //if (!File.Exists(filename)) return false;
            //IntPtr remoteAddress = Keyrox.Shared.WindowsAPI.VirtualAllocEx(Memory.HandleProcess, IntPtr.Zero, (uint)filename.Length, Keyrox.Shared.WindowsAPI.MEM_COMMIT | Keyrox.Shared.WindowsAPI.MEM_RESERVE, Keyrox.Shared.WindowsAPI.PAGE_READWRITE);
            //Memory.Writer.String(remoteAddress, filename); ;

            //IntPtr thread = Keyrox.Shared.WindowsAPI.CreateRemoteThread(Memory.HandleProcess, IntPtr.Zero, 0, Keyrox.Shared.WindowsAPI.GetProcAddress(Keyrox.Shared.WindowsAPI.GetModuleHandle("Kernel32"), "LoadLibraryA"), remoteAddress, 0, IntPtr.Zero);
            //Keyrox.Shared.WindowsAPI.VirtualFreeEx(Memory.HandleProcess, remoteAddress, (uint)filename.Length, Keyrox.Shared.WindowsAPI.MEM_RELEASE);
            //return thread.ToInt32() > 0 && remoteAddress.ToInt32() > 0;
            return false;
        }

    }
}
