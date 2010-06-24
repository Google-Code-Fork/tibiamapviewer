using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using DevExpress.Utils;
using KTibiaX.Analyzer.Events;
using KTibiaX.MapViewer.Model;
using KTibiaX.MapViewer.Modules;

namespace KTibiaX.MapViewer.Controls {
    [Serializable]
    public class MapMark {

        /// <summary>
        /// Initializes a new instance of the <see cref="MapMark"/> class.
        /// </summary>
        public MapMark() {
            OID = DateTime.Now.ToOADate();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapMark"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="image">The image.</param>
        public MapMark(string name, string description, string image, Location location, MapViewer map) {
            OID = DateTime.Now.ToOADate();
            Name = name;
            Description = description;
            ImageName = image;
            MapLocation = location;
            Map = map;
        }
        
        #region "[rgn] Public Properties   "
        [XmlAttribute]
        public double OID { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Description { get; set; }

        [XmlAttribute]
        public string ImageName { get; set; }

        [XmlIgnore]
        public bool MapHooked { get; set; }

        [XmlIgnore]
        public bool MouseIsPressed { get; set; }

        [XmlElement]
        public Location MapLocation { get; set; }

        [XmlIgnore]
        public Size Size { get; set; }

        [XmlIgnore]
        public Point Location { get; set; }

        [XmlIgnore]
        public Point LastTrueLocation { get; set; }
        
        [XmlIgnore]
        public MapViewer Map { get; set; }

        [XmlIgnore]
        public Image Image {
            get {
                var file = new FileInfo(Name);
                if (file.Exists) {
                    return Image.FromFile(Name);
                }
                return null;
            }
        }

        [XmlIgnore]
        public Rectangle MyBounds { get { return new Rectangle(this.Location, this.Size); } }

        [XmlIgnore]
        public ToolTipController TTController { get; set; }
        #endregion

        /// <summary>
        /// Occurs when [context menu requested].
        /// </summary>
        public event EventHandler<PositionEventArgs> ContextMenuRequested;

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
            //TODO:
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
        /// Draws the label.
        /// </summary>
        /// <param name="g">The g.</param>
        public void DrawMark(Graphics g) {
            CalcMyLocation(g);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.DrawImage(Image, Location);
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
            if (Image == null) return;
            var mapPoint = Map.MapCoorsToPoint(MapLocation);
            this.Size = new Size(Image.Width, Image.Height);
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
