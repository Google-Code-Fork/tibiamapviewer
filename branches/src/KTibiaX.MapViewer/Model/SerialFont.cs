using System;
using System.Drawing;
using System.Xml.Serialization;

namespace KTibiaX.MapViewer.Model {
    [Serializable]
    public class SerialFont {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerialFont"/> class.
        /// </summary>
        public SerialFont() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialFont"/> class.
        /// </summary>
        /// <param name="font">The font.</param>
        public SerialFont(Font font) {
            SetFont(font);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialFont"/> class.
        /// </summary>
        /// <param name="family">The family.</param>
        /// <param name="size">The size.</param>
        public SerialFont(string family, float size) {
            Family = family;
            Size = size;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialFont"/> class.
        /// </summary>
        /// <param name="family">The family.</param>
        /// <param name="size">The size.</param>
        /// <param name="style">The style.</param>
        public SerialFont(string family, float size, FontStyle style) {
            Family = family;
            Size = size;
            Style = style;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialFont"/> class.
        /// </summary>
        /// <param name="family">The family.</param>
        /// <param name="size">The size.</param>
        /// <param name="style">The style.</param>
        /// <param name="gdiCharSet">The GDI char set.</param>
        /// <param name="gdiVerticalFont">if set to <c>true</c> [GDI vertical font].</param>
        public SerialFont(string family, float size, FontStyle style, byte gdiCharSet, bool gdiVerticalFont) {
            Family = family;
            Size = size;
            Style = style;
            GdiCharSet = gdiCharSet;
            GdiVerticalFont = gdiVerticalFont;
        }

        #region "[rgn] Public Properties   "
        [XmlAttribute]
        public string Family { get; set; }
        [XmlAttribute]
        public float Size { get; set; }
        [XmlAttribute]
        public FontStyle Style { get; set; }
        [XmlAttribute]
        public byte GdiCharSet { get; set; }
        [XmlAttribute]
        public bool GdiVerticalFont { get; set; }
        #endregion

        /// <summary>
        /// Gets the font.
        /// </summary>
        /// <returns></returns>
        public Font GetFont() {
            return new Font(Family, Size, Style, GraphicsUnit.Point, GdiCharSet, GdiVerticalFont);
        }

        /// <summary>
        /// Sets the font.
        /// </summary>
        /// <param name="font">The font.</param>
        public void SetFont(Font font) {
            Family = font.FontFamily.Name;
            Size = font.Size;
            Style = font.Style;
            GdiCharSet = font.GdiCharSet;
            GdiVerticalFont = font.GdiVerticalFont;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString() {
            return string.Format("{0}, {1}", Family, Size);
        }

    }
}
