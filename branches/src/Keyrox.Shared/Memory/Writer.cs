using System;
using Keyrox.Shared;

namespace Keyrox.Shared.Memory {
    /// <summary>
    /// Write Process Memory.
    /// </summary>
    public class Writer {

        /// <summary>
        /// Initializes a new instance of the <see cref="Writer"/> class.
        /// </summary>
        /// <param name="hProcess">The h process.</param>
        public Writer(IntPtr processHandle) {
            if (processHandle != IntPtr.Zero) { ProcessHandle = processHandle; }
            else { throw new ArgumentException("Invalid Process Handle"); }
        }

        /// <summary>
        /// Internal Client Handle Process.
        /// </summary>
        protected IntPtr ProcessHandle;

        /// <summary>
        /// Write in Tibia Client Process Memory.
        /// </summary>
        /// <param name="memoryAddress">Adress.</param>
        /// <param name="value">Value To Write.</param>
        public void Bytes(IntPtr memoryAddress, byte[] value) {
            IntPtr ptrBytesWritten;
            WindowsAPI.WriteProcessMemory(ProcessHandle, memoryAddress, value, (uint)value.Length, out ptrBytesWritten);
        }

        /// <summary>
        /// Write in Tibia Client Process Memory.
        /// </summary>
        /// <param name="memoryAddress">Adress.</param>
        /// <param name="value">Value To Write.</param>
        public void Bytes(uint memoryAddress, byte[] value) {
            IntPtr ptrBytesWritten;
            WindowsAPI.WriteProcessMemory(ProcessHandle, (IntPtr)memoryAddress, value, (uint)value.Length, out ptrBytesWritten);
        }

        /// <summary>
        /// Write in Tibia Client Process Memory.
        /// </summary>
        /// <param name="memoryAddress">Adress.</param>
        /// <param name="value">Value To Write.</param>
        public void Uints(IntPtr memoryAddress, uint[] value) {
            IntPtr ptrBytesWritten;
            WindowsAPI.WriteProcessMemory(ProcessHandle, memoryAddress, value, (uint)value.Length, out ptrBytesWritten);
        }

        /// <summary>
        /// Write a string to memory without using econding.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="address"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public void StringNoEncoding(IntPtr memoryAddress, string value) {
            var str = value;
            str += '\0';
            byte[] bytes = str.ToByteArray();
            IntPtr ptrBytesWritten;
            WindowsAPI.WriteProcessMemory(ProcessHandle, (IntPtr)memoryAddress, bytes, (uint)bytes.Length, out ptrBytesWritten);

        }

        /// <summary>
        /// Write in Tibia Client Process Memory.
        /// </summary>
        /// <param name="memoryAddress">Adress.</param>
        /// <param name="value">Value To Write.</param>
        public void Uints(uint memoryAddress, uint[] value) {
            IntPtr ptrBytesWritten;
            WindowsAPI.WriteProcessMemory(ProcessHandle, (IntPtr)memoryAddress, value, (uint)value.Length, out ptrBytesWritten);
        }

        /// <summary>
        /// Write in Tibia Client Process Memory.
        /// </summary>
        /// <param name="memoryAddress">Adress.</param>
        /// <param name="value">Value To Write.</param>
        /// <param name="size">Size To Write.</param>
        public void Uint(IntPtr memoryAddress, uint value, int size) {
            uint[] nValor = { value };
            IntPtr ptrBytesWritten;
            WindowsAPI.WriteProcessMemory(ProcessHandle, memoryAddress, nValor, Convert.ToUInt32(size), out ptrBytesWritten);
        }

        /// <summary>
        /// Write in Tibia Client Process Memory.
        /// </summary>
        /// <param name="memoryAddress">Adress.</param>
        /// <param name="value">Value To Write.</param>
        /// <param name="size">Size To Write.</param>
        public void Uint(uint memoryAddress, uint value, int size) {
            uint[] nValor = { value };
            IntPtr ptrBytesWritten;
            WindowsAPI.WriteProcessMemory(ProcessHandle, (IntPtr)memoryAddress, nValor, Convert.ToUInt32(size), out ptrBytesWritten);
        }

        /// <summary>
        /// Write in Tibia Client Process Memory.
        /// </summary>
        /// <param name="memoryAddress">Adress.</param>
        /// <param name="value">Value To Write.</param>
        /// <param name="size">Size To Write.</param>
        public void Uint(uint memoryAddress, uint value) {
            uint[] nValor = { value };
            IntPtr ptrBytesWritten;
            WindowsAPI.WriteProcessMemory(ProcessHandle, (IntPtr)memoryAddress, nValor, 4, out ptrBytesWritten);
        }

        /// <summary>
        /// Write in Tibia Client Process Memory.
        /// </summary>
        /// <param name="memoryAddress">Adress.</param>
        /// <param name="value">Value To Write.</param>
        public void String(IntPtr memoryAddress, string value) {
            byte[] nBuffer = new byte[value.Length];
            for (int i = 0; i < value.Length; i++) { nBuffer[i] = Convert.ToByte(value[i]); }
            Bytes(memoryAddress, nBuffer);
        }

        /// <summary>
        /// Write in Tibia Client Process Memory.
        /// </summary>
        /// <param name="memoryAddress">Adress.</param>
        /// <param name="value">Value To Write.</param>
        /// <param name="addFinalZero">Add Final Zero.</param>
        public void String(IntPtr memoryAddress, string value, bool addFinalZero) {
            byte[] nBuffer = new byte[value.Length];
            if (addFinalZero) { nBuffer = new byte[value.Length + 1]; }

            for (int i = 0; i < value.Length; i++) { nBuffer[i] = Convert.ToByte(value[i]); }
            Bytes(memoryAddress, nBuffer);
        }

        /// <summary>
        /// Write in Tibia Client Process Memory.
        /// </summary>
        /// <param name="memoryAddress">Adress.</param>
        /// <param name="value">Value To Write.</param>
        public void String(uint memoryAddress, string value) {
            byte[] nBuffer = new byte[value.Length];
            for (int i = 0; i < value.Length; i++) { nBuffer[i] = Convert.ToByte(value[i]); }
            Bytes(memoryAddress, nBuffer);
        }

        /// <summary>
        /// Write in Tibia Client Process Memory.
        /// </summary>
        /// <param name="memoryAddress">Adress.</param>
        /// <param name="value">Value To Write.</param>
        /// <param name="addFinalZero">Add Final Zero.</param>
        public void String(uint memoryAddress, string value, bool addFinalZero) {
            byte[] nBuffer = new byte[value.Length];
            if (addFinalZero) { nBuffer = new byte[value.Length + 1]; }

            for (int i = 0; i < value.Length; i++) { nBuffer[i] = Convert.ToByte(value[i]); }
            Bytes(memoryAddress, nBuffer);
        }
    }
}
