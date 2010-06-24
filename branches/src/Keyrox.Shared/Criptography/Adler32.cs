using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keyrox.Shared.Criptography {
    public sealed class Adler32 {

        /// <summary>
        /// The base adler to checksum calculation.
        /// </summary>
        private const uint AdlerBase = 0xFFF1;

        /// <summary>
        /// The start adler to checksum calculation.
        /// </summary>
        private const uint AdlerStart = 0x0001;

        /// <summary>
        /// The buff adler to checksum calculation.
        /// </summary>
        private const uint AdlerBuff = 0x0400;

        /// <summary>
        /// Gets the checksum.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        public static uint GetChecksum(byte[] buffer) {

            byte[] data = new byte[buffer.Length - 6];
            Array.Copy(buffer, 6, data, 0, data.Length);

            int len = data.GetLength(0);
            if (len == 0) return 0;

            uint unSum1 = AdlerStart & 0xFFFF;
            uint unSum2 = (AdlerStart >> 16) & 0xFFFF;

            for (int i = 0; i < len; i++) {
                unSum1 = (unSum1 + data[i]) % AdlerBase;
                unSum2 = (unSum1 + unSum2) % AdlerBase;
            }
            return (unSum2 << 16) + unSum1;
        }

    }
}
