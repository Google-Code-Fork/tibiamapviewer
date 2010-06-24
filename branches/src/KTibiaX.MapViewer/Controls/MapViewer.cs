using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using Keyrox.Shared.Objects;
using KTibiaX.MapViewer.Events;
using KTibiaX.MapViewer.Model;
using KTibiaX.MapViewer.Modules;
using KTibiaX.MapViewer.Collections;
using KTibiaX.Analyzer.Events;
using KTibiaX.MapViewer.Properties;

namespace KTibiaX.MapViewer.Controls {
    public class MapViewer : Panel {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapViewer"/> class.
        /// </summary>
        public MapViewer() {
            MapLabels = Settings.Default.Labels;
            MapMarks = Settings.Default.Marks;
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();
            PercentChange = OnPercentageChanges;
            MapFileChange = OnMapFileChanges;
            LocationChange = OnLocationChanges;
        }

        #region "[rgn] Private Constants   "
        private const int MapFileDimension = 256;
        private const int FloorMax = 15;
        private const int FloorMin = -1;
        private const string MaskOT = "0??0??";
        private const string MaskReal = "1??1??";
        #endregion

        #region "[rgn] Events & Delegates  "
        public delegate void MapFloorToImagePercentCallback(int percentageComplete);

        public delegate void OnPercentChange(int perc);
        public delegate void OnMapFileChange(string fileName);
        public delegate void OnLocationChange(Location location);

        public event EventHandler<LocationEventArgs> MapLocationChanged;
        public event EventHandler<TextEventArgs> MapFileChanged;
        public event EventHandler<PercentEventArgs> LoadingPercentChanged;
        #endregion

        #region "[rgn] Private Variables   "
        private bool drawCrosshair = true;
        private bool drawCoors = true;
        private bool openTibiaMaps = false;
        private string directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Tibia\\Automap\\";

        private bool canDrawPercentBar = false;
        private int percent = 0;
        private Rectangle mapBoundary;
        private int currentZ = 7;
        private Bitmap[] floorImages = new Bitmap[FloorMax + 1];
        private Point startingPos;
        private Point imagePos = new Point(0, 0);
        private Size imageSize;
        private Image mapImage;
        private double zoomFactor = 1;
        #endregion

        #region "[rgn] Public Properties   "
        public bool IsInDesignMode { get { return DesignMode; } }

        public Point LastContextMenuLocation { get; set; }

        public bool DrawCrosshair {
            get { return drawCrosshair; }
            set { drawCrosshair = value; }
        }

        public bool DrawCoors {
            get { return drawCoors; }
            set { drawCoors = value; }
        }

        public bool OpenTibiaMaps {
            get { return openTibiaMaps; }
            set { openTibiaMaps = value; }
        }

        public string Directory {
            get { return directory; }
            set { directory = value; }
        }

        public int CurrentFloor {
            get {
                return currentZ;
            }
        }

        private MapLabelCollection mapLabels;
        public MapLabelCollection MapLabels {
            get { return mapLabels; }
            set {
                if (value != mapLabels) {
                    mapLabels = value;
                    if (mapLabels != null) {
                        mapLabels.UpdateEvents();
                        mapLabels.ContextMenuRequested += Labels_ContextMenuRequested;
                    }
                }
            }
        }

        private MapMarkCollection mapMarks;
        public MapMarkCollection MapMarks {
            get { return mapMarks; }
            set {
                if (value != mapMarks) {
                    mapMarks = value;
                    if (mapMarks != null) {
                        mapMarks.UpdateEvents();
                        mapMarks.ContextMenuRequested += Marks_ContextMenuRequested;
                    }
                }
            }
        }

        private ApplicationMenu MapContextMenu { get; set; }
        private ApplicationMenu LabelContextMenu { get; set; }
        private ApplicationMenu MarkContextMenu { get; set; }

        public bool MustDrawPercentBoard { get; set; }

        private OnPercentChange PercentChange { get; set; }
        private OnMapFileChange MapFileChange { get; set; }
        private OnLocationChange LocationChange { get; set; }
        #endregion

        #region "[rgn] Protected Handlers  "
        protected override void OnCreateControl() {
            base.OnCreateControl();
        }
        protected override void OnMouseWheel(MouseEventArgs e) {
            base.OnMouseWheel(e);
            Zoom((e.Delta > 0) ? 1.4 : .7);
            Invalidate();
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e) {
            base.OnMouseDoubleClick(e);
            SetMapCenter(PointToMapCoors(new Point(e.X, e.Y)));
            Invalidate();
        }
        protected override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);
            switch (e.Button) {
                case MouseButtons.Left:
                    startingPos.X = e.X;
                    startingPos.Y = e.Y;
                    this.Cursor = Cursors.Move;
                    break;
                case MouseButtons.Right:
                    if (!MapLabels.HasLabel(e.Location)) {
                        Map_ContextMenuRequested(e.Location);
                    }
                    break;
            }
        }
        protected override void OnMouseUp(MouseEventArgs e) {
            base.OnMouseUp(e);
            switch (e.Button) {
                case MouseButtons.Left:
                    this.Cursor = Cursors.Normal;
                    break;
            }
        }
        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            switch (e.Button) {
                case MouseButtons.Left:
                    imagePos.X += e.X - startingPos.X;
                    imagePos.Y += e.Y - startingPos.Y;
                    startingPos.X = e.X;
                    startingPos.Y = e.Y;
                    Invalidate();
                    break;
            }
        }
        protected override void OnPaint(PaintEventArgs e) {
            RedrawMap(e.Graphics);
        }
        protected override void OnResize(EventArgs e) {
            Invalidate();
        }
        protected void OnPercentageChanges(int perc) {
            if (LoadingPercentChanged != null) {
                LoadingPercentChanged(this, new PercentEventArgs(perc));
            }
        }
        protected void OnMapFileChanges(string fileName) {
            if (MapFileChanged != null) {
                MapFileChanged(this, new TextEventArgs(fileName));
            }
        }
        protected void OnLocationChanges(Location location) {
            if (MapLocationChanged != null) {
                MapLocationChanged(this, new LocationEventArgs(location));
            }
        }
        #endregion

        #region "[rgn] Static Methods      "
        /// <summary>
        /// Get the boundary coordinates of an array of map files
        /// </summary>
        /// <param name="mapFiles"></param>
        /// <returns></returns>
        private static Rectangle GetBoundary(FileInfo[] mapFiles) {
            int left = 0;
            int right = 0;
            int top = 0;
            int bottom = 0;
            bool first = true;
            foreach (FileInfo mapFile in mapFiles) {
                Location l = MapFileNameToLocation(mapFile.Name);
                if (first) {
                    left = right = l.X.ToInt32();
                    top = bottom = l.Y.ToInt32();
                    first = false;
                }
                else {
                    if (l.X < left)
                        left = l.X.ToInt32();
                    else if (l.X > right)
                        right = l.X.ToInt32();

                    if (l.Y < top)
                        top = l.Y.ToInt32();
                    else if (l.Y > bottom)
                        bottom = l.Y.ToInt32();
                }
            }
            return new Rectangle(left, top, right - left + MapFileDimension, bottom - top + MapFileDimension);
        }

        /// <summary>
        /// Get a map file name from coordinates
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        private static string GetMapFileName(string directory, uint x, uint y, uint z) {
            return directory +
                (x / MapFileDimension).ToString("000") +
                (y / MapFileDimension).ToString("000") +
                z.ToString("00") + ".map";
        }

        /// <summary>
        /// Convert a map file name to a location
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static Location MapFileNameToLocation(string fileName) {
            Location l = new Location(0, 0, 0);
            if (fileName.Length == 12 || fileName.Length == 8) {
                l.X = (Int32.Parse(fileName.Substring(0, 3)) * MapFileDimension);
                l.Y = (Int32.Parse(fileName.Substring(3, 3)) * MapFileDimension);
                l.Z = (Int32.Parse(fileName.Substring(6, 2)));
            }
            return l;
        }

        /// <summary>
        /// Read a map file into an image
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Image MapFileToImage(string path) {
            // All map files are 256 x 256
            Bitmap bitmap = new Bitmap(MapFileDimension, MapFileDimension);

            // Make sure the file actually exists
            if (File.Exists(path)) {
                // Open the file in a BufferedStream
                using (BufferedStream bs = new BufferedStream(new FileStream(path, FileMode.Open))) {
                    // Each map file contains this many pixels
                    byte[] array = new byte[65536];

                    // Read all of them at once (much faster than byte by byte) into an array
                    bs.Read(array, 0, 65536);

                    // Loop through the array
                    int index = 0;
                    for (int x = 0; x < MapFileDimension; x++) {
                        for (int y = 0; y < MapFileDimension; y++) {
                            byte b = array[index];

                            // Set the pixel on the bitmap, converting the byte a color
                            bitmap.SetPixel(x, y, ByteToColor(b));
                            index++;
                        }
                    }
                }
            }

            return bitmap;
        }

        /// <summary>
        /// Convert a map file byte to a color
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Color ByteToColor(byte b) {
            switch (b) {
                case 0x0C: // Foliage, dark green
                    return Color.FromArgb(0, 0x66, 0);
                case 0x18: // Grass, green
                    return Color.FromArgb(0, 0xcc, 0);
                case 0x1e: // Swamp, bright green
                    return Color.FromArgb(0, 0xFF, 0);
                case 0x28: // Water, blue
                    return Color.FromArgb(0x33, 0, 0xcc);
                case 0x56: // Stone wall, dark grey
                    return Color.FromArgb(0x66, 0x66, 0x66);
                case 0x72: // Not sure, maroon
                    return Color.FromArgb(0x99, 0x33, 0);
                case 0x79: // Dirt, brown
                    return Color.FromArgb(0x99, 0x66, 0x33);
                case 0x81: // Paths, tile floors, other floors
                    return Color.FromArgb(0x99, 0x99, 0x99);
                case 0xB3: // Ice, light blue
                    return Color.FromArgb(0xcc, 0xff, 0xff);
                case 0xBA: // Walls, red
                    return Color.FromArgb(0xff, 0x33, 0);
                case 0xC0: // Lava, orange
                    return Color.FromArgb(0xff, 0x66, 0);
                case 0xCF: // Sand, tan
                    return Color.FromArgb(0xff, 0xcc, 0x99);
                case 0xD2: // Ladder, yellow
                    return Color.FromArgb(0xff, 0xff, 0);
                case 0: // Nothing, black
                    return Color.Black;
                default: // Unknown, white
                    //this.Text = "" + b;
                    return Color.White;
            }
        }
        #endregion

        #region "[rgn] Map File Loading    "
        /// <summary>
        /// Load the map into the picturebox at the currentZ level.
        /// Uses two level caching: the generated bitmap is saved as
        /// a png file in the map directory, and the Bitmap object
        /// is saved in memory.
        /// </summary>
        public void LoadMap() {
            Bitmap image;

            // Check if the image is already in memory
            if (floorImages[currentZ] != null) {
                image = floorImages[currentZ];
            }
            else {
                string dir = directory;

                // Open the directory
                DirectoryInfo di = new DirectoryInfo(dir);

                // Get all the map files
                FileInfo[] mapFiles = di.GetFiles(
                    (openTibiaMaps ? MaskOT : MaskReal) +
                    currentZ.ToString("00") +
                    ".map");

                // Find the boundary
                Rectangle r = GetBoundary(mapFiles);
                mapBoundary = r;

                string imageFileName = dir +
                    (r.Left / MapFileDimension).ToString("000") +
                    (r.Top / MapFileDimension).ToString("000") +
                    currentZ.ToString("00") + ".png";

                // Check if we have an image file previously generated
                if (File.Exists(imageFileName)) {
                    image = (Bitmap)Bitmap.FromFile(imageFileName);
                }
                else {
                    if (MustDrawPercentBoard) canDrawPercentBar = true;

                    image = MapFloorToImage(dir, currentZ, delegate(int percentage) {
                        PercentChange.BeginInvoke(percentage, null, PercentChange);
                        if (MustDrawPercentBoard) {
                            this.percent = percentage;
                            Invalidate();
                        }
                    });

                    canDrawPercentBar = false;

                    image.Save(imageFileName, ImageFormat.Png);
                }
                floorImages[currentZ] = image;
            }

            // Draw the bitmap on the picture box
            if (imageSize.Width == 0)
                imageSize = image.Size;
            mapImage = image;

            Invalidate();
        }

        /// <summary>
        /// Get an image of the entire map on the specified floor
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="floor"></param>
        /// <returns></returns>
        public Bitmap MapFloorToImage(string dir, int floor) {
            return MapFloorToImage(dir, floor, null);
        }

        /// <summary>
        /// Get an image of the entire map on the specified floor
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="floor"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public Bitmap MapFloorToImage(string dir, int floor, MapFloorToImagePercentCallback callback) {
            // Open the directory
            DirectoryInfo di = new DirectoryInfo(dir);

            // Get all the map files
            FileInfo[] mapFiles = di.GetFiles((openTibiaMaps ? MaskOT : MaskReal) + floor.ToString("00") + ".map");

            // Find the boundary
            Rectangle r = GetBoundary(mapFiles);

            // Create a new bitmap big enough to hold all the map
            Bitmap b = new Bitmap(r.Width, r.Height);

            // Get the graphics object to draw on the image

            Graphics g = Graphics.FromImage(b);

            // Set the background to be black
            g.Clear(Color.Black);

            // Keep track of how far along we are
            int counter = 0;
            int total = mapFiles.Length;

            // Loop through the map files and draw them onto the bitmap
            foreach (FileInfo mapFile in mapFiles) {
                MapFileChange.BeginInvoke(mapFile.FullName, null, MapFileChange);
                Location l = MapFileNameToLocation(mapFile.Name);
                if (l.Z == currentZ) {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.DrawImage(MapFileToImage(dir + mapFile.Name), l.X.ToInt32() - r.Left, l.Y.ToInt32() - r.Top);
                }
                counter++;
                if (callback != null) { callback((int)(counter * 100.0 / total)); }
            }

            return b;
        }
        #endregion

        #region "[rgn] Map Graphics Draw   "
        /// <summary>
        /// Redraw the map (no background redraw)
        /// </summary>
        private void RedrawMap(Graphics g) {
            RedrawMap(true, g);
        }

        /// <summary>
        /// Redraw the map
        /// </summary>
        /// <param name="clear">if true clears the background first (produces a flicker)</param>
        /// <param name="g"></param>
        private void RedrawMap(bool clear, Graphics g) {
            if (canDrawPercentBar) {
                DrawPercentBar(percent, g);
                return;
            }

            if (mapImage == null)
                return;

            if (clear)
                g.Clear(Color.Black);

            g.DrawImage(mapImage, new Rectangle(imagePos, imageSize));

            if (drawCrosshair)
                DrawCrosshairs(g);

            if (drawCoors)
                DrawCoordinates(g);

            LocationChange.BeginInvoke(GetMapCenter(), null, LocationChange);
            //if (markers != null) {
            //    foreach (MapMarker mc in markers) {
            //        mc.Update();
            //        DrawMarker(mc.Location, mc.Name,
            //            mc.ColorMarkerFill, mc.ColorMarkerOutline, mc.ColorTextFill, mc.ColorTextOutline,
            //            mc.DrawHPBar ? mc.HPBar : -1, g);
            //    }
            //}
            foreach (var label in MapLabels) {
                label.DrawLabel(g);
            }

            foreach (var mark in MapMarks) {
                mark.DrawMark(g);
            }
        }
        #endregion

        #region "[rgn] Coordinate Helpers  "
        /// <summary>
        /// Convert Tibia coordinates to a point on the map
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Point MapCoorsToPoint(int x, int y) {
            return MapCoorsToPoint(new Location(x, y, currentZ));
        }

        /// <summary>
        /// Convert a Tibia Location to a point on the map
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public Point MapCoorsToPoint(Location l) {
            int x = l.X.ToInt32();
            int y = l.Y.ToInt32();
            int newX = (int)((x - mapBoundary.Left) * zoomFactor) + imagePos.X;
            int newY = (int)((y - mapBoundary.Top) * zoomFactor) + imagePos.Y;

            return new Point(newX, newY);
        }

        /// <summary>
        /// Convert a point on the map to a Tibia location
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Location PointToMapCoors(Point p) {
            int newX = (int)((p.X - imagePos.X) / zoomFactor + mapBoundary.Left);
            int newY = (int)((p.Y - imagePos.Y) / zoomFactor + mapBoundary.Top);

            return new Location(newX, newY, currentZ);
        }

        /// <summary>
        /// Get the point at the center of the picturebox
        /// </summary>
        /// <returns></returns>
        public Point GetMapCenterPoint() {
            int x = Width / 2;
            int y = Height / 2;

            return new Point(x, y);
        }

        /// <summary>
        /// Set the center of the map to a Tibia Location
        /// </summary>
        /// <param name="l"></param>
        public void SetMapCenter(Location l) {
            Point center = GetMapCenterPoint();

            int newX = (int)((l.X - mapBoundary.Left) * zoomFactor * -1 + center.X);
            int newY = (int)((l.Y - mapBoundary.Top) * zoomFactor * -1 + center.Y);

            imagePos.X = newX;
            imagePos.Y = newY;

            Invalidate();
        }

        /// <summary>
        /// Get the center of the map in Tibia Coordinates
        /// </summary>
        /// <returns></returns>
        public Location GetMapCenter() {
            return PointToMapCoors(GetMapCenterPoint());
        }
        #endregion

        #region "[rgn] Draw Method Helpers "
        /// <summary>
        /// Draw a percentage bar on the map
        /// </summary>
        /// <param name="percent"></param>
        /// <param name="g"></param>
        private void DrawPercentBar(int percent, Graphics g) {
            Font font = new Font("Tahoma", 10, FontStyle.Bold);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            int width = Width / 2;
            int height = font.Height;
            int x = (int)((Width - width) / 2);
            int y = (int)((Height - height) / 2);

            Rectangle outer = new Rectangle(x, y, width, height);

            Rectangle inner = new Rectangle(x + 1, y + 1, (int)((width - 2) * percent / 100.0), height - 2);

            g.FillRectangle(Brushes.Black, outer);
            g.FillRectangle(Brushes.DarkGray, inner);
            g.DrawString(percent + "%", font, Brushes.White, outer, sf);
        }

        /// <summary>
        /// Draw crosshairs in the middle of the map
        /// </summary>
        private void DrawCrosshairs(Graphics g) {
            DrawCrosshairs(GetMapCenterPoint(), g);
        }

        /// <summary>
        /// Draw crosshairs at the specified point
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="g"></param>
        private void DrawCrosshairs(int x, int y, Graphics g) {
            DrawCrosshairs(new Point(x, y), g);
        }

        /// <summary>
        /// Draw crosshairs at the specified point
        /// </summary>
        /// <param name="p"></param>
        /// <param name="g"></param>
        private void DrawCrosshairs(Point p, Graphics g) {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            int x = p.X;
            int y = p.Y;

            var pen = new Pen(Color.White.SetAlpha(180), 1);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            g.DrawLine(pen, new Point(x - 500, y), new Point(x + 500, y));
            g.DrawLine(pen, new Point(x, y - 500), new Point(x, y + 500));

            g.DrawEllipse(new Pen(Color.Black.SetAlpha(60), 6), x - 16, y - 16, 32, 32);
            g.FillEllipse(Color.White.SetAlpha(130).GetSolidBrush(), new Rectangle(x - 10, y - 10, 20, 20));

            g.DrawLine(new Pen(Color.White, 1), new Point(x - 5, y), new Point(x + 5, y));
            g.DrawLine(new Pen(Color.White, 1), new Point(x, y - 5), new Point(x, y + 5));

        }

        /// <summary>
        /// Draw the current coordinates of the center point in the upper right corner
        /// </summary>
        private void DrawCoordinates(Graphics g) {
            Location l = GetMapCenter();

            Font font = new Font("Tahoma", 10, FontStyle.Bold);
            Rectangle rect = new Rectangle(Width - 120, 0, 120, font.Height);

            // g.FillRectangle(new SolidBrush(Color.FromArgb(100, 255, 255, 255)), rect);
            var brush = new SolidBrush(Color.Black.SetAlpha(100));
            g.FillRectangle(brush, rect);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            g.DrawString("" + l.X + ", " + l.Y, font, Brushes.White, rect, sf);
        }

        /// <summary>
        /// Draw a marker with the default colors.
        /// </summary>
        /// <param name="l"></param>
        /// <param name="text"></param>
        /// <param name="g"></param>
        private void DrawMarker(Location l, string text, Graphics g) {
            DrawMarker(l, text, Color.Yellow, Color.Black, g);
        }

        /// <summary>
        /// Draw a marker with the default text fill and outline colors.
        /// </summary>
        /// <param name="l"></param>
        /// <param name="text"></param>
        /// <param name="markerFill"></param>
        /// <param name="markerOutline"></param>
        /// <param name="g"></param>
        private void DrawMarker(Location l, string text, Color markerFill, Color markerOutline, Graphics g) {
            DrawMarker(l, text, markerFill, markerOutline, Color.White, Color.Black, g);
        }

        private void DrawMarker(Location l, string text, Color markerFill, Color markerOutline, Color textFill, Color textOutline, Graphics g) {
            DrawMarker(l, text, markerFill, markerOutline, textFill, textOutline, -1, g);
        }

        /// <summary>
        /// Draw a marker given the specifications.
        /// </summary>
        /// <param name="l"></param>
        /// <param name="text"></param>
        /// <param name="markerFill"></param>
        /// <param name="markerOutline"></param>
        /// <param name="textFill"></param>
        /// <param name="textOutline"></param>
        /// <param name="hpBar">if hpBar >= 0 &amp;&amp; &lt;= 100 draw an HP bar</param>
        /// <param name="g"></param>
        private void DrawMarker(Location l, string text, Color markerFill, Color markerOutline, Color textFill, Color textOutline, int hpBar, Graphics g) {
            // Convert to Tibia coors
            Point p = MapCoorsToPoint(l);

            // This array of points makes the marker
            Point[] points = {
                new Point(p.X - 3, p.Y - 25),
                new Point(p.X + 3, p.Y - 25),
                new Point(p.X + 3, p.Y - 20),
                new Point(p.X + 10, p.Y - 20),
                new Point(p.X, p.Y),
                new Point(p.X - 10, p.Y - 20),
                new Point(p.X - 3, p.Y - 20)
            };

            // Anti-aliased for nice effect
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Draw the marker
            g.FillPolygon(new SolidBrush(markerFill), points);
            g.DrawPolygon(new Pen(markerOutline), points);

            // The text font
            Font font = new Font("Tahoma", 8, FontStyle.Bold);

            // The rectangle to hold the text
            Rectangle rect = new Rectangle(p.X - 100,
                p.Y - (((hpBar >= 0 && hpBar <= 100) ? 33 : 27) + font.Height),
                200, font.Height);

            // Center the text
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            // Add "up" or "down" to the name
            if (l.Z != 0 && l.Z != currentZ)
                text += " (" + (currentZ - l.Z > 0 ? "+" : "") + (currentZ - l.Z) + ")";

            // Draw the outlined text
            DrawOutlinedText(text, font, new SolidBrush(textFill), new SolidBrush(textOutline), rect, sf, g);

            // Draw HP bar if needed
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            if (hpBar >= 0 && hpBar <= 100) {
                Rectangle hpOuter = new Rectangle(p.X - 14, p.Y - 27 - 4, 28, 4);
                Rectangle hpInner = new Rectangle(p.X - 13, p.Y - 26 - 4,
                    (int)(hpBar / 100.0 * 26), 2);
                g.FillRectangle(Brushes.Black, hpOuter);
                //g.FillRectangle(new SolidBrush(Misc.HpPercentToColor(hpBar)), hpInner);
            }
        }

        /// <summary>
        /// Draw the specified text with an outline
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="fill"></param>
        /// <param name="outline"></param>
        /// <param name="rect"></param>
        /// <param name="sf"></param>
        /// <param name="g"></param>
        private void DrawOutlinedText(string text, Font font, Brush fill, Brush outline, Rectangle rect, StringFormat sf, Graphics g) {
            // Draw the outline by offsetting the rectangle
            rect.Offset(-1, -1);
            g.DrawString(text, font, outline, rect, sf);
            rect.Offset(2, 0);
            g.DrawString(text, font, outline, rect, sf);
            rect.Offset(0, 2);
            g.DrawString(text, font, outline, rect, sf);
            rect.Offset(-2, 0);
            g.DrawString(text, font, outline, rect, sf);

            // Return to the original position and draw the fill
            rect.Offset(1, -1);
            g.DrawString(text, font, fill, rect, sf);
        }
        #endregion

        #region "[rgn] Public User Actions "
        /// <summary>
        /// Move down a level
        /// </summary>
        public void LevelUp() {
            if (currentZ > FloorMin) {
                currentZ--;
                LoadMap();
            }
        }

        /// <summary>
        /// Move up a level
        /// </summary>
        public void LevelDown() {
            if (currentZ < FloorMax) {
                currentZ++;
                LoadMap();
            }
        }

        public void SwitchToLevel(int level) {
            if (level > FloorMin && level < FloorMax) {
                currentZ = level;
                LoadMap();
            }
        }

        /// <summary>
        /// Set the level of the map
        /// </summary>
        /// <param name="z"></param>
        public void SetLevel(int z) {
            if (z != currentZ && (currentZ < FloorMax || currentZ > FloorMin)) {
                currentZ = z;
                LoadMap();
            }
        }
        /// <summary>
        /// Set the zoom factor for the map
        /// </summary>
        /// <param name="factor"></param>
        public void Zoom(double factor) {
            Location center = GetMapCenter();

            zoomFactor *= factor;
            imageSize.Height = (int)(imageSize.Height * factor);
            imageSize.Width = (int)(imageSize.Width * factor);

            SetMapCenter(center);
        }
        #endregion

        /// <summary>
        /// Setups the context menus.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="label">The label.</param>
        /// <param name="mark">The mark.</param>
        public void SetupContextMenus(ApplicationMenu map, ApplicationMenu label, ApplicationMenu mark) {
            MapContextMenu = map;
            LabelContextMenu = label;
            MarkContextMenu = mark;
        }

        /// <summary>
        /// Map_s the context menu requested.
        /// </summary>
        /// <param name="p">The p.</param>
        private void Map_ContextMenuRequested(Point p) {
            if (MapContextMenu != null) {
                LastContextMenuLocation = p;
                MapContextMenu.ShowPopup(Cursor.Position);
            }
        }

        /// <summary>
        /// Handles the ContextMenuRequested event of the Labels control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="KTibiaX.Analyzer.Events.PositionEventArgs"/> instance containing the event data.</param>
        private void Labels_ContextMenuRequested(object source, PositionEventArgs e) {
            if (LabelContextMenu != null) {
                LastContextMenuLocation = e.Position;
                LabelContextMenu.ShowPopup(Cursor.Position);
            }
        }

        /// <summary>
        /// Handles the ContextMenuRequested event of the Marks control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="KTibiaX.Analyzer.Events.PositionEventArgs"/> instance containing the event data.</param>
        private void Marks_ContextMenuRequested(object source, PositionEventArgs e) {
            if (MarkContextMenu != null) {
                LastContextMenuLocation = e.Position;
                MarkContextMenu.ShowPopup(Cursor.Position);
            }
        }
    }
}
