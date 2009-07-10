using System;
using System.Drawing;
using System.Xml.Serialization;

namespace KTibiaX.MapViewer.Model {
    [Serializable, XmlRoot("Theme")]
    public class LabelTheme {
        /// <summary>
        /// Initializes a new instance of the <see cref="LabelTheme"/> class.
        /// </summary>
        public LabelTheme() {
            Font = new Font("Arial", 20, FontStyle.Bold, GraphicsUnit.Point);
            ForeColor = Color.White;
            ShadowColor = Color.Black;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelTheme"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="font">The font.</param>
        /// <param name="shadowSize">Size of the shadow.</param>
        /// <param name="zoomIn">if set to <c>true</c> [zoom in].</param>
        /// <param name="foreColor">Color of the fore.</param>
        /// <param name="shadowColor">Color of the shadow.</param>
        public LabelTheme(string name, Font font, int shadowSize, bool zoomIn, Color foreColor, Color shadowColor) {
            Name = name;
            Font = font;
            ZoomIn = zoomIn;
            ZoomOut = !zoomIn;
            ForeColor = foreColor;
            ShadowSize = shadowSize;
            ShadowColor = shadowColor;
        }
        #region "[rgn] Public Properties   "
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public int ShadowSize { get; set; }

        [XmlAttribute]
        public bool ZoomIn { get; set; }
        [XmlAttribute]
        public bool ZoomOut { get; set; }

        [XmlAttribute]
        public string HexForeColor { get; set; }
        [XmlIgnore]
        public Color ForeColor {
            get { return ColorTranslator.FromHtml(HexForeColor); }
            set { HexForeColor = ColorTranslator.ToHtml(value); }
        }

        [XmlAttribute]
        public string HexShadowColor { get; set; }
        [XmlIgnore]
        public Color ShadowColor {
            get { return ColorTranslator.FromHtml(HexShadowColor); }
            set { HexShadowColor = ColorTranslator.ToHtml(value); }
        }

        [XmlElement]
        public SerialFont SFont { get; set; }
        [XmlIgnore]
        public Font Font {
            get { return SFont != null ? SFont.GetFont() : null; }
            set { SFont = new SerialFont(value); }
        }
        #endregion


        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString() {
            return Name;
        }

    }
}
