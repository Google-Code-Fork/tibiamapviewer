using System.Drawing;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Keyrox.Shared.Objects;
using System.Windows.Forms;

namespace KTibiaX.MapViewer.Modules {
    public static class Extensions {

        /// <summary>
        /// Adds the new item.
        /// </summary>
        /// <param name="combobox">The combobox.</param>
        /// <param name="text">The text.</param>
        /// <param name="value">The value.</param>
        /// <param name="imageIndex">Index of the image.</param>
        public static void AddNewItem(this ImageComboBoxEdit combobox, string text, object value, int imageIndex) {
            combobox.Properties.Items.Add(
                new DevExpress.XtraEditors.Controls.ImageComboBoxItem(text, value, imageIndex)
            );
        }
        /// <summary>
        /// Adds the new item.
        /// </summary>
        /// <param name="combobox">The combobox.</param>
        /// <param name="text">The text.</param>
        /// <param name="value">The value.</param>
        public static void AddNewItem(this ImageComboBoxEdit combobox, string text, object value) {
            AddNewItem(combobox, text, value, -1);
        }

        /// <summary>
        /// Clears the specified combobox.
        /// </summary>
        /// <param name="combobox">The combobox.</param>
        public static void Clear(this ImageComboBoxEdit combobox) {
            combobox.Properties.Items.Clear();
        }

        /// <summary>
        /// Gets the selected value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="combobox">The combobox.</param>
        /// <returns></returns>
        public static T GetSelectedValue<T>(this ImageComboBoxEdit combobox) {
            return combobox.Properties.Items[combobox.SelectedIndex].Value.To<T>();
        }

        /// <summary>
        /// Gets the selected text.
        /// </summary>
        /// <param name="combobox">The combobox.</param>
        /// <returns></returns>
        public static object GetSelectedText(this ImageComboBoxEdit combobox) {
            return combobox.Properties.Items[combobox.SelectedIndex].Description;
        }

        /// <summary>
        /// Selects the combo item.
        /// </summary>
        /// <param name="combobox">The combobox.</param>
        /// <param name="description">The description.</param>
        public static void SelectComboItem(this ImageComboBoxEdit combobox, string description) {
            foreach (ImageComboBoxItem item in combobox.Properties.Items) {
                if (item.Description == description) {
                    combobox.SelectedItem = item;
                    return;
                }
            }
        }

        /// <summary>
        /// Selects the combo item.
        /// </summary>
        /// <param name="combobox">The combobox.</param>
        /// <param name="description">The description.</param>
        public static void SelectComboItem(this ComboBoxEdit combobox, string description) {
            foreach (string item in combobox.Properties.Items) {
                if (item == description) {
                    combobox.SelectedItem = item;
                    return;
                }
            }
        }

        /// <summary>
        /// Sets the alpha.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="alpha">The alpha.</param>
        /// <returns></returns>
        public static Color SetAlpha(this Color color, int alpha) {
            var newColor = Color.FromArgb(alpha, color);
            return newColor;
        }

        /// <summary>
        /// Gets the solid brush.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns></returns>
        public static SolidBrush GetSolidBrush(this Color color) {
            return new SolidBrush(color);
        }

        /// <summary>
        /// Sets the off set.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static Point SetOffSet(this Point p, int x, int y) {
            return new Point(p.X + x, p.Y + y);
        }

        /// <summary>
        /// Matches the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        public static bool Match(this Point source, Point destination) {
            return source.X == destination.X && source.Y == destination.Y;
        }

        /// <summary>
        /// Sets the family.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="family">The family.</param>
        /// <returns></returns>
        public static Font SetFamily(this Font font, FontFamily family) {
            return new Font(family, font.Size, font.Style, font.Unit, font.GdiCharSet, font.GdiVerticalFont);
        }

        /// <summary>
        /// Sets the size.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static Font SetSize(this Font font, float size) {
            return new Font(font.FontFamily, size, font.Style, font.Unit, font.GdiCharSet, font.GdiVerticalFont);
        }

        /// <summary>
        /// Sets the style.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="style">The style.</param>
        /// <returns></returns>
        public static Font SetStyle(this Font font, FontStyle style) {
            return new Font(font.FontFamily, font.Size, style, font.Unit, font.GdiCharSet, font.GdiVerticalFont);
        }

        /// <summary>
        /// Gets the bounds.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns></returns>
        public static Rectangle GetBounds(this Control control) {
            return new Rectangle(control.Location, control.Size);
        }

        /// <summary>
        /// Ins the bounds.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        public static bool InBounds(this Control control, Point location) {
            var rec = control.GetBounds();
            return rec.Contains(location);
        }

        /// <summary>
        /// Creates the control.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        public static Control CreateControl(this Control parent, Point location) {
            var ctrl = new Control(parent, string.Empty) {
                Size = new Size(0, 0),
                Location = location,
            };
            return ctrl;
        }
    }
}
