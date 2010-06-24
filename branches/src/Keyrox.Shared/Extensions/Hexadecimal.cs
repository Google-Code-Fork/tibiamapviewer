using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace System {
    public static class Hex {

        #region "[rgn] String Iteration  "
        /// <summary>
        /// Convert o Byte especificado em uma String Hexadecimal.
        /// </summary>
        /// <param name="bytes">Byte[] para conversão.</param>
        /// <param name="toHex">Define se a Conversão é pra Hexadecimal.</param>
        /// <returns></returns>
        public static string GetString(this byte[] bytes, bool toHex) {
            if (toHex) {
                var HexDigits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
                var Buffer = new StringBuilder();
                var length = bytes.Length;
                for (var i = 0; i < length; i++) {
                    Buffer.Append(HexDigits[bytes[i] >> 4]);
                    Buffer.Append(HexDigits[bytes[i] & 15]);
                    Buffer.Append(" ");
                }
                return Buffer.ToString().ToUpper();
            }
            var sNewString = System.Text.Encoding.GetEncoding(1251).GetString(bytes);
            //for (var n = 0; n < bytes.Length; n++) { sNewString += Convert.ToChar(bytes[n]).ToString(); }
            //return Regex.Replace(sNewString, "[^0-9A-Za-z]", ".");
            return Regex.Replace(sNewString, "[^0-9A-Za-z ?/|.,*!@#$%¨&=;:><-]", ".");
        }

        /// <summary>
        /// Convert o Byte especificado em uma String Hexadecimal.
        /// </summary>
        /// <param name="bytes">Byte[] para conversão.</param>
        /// <param name="zeroStop">Define se a Conversão deve parar no Byte "0".</param>
        /// <param name="noLength">Apenas para Diferenciar a Assintaura.</param>
        /// <returns></returns>
        public static string GetString(this byte[] bytes, bool zeroStop, uint noLength) {
            var sNewString = "";
            for (var n = 0; n < bytes.Length; n++) {
                if (zeroStop && (bytes[n] == Convert.ToByte(0))) { break; }
                sNewString += Convert.ToChar(bytes[n]).ToString();
            }
            return Regex.Replace(sNewString, "[^0-9A-Za-z ?/|.,*!@#$%¨&=;:><-]", ".");
        }

        /// <summary>
        /// Convert o Byte especificado em uma String Hexadecimal.
        /// </summary>
        /// <param name="bytes">Byte[] para conversão.</param>
        /// <param name="toHex">Define se a Conversão é pra Hexadecimal.</param>
        /// <param name="getLenght">Define se será lido apenas o Tamanho do Pacote.</param>
        /// <returns></returns>
        public static string GetString(this byte[] bytes, bool toHex, bool getLenght) {
            if (toHex) {
                var HexDigits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
                var Buffer = new StringBuilder();
                var length = Convert.ToInt32(bytes[0] + bytes[1] * 256 + 2);

                for (var i = 0; i < length; i++) {
                    Buffer.Append(HexDigits[bytes[i] >> 4]);
                    Buffer.Append(HexDigits[bytes[i] & 15]);
                    Buffer.Append(" ");
                }
                return Buffer.ToString().ToUpper();
            }
            var NewString = new StringBuilder();
            for (int n = 0; n < bytes.Length; n++) { NewString.Append(Convert.ToChar(bytes[n])); }
            var nChars = NewString.ToString().ToCharArray();
            return new string(nChars);
        }

        /// <summary>
        /// Convert o Byte especificado em uma String.
        /// </summary>
        /// <param name="bytes">Byte[] para conversão.</param>
        /// <param name="index">Byte de início da conversão.</param>
        /// <param name="size">Quantidade de bytes para conversão.</param>
        /// <returns></returns>
        public static string GetString(this byte[] bytes, uint index, uint size) {
            var NewString = new StringBuilder();
            for (var n = 0; n < size; n++) { NewString.Append(Convert.ToChar(bytes[index + n])); }
            var nChars = NewString.ToString().ToCharArray();
            return new string(nChars);
        }

        /// <summary>
        /// Convert o Byte especificado em uma String.
        /// </summary>
        /// <param name="bytes">Byte[] para conversão.</param>
        /// <param name="index">Byte de início da conversão.</param>
        /// <param name="size">Quantidade de bytes para conversão.</param>
        /// <returns></returns>
        public static string GetString(this byte[] bytes, int index, int size) {
            return GetString(bytes, (uint)index, (uint)size);
        }

        /// <summary>
        /// Convert o Byte especificado em uma String.
        /// </summary>
        /// <param name="bytes">Byte[] para conversão.</param>
        /// <param name="index">Byte de início da conversão.</param>
        /// <param name="size">Quantidade de bytes para conversão.</param>
        /// <returns></returns>
        public static string GetString(this byte[] bytes, uint index, int size) {
            return GetString(bytes, index, (uint)size);
        }

        /// <summary>
        /// Convert o Byte especificado em uma String.
        /// </summary>
        /// <param name="bytes">Byte[] para conversão.</param>
        /// <param name="index">Byte de início da conversão.</param>
        /// <param name="size">Quantidade de bytes para conversão.</param>
        /// <returns></returns>
        public static string GetString(this byte[] bytes, int index, uint size) {
            return GetString(bytes, (uint)index, size);
        }

        /// <summary>
        /// Pega a representação decimal em string do Byte especificado.
        /// </summary>
        /// <param name="bytes">Byte para Análise.</param>
        /// <param name="size">Quantidade de Bytes para Conversão.</param>
        /// <returns></returns>
        public static string GetUintString(this byte[] bytes, int size) {
            var sBytes = "";
            for (var i = 0; i < size; i++) { sBytes += bytes[i] + " "; }
            return sBytes;
        }

        /// <summary>
        /// Apenas Corrige bytes de 1 Lenght com o "0" no Início.
        /// </summary>
        /// <param name="text">String Hexadecimal.</param>
        /// <returns></returns>
        public static string GoodByte(this string text) {
            if (text.Length == 1) { return "0" + text.ToUpper(); }
            return text.ToUpper();
        }

        /// <summary>
        /// Return the Hexadecimal value from a Decimal Eight Basead.
        /// </summary>
        public static string GetDecHex(this uint value) {
            if (value.ToString().Length > 10) { return "Invalid Decimal Value!"; }

            var nHex = value.ToString("x").ToUpper();
            var nLeng = 8 - nHex.Length; var zString = "";

            if (nLeng > 0) { for (var i = 0; i < nLeng; i++) { zString += "0"; } }
            nHex = zString + nHex;
            return nHex;
        }

        /// <summary>
        /// Return de Hexa Chain from a Hexadecimal Eight Basead.
        /// </summary>
        /// <param name="decHex"></param>
        /// <returns></returns>
        public static string GetHexaChain(this string decHex) {
            var nLeng = 8 - decHex.Length; var zString = "";
            if (nLeng > 0) { for (int i = 0; i < nLeng; i++) { zString += "0"; } }
            var nHexC = zString + decHex;

            var nHexR = "";
            for (var h = 0; h < 8; h += 2) { nHexR += nHexC.Substring(8 - (h + 2), 2) + " "; }
            return nHexR;
        }
        #endregion

        #region "[rgn] Integer Iteration "
        /// <summary>
        /// Pega o Long dos dois Bytes especificados.
        /// </summary>
        /// <returns></returns>
        public static uint GetTheLong(byte byte1, byte byte2) {
            return Convert.ToUInt32(byte2) * 256 + Convert.ToUInt32(byte1);
        }

        /// <summary>
        /// Pega o Long dos dois Bytes especificados.
        /// </summary>
        /// <param name="value">Two bytes long.</param>
        /// <returns></returns>
        public static uint GetTheLong(this byte[] value) {
            if (value.Length != 2) { throw new ArgumentException("Value must contain 2 Bytes!"); }
            return GetTheLong(value[0], value[1]);
        }

        /// <summary>
        /// Transfere os dados do Byte[] e Retorna um Uint[].
        /// </summary>
        /// <param name="data">Byte para conversão.</param>
        /// <param name="size">Quantidade de bytes para trasnferência.</param>
        /// <returns></returns>
        public static uint[] GetUint(this byte[] data, int size) {
            var nUint = new uint[size];
            for (var i = 0; i < size; i++) { nUint[i] = data[i]; }
            return nUint;
        }

        /// <summary>
        /// Return a Decimal Value From a Decimal Hex String.
        /// </summary>
        public static uint GetUintFromHexString(this string decHex) {
            return Convert.ToUInt32(decHex, 16);
        }
        #endregion

    }
}
