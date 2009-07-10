using System;
using System.Drawing;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using KTibiaX.MapViewer.Model;
using KTibiaX.MapViewer.Modules;

namespace KTibiaX.MapViewer.Controls {
    public class PreviewPanel : PictureEdit {

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewPanel"/> class.
        /// </summary>
        public PreviewPanel() {
        }

        #region "[rgn] Public Properties   "
        public LabelTheme Theme { get; set; }
        public Image CurrentImage { get; set; }
        public Rectangle TextBounds { get; set; }
        public bool ToolTipIsVisible { get; private set; }
        #endregion

        /// <summary>
        /// Draws the label.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="theme">The theme.</param>
        public void DrawLabel(LabelTheme theme) {
            this.Theme = theme;
            Invalidate();
        }

        #region "[rgn] Overrideable Handlers "
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
            base.OnPaint(e);
            if (!DesignMode) {
                if (ToolTipController == null) { ToolTipController = new ToolTipController(); }
                if (Theme != null) { DrawLabelTheme(e.Graphics); }
                base.Image = CurrentImage;
            }
        }
        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e) {
            base.OnMouseMove(e);
            if (!DesignMode) {
                switch (e.Button) {
                    case System.Windows.Forms.MouseButtons.None:
                        if (TextBounds.Contains(e.Location)) {
                            this.Cursor = Cursors.Hand;
                            ShowToolTip(System.Windows.Forms.Cursor.Position);
                        }
                        else { this.Cursor = Cursors.Normal; if (ToolTipController != null) { ToolTipController.HideHint(); } }
                        break;
                    case System.Windows.Forms.MouseButtons.Left:
                        this.Cursor = Cursors.Move;
                        break;
                }
            }
        }
        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e) {
            base.OnMouseUp(e);
            this.Cursor = Cursors.Hand;
            if (!DesignMode) {
                switch (e.Button) {
                    case System.Windows.Forms.MouseButtons.Left:
                        this.Cursor = Cursors.Normal;
                        break;
                }
            }
        }
        #endregion

        /// <summary>
        /// Draws the label theme.
        /// </summary>
        /// <param name="g">The g.</param>
        protected void DrawLabelTheme(Graphics g) {
            if (Theme != null && !string.IsNullOrEmpty(Theme.Name)) {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                var textSiz = g.MeasureString(Theme.Name, Theme.Font);
                var nx = (this.Size.Width / 2) - (Convert.ToInt32(textSiz.Width) / 2);
                var ny = (this.Size.Height / 2) - (Convert.ToInt32(textSiz.Height) / 2);
                var textPos = new Point(nx, ny);


                g.DrawString(Theme.Name, Theme.Font, Theme.ShadowColor.GetSolidBrush(), textPos.SetOffSet(Theme.ShadowSize, Theme.ShadowSize));
                g.DrawString(Theme.Name, Theme.Font, Theme.ForeColor.GetSolidBrush(), textPos);
                TextBounds = new Rectangle(nx, ny, (Convert.ToInt32(textSiz.Width)), (Convert.ToInt32(textSiz.Height)));
            }
        }

        /// <summary>
        /// Shows the tool tip.
        /// </summary>
        protected void ShowToolTip(Point location) {
            if (Theme != null && !string.IsNullOrEmpty(Theme.Name)) {
                ToolTipController.ShowHint("This is Just a preview of  the current template!",Theme.Name, location);
            }
        }

    }
}
