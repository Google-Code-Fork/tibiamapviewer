using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using Keyrox.Shared.Objects;

namespace System {
    public static class Bytes {

        /// <summary>
        /// Toes the uint array.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static uint[] ToUintArray(this byte[] bytes) {
            uint[] uints = new uint[bytes.Length / 4];

            for (int i = 0; i < uints.Length; i++) {
                uints[i] = BitConverter.ToUInt32(bytes, i * 4);
            }

            return uints;
        }

        /// <summary>
        /// Toes the uint array.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="preserveLenght">if set to <c>true</c> [preserve lenght].</param>
        /// <returns></returns>
        public static uint[] ToUintArray(this byte[] bytes, bool preserveLenght) {
            if (bytes.Length % 4 > 0)
                throw new Exception();

            uint[] temp = new uint[bytes.Length / 4];

            for (int i = 0; i < temp.Length; i++)
                temp[i] = BitConverter.ToUInt32(bytes, i * 4);

            return temp;
        }

        /// <summary>
        /// Toes the byte array.
        /// </summary>
        /// <param name="uints">The uints.</param>
        /// <returns></returns>
        public static byte[] ToByteArray(this uint[] uints) {
            byte[] bytes = new byte[uints.Length * 4];

            for (int i = 0; i < uints.Length; i++) {
                Array.Copy(BitConverter.GetBytes(uints[i]), 0, bytes, i * 4, 4);
            }

            return bytes;
        }

        /// <summary>
        /// Converts a string to a byte array
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this string s) {
            List<byte> value = new List<byte>();
            foreach (char c in s.ToCharArray())
                value.Add(c.ToByte());
            return value.ToArray();
        }

        /// <summary>
        /// Converts a char to a byte
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte ToByte(this char value) {
            return (byte)value;
        }

        /// <summary>
        /// Conta a Quantidade de Bytes na String Especificada.
        /// </summary>
        /// <param name="valor">String Hexadecimal para Análise.</param>
        /// <returns></returns>
        public static int CountBytes(this string valor) {
            var Sep = new char[1]; Sep[0] = Convert.ToChar(" ");
            return valor.Split(Sep).Length;
        }

        /// <summary>
        /// Converte a string especificada em Bytes.
        /// </summary>
        /// <param name="valor">String para conversão.</param>
        /// <param name="hex">Define se a string é Hexadecimal.</param>
        /// <returns></returns>
        public static byte[] GetBytes(this string valor, bool hex) {
            if (hex) {
                if (valor.Length == 1) { valor = "0" + valor; }

                var nHex = Regex.Replace(valor.ToUpper(), "[^0-9A-F]", "");
                var nByte = new byte[nHex.Length / 2];

                for (var i = 0; i < nByte.Length; i++) {
                    var nLocation = nHex.Length - 2 - (i * 2);
                    nByte[i] = byte.Parse(nHex.Substring(nLocation, 2), NumberStyles.AllowHexSpecifier);
                }
                Array.Reverse(nByte);
                return nByte;
            }
            var cHex = valor.ToCharArray();
            var cByte = new byte[cHex.Length];

            for (var i = 0; i < cByte.Length; i++) { cByte[i] = Convert.ToByte(cHex[i]); }
            return cByte;
        }

        /// <summary>
        /// Pega o High Byte do Valor especificado.
        /// </summary>
        /// <returns></returns>
        public static byte HighByteOfLong(this uint address) {
            return Convert.ToByte(address / 256);
        }

        /// <summary>
        /// Pega o High Byte do Valor especificado.
        /// </summary>
        /// <returns></returns>
        public static byte HighByteOfLong(this int address) {
            return HighByteOfLong((uint)address);
        }

        /// <summary>
        /// Pega o Low Byte do Valor especificado.
        /// </summary>
        /// <returns></returns>
        public static byte LowByteOfLong(this uint address) {
            var h = Convert.ToByte(address / 256);
            var l = Convert.ToByte(address - (Convert.ToInt32(h) * 256)); // low byte
            return l;
        }

        /// <summary>
        /// Pega o Low Byte do Valor especificado.
        /// </summary>
        /// <returns></returns>
        public static byte LowByteOfLong(this int address) {
            return LowByteOfLong((uint)address);
        }

        /// <summary>
        /// Verifica se os Pacotes são Iguais.
        /// </summary>
        public static bool Match(this byte[] packet1, byte[] packet2) {
            if (packet1.Length == 3 && packet1[0] == 01 && packet1[1] == 00 && packet1[2] == 30) { return true; }

            if (packet2.Length == 3 && packet2[0] == 01 && packet2[1] == 00 && packet2[2] == 30) { return true; }

            if (packet1.Length == packet2.Length) {
                for (int i = 0; i < packet2.Length; i++) {
                    if (packet1[i] != packet2[i]) { return false; }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Matches the specified packet1.
        /// </summary>
        /// <param name="packet1">The packet1.</param>
        /// <param name="packet2">The packet2.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static bool Match(this byte[] packet1, byte[] packet2, int count) {
            if (count > packet1.Length || count > packet2.Length) return false;
            for (int i = 0; i < count; i++) {
                if (packet1[i] != packet2[i]) return false;
            }
            return true;
        }

        /// <summary>
        /// Redimensione the given array of bytes.
        /// </summary>
        /// <returns></returns>
        public static byte[] Redim(this byte[] data, int length) {
            var newData = new byte[length];
            var iterator = length;
            if (data.Length < iterator) iterator = data.Length;
            Array.Copy(data, newData, iterator);
            return newData;
        }

        /// <summary>
        /// Gets the data value lenght.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static int GetDataValueLenght(this byte[] data) {
            if (data.Length < 2) return 0;
            return Hex.GetTheLong(data[0], data[1]).ToInt32();
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static byte[] GetBytes(this uint obj) {
            return BitConverter.GetBytes(obj);
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static byte[] GetBytes(this int obj) {
            return BitConverter.GetBytes(obj);
        }

    }
}
