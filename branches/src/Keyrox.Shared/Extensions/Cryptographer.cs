using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Keyrox.Shared {
	/// <summary>
	/// Summary description for Encrypter
	/// </summary>
	public class Cryptographer {
        
        /// <summary>
        /// Serialization Default Path.
        /// </summary>
        public static string SerializationPath { get { return System.IO.Path.Combine(Environment.CurrentDirectory, "Data"); } }

        /// <summary>
        /// Serialization Default Encryption Key.
        /// </summary>
        public static string SerializationKey { get { return "($*)@-$*@@$-*@@$*-@@(*$)Nüó¤^Ã$@@è3óÿÿƒÀZ‰ÀSV‹Ø‹Ãè•ùÿÿ‹Mð‹UüƒÂ1‹Eøè´þÿÿ‹Uô‹Æè6ßÿÿEôPUðƒæÿ($*)@-$*@@$-*@@$*-@@(*$)"; } }

        /// <summary>
        /// Encrypts the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
		public static string Encrypt(string text) {
			return Encrypt(text, SerializationKey);
		}
        /// <summary>
        /// Encrypts the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
		public static string Encrypt(string text, string key) {
			return ConvertByteArrayToWriteableString(InnerEncrypt(text, key));
		}
        /// <summary>
        /// Decrypts the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
		public static string Decrypt(string text) {
			return Decrypt(text, SerializationKey);
		}
        /// <summary>
        /// Decrypts the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
		public static string Decrypt(string text, string key) {
			return InnerDecrypt(ConvertWriteableStringToByteArray(text), key);
		}


        /// <summary>
        /// Inners the encrypt.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="primer">The primer.</param>
        /// <returns></returns>
		private static byte[] InnerEncrypt(string data, string primer) {
			// From clear text to encrypted
			MemoryStream memory = new MemoryStream();
			TripleDESCryptoServiceProvider cryptoProvider = new TripleDESCryptoServiceProvider();
			byte[] byteArray = ConvertStringToByteArray(data);

			ICryptoTransform cryptoTransform = cryptoProvider.CreateEncryptor(Get24ByteKey(primer), Get8ByteInitializationVector(primer));
			CryptoStream cryptoStream = new CryptoStream(memory, cryptoTransform, CryptoStreamMode.Write);
			cryptoStream.Write(byteArray, 0, byteArray.Length);
			cryptoStream.FlushFinalBlock();
			byte[] retVal = memory.ToArray();
			cryptoStream.Close();
			memory.Close();
			return retVal;
		}

        /// <summary>
        /// Inners the decrypt.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="primer">The primer.</param>
        /// <returns></returns>
        private static string InnerDecrypt(byte[] data, string primer) {
			// From encrypted to clear text
			MemoryStream memory = new MemoryStream();
			TripleDESCryptoServiceProvider cryptoProvider = new TripleDESCryptoServiceProvider();
			ICryptoTransform cryptoTransform = cryptoProvider.CreateDecryptor(Get24ByteKey(primer),
				Get8ByteInitializationVector(primer));
			CryptoStream cryptoStream = new CryptoStream(new MemoryStream(data), cryptoTransform, CryptoStreamMode.Read);
			byte[] decrypedValue = new byte[data.Length];
			cryptoStream.Read(decrypedValue, 0, data.Length);
			cryptoStream.Close();
			string sAux = ConvertByteArrayToString(decrypedValue);
			memory.Close();
			return sAux;
		}
        /// <summary>
        /// Converts the string to byte array.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
		private static byte[] ConvertStringToByteArray(string s) {
			return (new UnicodeEncoding()).GetBytes(s);
		}
        /// <summary>
        /// Converts the writeable string to byte array.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
		private static byte[] ConvertWriteableStringToByteArray(string s) {
			byte[] bAux = (new ASCIIEncoding()).GetBytes(s);
			return ConvertFromASCIIFriendly(bAux);
		}
        /// <summary>
        /// Converts the byte array to string.
        /// </summary>
        /// <param name="bu">The bu.</param>
        /// <returns></returns>
		private static string ConvertByteArrayToString(byte[] bu) {
			string sAux = "";
			byte[] ba = ASCIIEncoding.Convert(new UnicodeEncoding(), new ASCIIEncoding(), bu);
			foreach (byte b in ba) {
				int iUnicodeChar = b;
				if (b > 0) sAux += (char)iUnicodeChar;
			}
			return sAux;
		}
        /// <summary>
        /// Converts to ASCII friendly.
        /// </summary>
        /// <param name="ba">The ba.</param>
        /// <returns></returns>
		private static byte[] ConvertToASCIIFriendly(byte[] ba) {
			byte[] bAux = new byte[ba.GetLength(0) * 2];
			for (int i = 0; i < ba.GetLength(0); i++) {
				byte b = (byte)(ba[i] / 16);
				bAux[2 * i] = (byte)('A' + b);
				bAux[2 * i + 1] = (byte)('A' + ba[i] - b * 16);
			}
			return bAux;
		}
        /// <summary>
        /// Converts from ASCII friendly.
        /// </summary>
        /// <param name="ba">The ba.</param>
        /// <returns></returns>
		private static byte[] ConvertFromASCIIFriendly(byte[] ba) {
			byte[] bAux = new byte[ba.GetLength(0) / 2];
			for (int i = 0; i < ba.GetLength(0); i += 2)
				bAux[i / 2] = (byte)((ba[i] - 'A') * 16 + (ba[i + 1] - 'A'));
			return bAux;
		}
        /// <summary>
        /// Converts the byte array to writeable string.
        /// </summary>
        /// <param name="ba">The ba.</param>
        /// <returns></returns>
		private static string ConvertByteArrayToWriteableString(byte[] ba) {
			byte[] bAux = ConvertToASCIIFriendly(ba);
			return (new ASCIIEncoding()).GetString(bAux);
		}
        /// <summary>
        /// Get24s the byte key.
        /// </summary>
        /// <param name="primer">The primer.</param>
        /// <returns></returns>
		private static byte[] Get24ByteKey(string primer) {
			// Must return a 24 byte array
			SHA256 sh = new SHA256Managed();
			byte[] hash =
				sh.ComputeHash(ConvertStringToByteArray(primer));
			byte[] retVal = new byte[24];
			for (int j = 0; j < retVal.Length; j++) {
				retVal.SetValue(hash.GetValue(j), j);
			}
			return retVal;
		}
        /// <summary>
        /// Get8s the byte initialization vector.
        /// </summary>
        /// <param name="primer">The primer.</param>
        /// <returns></returns>
		private static byte[] Get8ByteInitializationVector(string primer) {
			// Must return a 8 byte array
			SHA256 sh = new SHA256Managed();
			byte[] hash =
				sh.ComputeHash(ConvertStringToByteArray(primer));
			byte[] retVal = new byte[8];
			for (int j = 0; j < retVal.Length; j++) {
				retVal.SetValue(hash.GetValue(j), j);
			}
			return retVal;
		}
	}
}