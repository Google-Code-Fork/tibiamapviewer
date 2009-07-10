using System.IO;
using System.Linq;
using Keyrox.Shared.Objects;
using System.Windows.Forms;

namespace Keyrox.Shared.Files {
    public static class Files {

        /// <summary>
        /// Reads the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static string Read(this FileInfo file) {
            if (!file.Exists) throw new FileNotFoundException();
            var reader = new StreamReader(file.FullName);
            string result = string.Empty;
            try {
                result = reader.ReadToEnd();
            }
            finally { reader.Close(); }
            return result;
        }

        /// <summary>
        /// Creates the file info.
        /// </summary>
        /// <param name="fromAppPath">if set to <c>true</c> [from app path].</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static FileInfo CreateFileInfo(bool fromAppPath, params string[] args) {
            var path = string.Empty;
            if (fromAppPath) { path = Application.StartupPath; }
            args.ToList().ForEach(dir => path = Path.Combine(path, dir.Replace("\\", "")));
            return new FileInfo(path);
        }

        /// <summary>
        /// Reads the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static string Read(string filePath) {
            return Read(new FileInfo(filePath));
        }

        /// <summary>
        /// Appends the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="content">The content.</param>
        public static void Append(this FileInfo file, string content) {
            if (!file.Exists) throw new FileNotFoundException();
            Write(content, file.FullName, true);
        }

        /// <summary>
        /// Writes the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="content">The content.</param>
        public static void Write(this FileInfo file, string content) {
            Write(content, file.FullName, false);
        }

        /// <summary>
        /// Writes the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="append">if set to <c>true</c> [append].</param>
        public static void Write(string content, string filePath, bool append) {
            var file = new FileInfo(filePath);
            if (!append) {
                if (file.Exists) file.Delete();
            }
            StreamWriter writer = null;
            try {
                if (append && file.Exists) {
                    writer = file.AppendText();
                }
                else if (!file.Exists) {
                    if (!Directory.Exists(file.Directory.FullName)) {
                        Directory.CreateDirectory(file.Directory.FullName);
                    }
                    writer = new StreamWriter(filePath);
                }
                else {
                    writer = new StreamWriter(filePath);
                }
                writer.Write(content);
            }
            finally { writer.Flush(); writer.Close(); }
        }

        /// <summary>
        /// Deserializes the specified file.
        /// </summary>
        /// <typeparam name="T">Output type.</typeparam>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static T Deserialize<T>(this FileInfo file) {
            if (!file.Exists) throw new FileNotFoundException();
            return file.Read().Deserialize<T>();
        }

    }
}
