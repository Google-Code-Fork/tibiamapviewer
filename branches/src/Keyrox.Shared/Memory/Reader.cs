using System;
using Keyrox.Shared;

namespace Keyrox.Shared.Memory {
	/// <summary>
	/// Read Process Memory.
	/// </summary>
	public class Reader {

        /// <summary>
        /// Initializes a new instance of the <see cref="Reader"/> class.
        /// </summary>
        /// <param name="hProcess">The h process.</param>
		public Reader(IntPtr processHandle) {
            if (processHandle != IntPtr.Zero) { ProcessHandle = processHandle; }
            else { throw new ArgumentException("Invalid Process Handle"); }
		}

		/// <summary>
		/// Internal Client Handle Process.
		/// </summary>
		public IntPtr ProcessHandle;

		/// <summary>
		/// Read a String from Memory.
		/// </summary>
		/// <param name="memoryAddress">Adress.</param>
		/// <returns></returns>
		public string String(IntPtr memoryAddress) {
			byte[] Buffer;
			Byte((uint)memoryAddress, 100, out Buffer);

			string sBuffer = "";
			for (int i = 0; i < Buffer.Length; i++) {
				if (Convert.ToChar(Buffer[i]).ToString() != "\0") { sBuffer += Convert.ToChar(Buffer[i]).ToString(); }
				else { break; }
			}
			return sBuffer;
		}

		/// <summary>
		/// Read a String from Memory.
		/// </summary>
		/// <param name="memoryAddress">Adress.</param>
		/// <returns></returns>
		public string String(uint memoryAddress) {
			byte[] Buffer;
			Byte(memoryAddress, 100, out Buffer);

			string sBuffer = "";
			for (int i = 0; i < Buffer.Length; i++) {
				if (Convert.ToChar(Buffer[i]).ToString() != "\0") { sBuffer += Convert.ToChar(Buffer[i]).ToString(); }
				else { break; }
			}
			return sBuffer;
		}

		/// <summary>
		/// Read Bytes from Memory.
		/// </summary>
		/// <param name="memoryAddress">Adress.</param>
		/// <param name="bytesToRead">Bytes Lenght.</param>
		/// <param name="buffer">Return Byte.</param>
		/// <returns></returns>
		public void Byte(uint memoryAddress, uint bytesToRead, out byte[] buffer) {
			buffer = new byte[bytesToRead];
			IntPtr ptrBytesRead;
			WindowsAPI.ReadProcessMemory(ProcessHandle, (IntPtr)memoryAddress, buffer, bytesToRead, out ptrBytesRead);
		}

		/// <summary>
		/// Read Long Integers from Memory.
		/// </summary>
		/// <param name="memoryAddress">Adress.</param>
		/// <param name="bytesToRead">Bytes Lenght.</param>
		/// <param name="buffer">Return Uint.</param>
		/// <returns></returns>
		public void Uint(uint memoryAddress, uint bytesToRead, out uint buffer) {
			uint[] nbuffer = new uint[1];
			IntPtr ptrBytesRead;
			WindowsAPI.ReadProcessMemory(ProcessHandle, (IntPtr)memoryAddress, nbuffer, bytesToRead, out ptrBytesRead);
			buffer = nbuffer[0];
		}

        /// <summary>
        /// Read Long Integers from Memory.
        /// </summary>
        /// <param name="memoryAddress">Adress.</param>
        /// <param name="bytesToRead">Bytes Lenght.</param>
        /// <param name="buffer">Return Uint.</param>
        /// <returns></returns>
        public uint Uint(uint memoryAddress) {
            uint[] nbuffer = new uint[1];
            IntPtr ptrBytesRead;
            WindowsAPI.ReadProcessMemory(ProcessHandle, (IntPtr)memoryAddress, nbuffer, 4, out ptrBytesRead);
            return nbuffer[0];
        }

		/// <summary>
		/// Read Short Integers from Memory.
		/// </summary>
		/// <param name="memoryAddress">Adress.</param>
		/// <param name="bytesToRead">Bytes Lenght.</param>
		/// <param name="buffer">Return Ushort.</param>
		/// <returns></returns>
		public void UShort(IntPtr memoryAddress, uint bytesToRead, out ushort buffer) {
			ushort[] nbuffer = new ushort[1];
			IntPtr ptrBytesRead;
			WindowsAPI.ReadProcessMemory(ProcessHandle, memoryAddress, nbuffer, bytesToRead, out ptrBytesRead);
			buffer = nbuffer[0];
		}

		/// <summary>
		/// Read Short Integers from Memory.
		/// </summary>
		/// <param name="memoryAddress">Adress.</param>
		/// <param name="bytesToRead">Bytes Lenght.</param>
		/// <param name="buffer">Return Ushort.</param>
		/// <returns></returns>
		public void UShort(uint memoryAddress, uint bytesToRead, out ushort buffer) {
			ushort[] nbuffer = new ushort[1];
			IntPtr ptrBytesRead;
			WindowsAPI.ReadProcessMemory(ProcessHandle, (IntPtr)memoryAddress, nbuffer, bytesToRead, out ptrBytesRead);
			buffer = nbuffer[0];
		}

		/// <summary>
		/// Read Short Integers from Memory.
		/// </summary>
		/// <param name="memoryAddress">Adress.</param>
		/// <param name="bytesToRead">Bytes Lenght.</param>
		/// <param name="buffer">Return Ushort.</param>
		/// <returns></returns>
		public void Int(uint memoryAddress, uint bytesToRead, out int buffer) {
			uint[] nbuffer = new uint[1];
			IntPtr ptrBytesRead;
			WindowsAPI.ReadProcessMemory(ProcessHandle, (IntPtr)memoryAddress, nbuffer, bytesToRead, out ptrBytesRead);
			buffer = Convert.ToInt32(nbuffer[0]);
		}
	}
}
