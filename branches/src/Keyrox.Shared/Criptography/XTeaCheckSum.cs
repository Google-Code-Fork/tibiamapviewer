using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keyrox.Shared.Criptography.Contracts;
using Keyrox.Shared.Objects;

namespace Keyrox.Shared.Criptography {
    public class XTeaCheckSum : ICriptoProvider {

        public byte[] Key { get; set; }

        public byte[] Encode(byte[] buffer, byte[] key) {
            if (key == null || key.Length != 16) throw new ArgumentException("XTea Key was not properly initialized!");

            int msgSize = buffer.Length;
            int pad = msgSize % 8;

            if (pad > 0)
                msgSize += (8 - pad);

            byte[] originalMsg = new byte[msgSize];
            Array.Copy(buffer, 0, originalMsg, 0, buffer.Length);

            uint[] msgUInt = originalMsg.ToUintArray(true);

            for (int i = 0; i < msgUInt.Length; i += 2)
                InternalEncode(ref msgUInt, i, key.ToUintArray(true));

            int buflen = msgUInt.ToByteArray().Length;
            byte[] encryptMsg = new byte[buflen + 6];
            Array.Copy(msgUInt.ToByteArray(), 0, encryptMsg, 6, buflen);

            AddCheckSum(ref encryptMsg);
            AddHeader(ref encryptMsg);
            return encryptMsg;
        }

        public byte[] Encode(byte[] buffer) {
            return Encode(buffer, Key);
        }

        public byte[] Decode(byte[] buffer, byte[] key) {

            if (buffer.Length <= 6) { throw new ArgumentException("Invalid encripted packet length"); }
            if ((buffer.Length - 6) % 8 > 0) { throw new ArgumentException("Invalid encripted packet length"); }

            byte[] encryptMsg = new byte[buffer.Length - 6];
            Array.Copy(buffer, 6, encryptMsg, 0, buffer.Length - 6);

            uint[] encryptUInt = encryptMsg.ToUintArray();

            for (int i = 0; i < encryptUInt.Length; i += 2) {
                InternalDecode(ref encryptUInt, i, key.ToUintArray());
            }

            int buflen = encryptUInt.ToByteArray().Length;
            byte[] decrpytMsg = new byte[buflen];
            Array.Copy(encryptUInt.ToByteArray(), 0, decrpytMsg, 0, buflen);

            FixBufferSize(ref decrpytMsg);
            return decrpytMsg;
        }

        public byte[] Decode(byte[] buffer) {
            return Decode(buffer, Key);
        }

        private void AddCheckSum(ref byte[] buffer) {
            var checkSum = Adler32.GetChecksum(buffer).GetBytes();
            Array.Copy(checkSum, 0, buffer, 2, 4);
        }

        private void AddHeader(ref byte[] buffer) {
            var len = (buffer.Length - 2).GetBytes();
            Array.Copy(len, 0, buffer, 0, 2);
        }

        private void FixBufferSize(ref byte[] buffer) {
            if (buffer.Length < 3) { return; }
            var len = new byte[] { buffer[0], buffer[1] }.GetTheLong();
            var newByte = new byte[len.ToInt32() + 2];

            if (buffer.Length < newByte.Length) { return; }
            Array.Copy(buffer, 0, newByte, 0, newByte.Length);
            buffer = newByte;
        }

        private void InternalEncode(ref uint[] v, int index, uint[] XteaKey) {
            uint y = v[index];
            uint z = v[index + 1];
            uint sum = 0;
            uint delta = 0x9e3779b9;
            uint n = 32;

            while (n-- > 0) {
                y += (z << 4 ^ z >> 5) + z ^ sum + XteaKey[sum & 3];
                sum += delta;
                z += (y << 4 ^ y >> 5) + y ^ sum + XteaKey[sum >> 11 & 3];
            }

            v[index] = y;
            v[index + 1] = z;
        }

        private void InternalDecode(ref uint[] v, int index, uint[] XteaKey) {
            uint n = 32;
            uint sum;
            uint y = v[index];
            uint z = v[index + 1];
            uint delta = 0x9e3779b9;

            sum = delta << 5;

            while (n-- > 0) {
                z -= (y << 4 ^ y >> 5) + y ^ sum + XteaKey[sum >> 11 & 3];
                sum -= delta;
                y -= (z << 4 ^ z >> 5) + z ^ sum + XteaKey[sum & 3];
            }

            v[index] = y;
            v[index + 1] = z;

        }

    }
}
