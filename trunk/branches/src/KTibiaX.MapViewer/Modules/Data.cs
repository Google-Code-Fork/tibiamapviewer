using System;
using System.Collections.Generic;
using System.IO;
using Keyrox.Shared.Files;
using Keyrox.Shared.Objects;
using KTibiaX.MapViewer.Collections;
using KTibiaX.MapViewer.Properties;
using System.Windows.Forms;

namespace KTibiaX.MapViewer.Modules {
    public static class Data {

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize() {
            var di = new DirectoryInfo(Path.Combine(Application.StartupPath, Resources.DataDirectory));
            if (!di.Exists) di.Create();

            if (!ReadThemesFile()) { Settings.Default.Themes = new LabelThemeCollection(); }
            if (!ReadLabelsFile()) { Settings.Default.Labels = new MapLabelCollection(); }
            if (!ReadMarksFile()) { Settings.Default.Marks = new MapMarkCollection(); }

        }
        /// <summary>
        /// Persists the values.
        /// </summary>
        public static void PersistValues() {
            var di = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, Resources.DataDirectory));
            if (!di.Exists) di.Create();

            WriteThemesFile();
            WriteLabelsFile();
            WriteMarksFile();
        }

        #region "[rgn] Themes     "
        /// <summary>
        /// Reads the themes file.
        /// </summary>
        private static bool ReadThemesFile() {
            var fi = new FileInfo(GetDataPath(Resources.ThemesFile));
            if (fi.Exists) {
                var xml = fi.Read();
                Settings.Default.Themes = xml.Deserialize<LabelThemeCollection>();
                Settings.Default.Save();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Writes the themes file.
        /// </summary>
        private static void WriteThemesFile() {
            if (Settings.Default.Themes == null) {
                Settings.Default.Themes = new LabelThemeCollection();
            }
            var fi = new FileInfo(GetDataPath(Resources.ThemesFile));
            if (fi.Exists) { fi.Delete(); }
            fi.Write(Settings.Default.Themes.Serialize());
        }
        #endregion

        #region "[rgn] Labels     "
        /// <summary>
        /// Reads the labels file.
        /// </summary>
        private static bool ReadLabelsFile() {
            var fi = new FileInfo(GetDataPath(Resources.LabelsFile));
            if (fi.Exists) {
                var xml = fi.Read();
                Settings.Default.Labels = xml.Deserialize<MapLabelCollection>();
                Settings.Default.Save();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Writes the labels file.
        /// </summary>
        private static void WriteLabelsFile() {
            if (Settings.Default.Labels == null) {
                Settings.Default.Labels = new MapLabelCollection();
            }
            var fi = new FileInfo(GetDataPath(Resources.LabelsFile));
            if (fi.Exists) { fi.Delete(); }
            fi.Write(Settings.Default.Labels.Serialize());
        }
        #endregion

        #region "[rgn] Marks      "
        /// <summary>
        /// Reads the marks file.
        /// </summary>
        private static bool ReadMarksFile() {
            var fi = new FileInfo(GetDataPath(Resources.MarksFile));
            if (fi.Exists) {
                var xml = fi.Read();
                Settings.Default.Marks = xml.Deserialize<MapMarkCollection>();
                Settings.Default.Save();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Writes the marks file.
        /// </summary>
        private static void WriteMarksFile() {
            if (Settings.Default.Marks == null) {
                Settings.Default.Marks = new MapMarkCollection();
            }
            var fi = new FileInfo(GetDataPath(Resources.MarksFile));
            if (fi.Exists) { fi.Delete(); }
            fi.Write(Settings.Default.Marks.Serialize());

        }
        #endregion

        #region "[rgn] Items      "
        public static Dictionary<int, string> GetImagesPath() {
            var res = new Dictionary<int, string>();

            var di = new System.IO.DirectoryInfo(Path.Combine(System.Windows.Forms.Application.StartupPath, "Resources\\items"));
            if (di.Exists) {
                var files = di.GetFiles("*.gif");
                for (int i = 0; i < files.Length; i++) {
                    res.Add(i, files[i].FullName);
                }
            }
            return res;
        }
        public static System.Windows.Forms.ImageList GetImagesList(Dictionary<int, string> imagesPath) {
            var imglist = new System.Windows.Forms.ImageList();
            for(int i = 0; i < imagesPath.Count; i++){
                imglist.Images.Add(imagesPath[i], System.Drawing.Image.FromFile(imagesPath[i]));
            }
            return imglist;
        }
        #endregion

        /// <summary>
        /// Gets the data path.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        private static string GetDataPath(string fileName) {
            return Path.Combine(Environment.CurrentDirectory, string.Concat(Resources.DataDirectory, "\\", fileName));
        }

    }
}
