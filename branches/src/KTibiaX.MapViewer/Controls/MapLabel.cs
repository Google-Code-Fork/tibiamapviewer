using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using DevExpress.Utils;
using KTibiaX.Analyzer.Events;
using KTibiaX.MapViewer.Model;
using KTibiaX.MapViewer.Modules;
using KTibiaX.MapViewer.Properties;

namespace KTibiaX.MapViewer.Controls {
    [Serializable]
    public class MapLabel {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapLabel"/> class.
        /// </summary>
        public MapLabel() { OID = DateTime.Now.ToOADate(); }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapLabel"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="description">The description.</param>
        /// <param name="theme">The theme.</param>
        /// <param name="location">The location.</param>
        /// <param name="map">The map.</param>
        public MapLabel(string text, string description, LabelTheme theme, Location location, MapViewer map) {
            OID = DateTime.Now.ToOADate();
            Text = text;
            Description = description;
            Theme = theme;
            Map = map;
            MapLocation = location;
        }

        #region "[rgn] Public Properties   "
        [XmlAttribute]
        public double OID { get; set; }

        [XmlAttribute]
        public string Text { get; set; }

        [XmlAttribute]
        public string Description { get; set; }

        [XmlIgnore]
        public bool MapHooked { get; set; }

        [XmlIgnore]
        public bool MouseIsPressed { get; set; }

        [XmlIgnore]
        public Size Size { get; set; }

        [XmlIgnore]
        public Point Location { get; set; }

        [XmlIgnore]
        public Point LastTrueLocation { get; set; }

        [XmlElement]
        public Location MapLocation { get; set; }

        [XmlAttribute]
        public string ThemeName { get; set; }

        [XmlIgnore]
        public LabelTheme Theme {
            get { 
                return Settings.Default.Themes.GetTheme(ThemeName); 
            }
            set {
                if (value != null) {
                    if (Settings.Default.Themes.GetTheme(value.Name) != null) {
                        ThemeName = value.Name;
                    }
                    else { throw new ArgumentException("Cannot use unsaved Themes!"); }
                }
            }
        }

        [XmlIgnore]
        public MapViewer Map { get; set; }

        [XmlIgnore]
        public Rectangle MyBounds { get { return new Rectangle(this.Location, this.Size); } }

        [XmlIgnore]
        public ToolTipController TTController { get; set; }
        #endregion

        #region "[rgn] Map Mouse Events    "
        private void HookMap() {
            Map.MouseMove += new MouseEventHandler(map_MouseMove);
            Map.MouseDown += new MouseEventHandler(Map_MouseDown);
            Map.MouseUp += new MouseEventHandler(Map_MouseUp);
            MapHooked = true;
        }
        private void Map_MouseDown(object sender, MouseEventArgs e) {
            switch (e.Button) {
                case System.Windows.Forms.MouseButtons.Left:
                    LastTrueLocation = e.Location;
                    break;
            }
        }
        private void Map_MouseUp(object sender, MouseEventArgs e) {
            switch (e.Button) {
                case MouseButtons.Left:
                    if (MyBounds.Contains(e.Location)) {
                        if (LastTrueLocation.Match(e.Location)) {
                            Map.Cursor = Cursors.Working;
                            OnClick(e);
                        }
                    }
                    break;
                case MouseButtons.Right:
                    if (MyBounds.Contains(e.Location)) {
                        if (ContextMenuRequested != null) {
                            ContextMenuRequested(this, new PositionEventArgs(e.Location));
                        }
                    }
                    break;
            }
        }
        private void map_MouseMove(object sender, MouseEventArgs e) {
            switch (e.Button) {
                case System.Windows.Forms.MouseButtons.None:
                    if (MyBounds.Contains(e.Location)) {
                        Map.Cursor = Cursors.Hand;
                        ShowToolTip(System.Windows.Forms.Cursor.Position);
                    }
                    else { Map.Cursor = Cursors.Normal; HideToolTip(); }
                    break;
            }
        }
        #endregion

        #region "[rgn] Tool Tip Controller "
        /// <summary>
        /// Shows the tool tip.
        /// </summary>
        /// <param name="location">The location.</param>
        public void ShowToolTip(Point location) {
            if (TTController == null) { TTController = new ToolTipController(); }
            if (Theme != null && !string.IsNullOrEmpty(Text)) {
                TTController.ShowHint(Description, Text, location);
            }
        }
        /// <summary>
        /// Hides the tool tip.
        /// </summary>
        public void HideToolTip() {
            if (TTController != null) {
                TTController.HideHint();
            }
        }
        #endregion

        /// <summary>
        /// Occurs when [context menu requested].
        /// </summary>
        public event EventHandler<PositionEventArgs> ContextMenuRequested;

        /// <summary>
        /// Draws the label.
        /// </summary>
        /// <param name="g">The g.</param>
        public void DrawLabel(Graphics g) {
            CalcMyLocation(g);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.DrawString(Text, Theme.Font, Theme.ShadowColor.GetSolidBrush(), Location.SetOffSet(Theme.ShadowSize, Theme.ShadowSize));
            g.DrawString(Text, Theme.Font, Theme.ForeColor.GetSolidBrush(), Location);
            if (!MapHooked) { HookMap(); }
        }

        /// <summary>
        /// Raises the <see cref="E:Click"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        public void OnClick(MouseEventArgs e) {
            Map.SetMapCenter(this.MapLocation);
            //System.Threading.Thread.Sleep(1000);
            //TODO: Check Information.
        }

        /// <summary>
        /// Calcs my location.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <returns></returns>
        public void CalcMyLocation(Graphics g) {
            var textSize = g.MeasureString(Text, Theme.Font);
            var mapPoint = Map.MapCoorsToPoint(MapLocation);
            this.Size = new Size(Convert.ToInt32(textSize.Width), Convert.ToInt32(textSize.Height));
            this.Location = new Point(mapPoint.X - (this.Size.Width / 2), mapPoint.Y - (this.Size.Height / 2));
        }

        /// <summary>
        /// Determines whether [is in bounds] [the specified point].
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>
        /// 	<c>true</c> if [is in bounds] [the specified point]; otherwise, <c>false</c>.
        /// </returns>
        public bool InBounds(Point point) {
            return MyBounds.Contains(point);
        }
    }
}
