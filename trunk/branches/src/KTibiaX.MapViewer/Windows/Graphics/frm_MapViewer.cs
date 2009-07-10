using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Keyrox.Shared.Objects;
using KTibiaX.MapViewer.Events;
using KTibiaX.MapViewer.Model;
using KTibiaX.MapViewer.Properties;
using KTibiaX.MapViewer.Windows.Dialogs;

namespace KTibiaX.MapViewer.Windows.Graphics {
    public partial class frm_MapViewer : DevExpress.XtraBars.Ribbon.RibbonForm {
        /// <summary>
        /// Initializes a new instance of the <see cref="frm_MapViewer"/> class.
        /// </summary>
        public frm_MapViewer() {
            InitializeComponent();
            pnNoFiles.Visible = false;
            pnLoading.Visible = false;
            mapViewer1.Cursor = KTibiaX.MapViewer.Controls.Cursors.Normal;
        }

        /// <summary>
        /// Handles the Load event of the frm_MapViewer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void frm_MapViewer_Load(object sender, EventArgs e) {
            UpdateControls();
            LastZoom = 6;
            mapViewer1.Visible = false;
            DefaultFactor = Convert.ToInt32(zoomEditItem.EditValue.ToString());

            if (FixMapsFolder()) {
                lblMapsDir.Caption = Settings.Default.LastMapFolder;
                pnNoFiles.Visible = false;
                MapCallBack load = LoadMap;
                load.BeginInvoke(LoadMap_Complete, load);
            }
            else { FixNoFilesPanelPosition(); pnNoFiles.Visible = true; }
        }

        /// <summary>
        /// Updates the controls.
        /// </summary>
        public void UpdateControls() {
            if (Settings.Default.Labels == null) { Settings.Default.Labels = new KTibiaX.MapViewer.Collections.MapLabelCollection(); }
            if (Settings.Default.Marks == null) { Settings.Default.Marks = new KTibiaX.MapViewer.Collections.MapMarkCollection(); }
            if (Settings.Default.Themes == null) { Settings.Default.Themes = new KTibiaX.MapViewer.Collections.LabelThemeCollection(); }
            Settings.Default.Labels.UpdateMap(mapViewer1);
            Settings.Default.Marks.UpdateMap(mapViewer1);

            mapViewer1.MapLabels = Settings.Default.Labels;
            mapViewer1.MapMarks = Settings.Default.Marks;
            mapViewer1.SetupContextMenus(mapContextMenu, labelContextMenu, markContextMenu);
        }

        #region "[rgn] Public Properties "
        public int DefaultFactor { get; set; }
        public int LastZoom { get; set; }
        public delegate void MapCallBack();
        public delegate void ZoomCallBack(bool decrease, int factor);
        public delegate void FloorCallBack(int floor);
        #endregion

        #region "[rgn] Map Load Events   "
        private void LoadMap() {
            if (!Settings.Default.LastMapFolder.EndsWith("\\")) {
                Settings.Default.LastMapFolder += "\\";
                Settings.Default.Save();
            }
            mapViewer1.LoadingPercentChanged += mapViewer1_LoadingPercentChanged;
            mapViewer1.MapFileChanged += mapViewer1_MapFileChanged;
            mapViewer1.MapLocationChanged += mapViewer1_LocationChanged;

            //TODO: Detect if map files has change and them, delete old maps images.
            //DeleteOldMapImages();

            this.Invoke(new MapCallBack(delegate() {
                DisableControlsOnLoad(false);
                mapViewer1.Directory = Settings.Default.LastMapFolder;
            }));
            mapViewer1.LoadMap();
        }
        private void LoadMap_Complete(IAsyncResult result) {
            if (!mapViewer1.OpenTibiaMaps) { mapViewer1.SetMapCenter(new Location(32585, 31922, 7)); }
            var factor = Convert.ToInt32(zoomEditItem.EditValue.ToString()) - repositoryItemZoomTrackBar1.Minimum;
            ZoomCallBack changeZoom = ChangeZoom;
            changeZoom.BeginInvoke(true, factor, ChangeZoomLoad_Complete, changeZoom);
            this.Invoke(new MapCallBack(delegate() { DisableControlsOnLoad(true); }));
        }
        private bool FixMapsFolder() {
            if (string.IsNullOrEmpty(Settings.Default.LastMapFolder)) {
                Settings.Default.LastMapFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Tibia\\Automap\\";
                lblMapsDir.Caption = Settings.Default.LastMapFolder;
            }
            var di = new DirectoryInfo(Settings.Default.LastMapFolder);
            bool invalidMapsDirectory = false;
            if (!di.Exists) { invalidMapsDirectory = true; }
            else {
                var mapfiles = di.GetFiles("*.map");
                if (mapfiles.Length == 0) { invalidMapsDirectory = true; }
                else { return true; }
            }
            if (invalidMapsDirectory) {
                mapFolderBrowser.ShowDialog();
                if (!string.IsNullOrEmpty(mapFolderBrowser.SelectedPath)) {

                    Settings.Default.LastMapFolder = mapFolderBrowser.SelectedPath;
                    Settings.Default.Save();
                    lblMapsDir.Caption = Settings.Default.LastMapFolder;

                    di = new DirectoryInfo(Settings.Default.LastMapFolder);
                    var mapfiles = di.GetFiles("*.map");
                    if (mapfiles.Length == 0) { return false; }
                    else { return true; }
                }
            }
            return false;
        }
        private void ChangeMapsFolder() {
            mapFolderBrowser.ShowDialog();
            if (!string.IsNullOrEmpty(mapFolderBrowser.SelectedPath)) {
                var di = new DirectoryInfo(Settings.Default.LastMapFolder);
                var mapfiles = di.GetFiles("*.map");
                if (mapfiles.Length == 0) {
                    MessageBox.Show("There is no map files in selected directory!\nLast valid directory was restored.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else {
                    Settings.Default.LastMapFolder = mapFolderBrowser.SelectedPath;
                    Settings.Default.Save();
                    lblMapsDir.Caption = Settings.Default.LastMapFolder;
                    MapCallBack load = LoadMap;
                    load.BeginInvoke(LoadMap_Complete, load);
                }
            }

        }
        private void DeleteOldMapImages() {
            var di = new DirectoryInfo(Settings.Default.LastMapFolder);
            var pngs = di.GetFiles("*.png");
            foreach (var png in pngs) { png.Delete(); }
        }
        private void FixNoFilesPanelPosition() {
            var x = (mapViewer1.Size.Width / 2) - (pnNoFiles.Size.Width / 2);
            var y = (mapViewer1.Size.Height / 2) + (pnNoFiles.Size.Height / 2);
            pnNoFiles.Location = new Point(x, y);
        }
        private void btnDir_Click(object sender, EventArgs e) {
            if (FixMapsFolder()) {
                lblMapsDir.Caption = Settings.Default.LastMapFolder;
                pnNoFiles.Visible = false;
                MapCallBack load = LoadMap;
                load.BeginInvoke(LoadMap_Complete, load);
            }
            else { FixNoFilesPanelPosition(); pnNoFiles.Visible = true; }
        }
        private void btnClose_Click(object sender, EventArgs e) {
            Close();
        }
        #endregion

        #region "[rgn] Map Zoom Events   "
        private void ChangeZoomLoad_Complete(IAsyncResult result) {
            this.Invoke(new MapCallBack(delegate() { mapViewer1.Visible = true; }));
            Thread.Sleep(500);
            ZoomCallBack changeZoom = ChangeZoom;
            changeZoom.BeginInvoke(false, DefaultFactor - 1, null, changeZoom);
        }
        private void barEditItem1_EditValueChanged(object sender, EventArgs e) {
            ChangeZoom(Convert.ToInt32(zoomEditItem.EditValue.ToString()));
        }
        private void ChangeZoom(int newZoom) {
            if (LastZoom != newZoom) {
                bool decrease = newZoom < LastZoom;
                int factor = decrease ? LastZoom - newZoom : newZoom - LastZoom;

                LastZoom = newZoom;                
                ZoomCallBack changeZoom = ChangeZoom;
                changeZoom.BeginInvoke(decrease, factor, ChangeZoom_Complete, changeZoom);
            }
        }
        private void ChangeZoom(bool decrease, int factor) {
            if (decrease) { for (int i = 0; i < factor; i++) { mapViewer1.Zoom(.7); Thread.Sleep(30); } }
            else { for (int i = 0; i < factor; i++) { mapViewer1.Zoom(1.4); Thread.Sleep(30); } }
        }
        private void ChangeZoom_Complete(IAsyncResult result) {
            this.Invoke(new MapCallBack(delegate() { 
                zoomEditItem.Enabled = true;
            }));
        }
        private void repositoryItemZoomTrackBar1_EditValueChanged(object sender, EventArgs e) {
            ribbon.Manager.ActiveEditItemLink.PostEditor();
        }
        #endregion

        #region "[rgn] Loading Events    "
        private void mapViewer1_LoadingPercentChanged(object sender, PercentEventArgs e) {
            mapViewer1.Invoke(new MapCallBack(delegate() {
                if (e.Percent != 100) {
                    if (!pnLoading.Visible) { FixPnLoadingPosition(); pnLoading.Visible = true; }
                    progressLoading.Position = e.Percent;
                }
                else { pnLoading.Visible = false; return; }
            }));
        }
        private void mapViewer1_MapFileChanged(object sender, TextEventArgs e) {
            this.Invoke(new MapCallBack(delegate() {
                var fi = new FileInfo(e.Text);
                string pathMask = string.Empty;
                var paths = fi.FullName.Split(new char[] { '\\' });
                if (paths.Length > 3) {
                    pathMask = string.Concat(paths[0], "\\", paths[1], "\\", paths[2], "\\", paths[3], "\\..\\", fi.Name);
                }
                else if (paths.Length > 2) {
                    pathMask = string.Concat(paths[0], "\\", paths[1], "\\", paths[2], "\\..\\", fi.Name);
                }
                else if (paths.Length > 1) {
                    pathMask = string.Concat(paths[0], "\\", paths[1], "\\..\\", fi.Name);
                }
                else if (paths.Length > 0) {
                    pathMask = string.Concat(paths[0], "\\..\\", fi.Name);
                }
                else { pathMask = fi.Name; }
                lblMapFile.Text = pathMask;
            }));
        }
        private void FixPnLoadingPosition() {
           var x = (mapViewer1.Size.Width / 2) - (pnLoading.Size.Width / 2);
           var y = (mapViewer1.Size.Height / 2) + (pnLoading.Size.Height / 2);
           pnLoading.Location = new Point(x, y);
        }
        #endregion

        #region "[rgn] Floor Events      "
        private void trackBarControl1_EditValueChanged(object sender, EventArgs e) {
            var newFloor = trackBarControl1.EditValue.ToInt32();
            if (newFloor > -1) {
                DisableControlsOnLoad(false);
                FloorCallBack floorChange = ChangeFloor;
                floorChange.BeginInvoke(newFloor, ChangeFloor_Complete, floorChange);
            }
        }
        private void ChangeFloor(int floor) {
            mapViewer1.SwitchToLevel(floor);
        }
        private void ChangeFloor_Complete(IAsyncResult result) {
            this.Invoke(new MapCallBack(delegate() {
                DisableControlsOnLoad(true);
                if (trackBarControl1.EditValue.ToInt32() != mapViewer1.CurrentFloor) { trackBarControl1.EditValue = mapViewer1.CurrentFloor; }
                if (lblCurrentFloor.Text != mapViewer1.CurrentFloor.ToString()) { lblCurrentFloor.Text = mapViewer1.CurrentFloor.ToString(); }
                lblCurrentFloor.Text = mapViewer1.CurrentFloor.ToString();
            }));
        }
        private void DisableControlsOnLoad(bool disabled) {
            trackBarControl1.Enabled = disabled;
            zoomEditItem.Enabled = disabled;
            ribbon.Enabled = disabled;
        }
        #endregion

        #region "[rgn] Location Events   "
        private void mapViewer1_LocationChanged(object sender, LocationEventArgs e) {
            lblLocation.Caption = string.Concat(e.Location.X, ",", e.Location.Y, " - ", e.Location.Z);
        }
        #endregion

        #region "[rgn] Menu Item Handler "
        private void btnChangePath_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            ChangeMapsFolder();
        }
        private void btnRebuild_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            DeleteOldMapImages();
            MapCallBack load = LoadMap;
            load.BeginInvoke(LoadMap_Complete, load);
        }
        private void btnUp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            FloorCallBack floorChange = ChangeFloor;
            floorChange.BeginInvoke(mapViewer1.CurrentFloor - 1, ChangeFloor_Complete, floorChange);
        }
        private void btnDown_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            FloorCallBack floorChange = ChangeFloor;
            floorChange.BeginInvoke(mapViewer1.CurrentFloor + 1, ChangeFloor_Complete, floorChange);
        }
        private void btnZoomM_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            zoomEditItem.EditValue = LastZoom + 1;
        }
        private void btnZoomN_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            zoomEditItem.EditValue = LastZoom - 1;
        }
        private void btnCenter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            FloorCallBack floorChange = ChangeFloor;
            floorChange.BeginInvoke(7, ChangeFloor_Complete, floorChange);
            if (!mapViewer1.OpenTibiaMaps) { mapViewer1.SetMapCenter(new Location(32585, 31922, 7)); }
            else {
                var nloc = mapViewer1.GetMapCenter();
                mapViewer1.SetMapCenter(new Location(nloc.X, nloc.Y, 7));
            }
            zoomEditItem.EditValue = 6;
        }
        private void btnAddLabel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var frmLabel = new frm_MapLabel();
            frmLabel.Databind(mapViewer1, mapViewer1.GetMapCenter());
            frmLabel.LabelChanged += new EventHandler(frmLabel_LabelChanged);
            frmLabel.ShowDialog();
        }
        private void btnAddMark_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var frmMark = new frm_MapMark();
            frmMark.Databind(mapViewer1, mapViewer1.GetMapCenter());
            frmMark.MarkChanged += new EventHandler(frmLabel_LabelChanged);
            frmMark.ShowDialog();
        }
        private void btnAddLabelMenu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var frmLabel = new frm_MapLabel();
            frmLabel.Databind(mapViewer1, mapViewer1.PointToMapCoors(mapViewer1.LastContextMenuLocation));
            frmLabel.LabelChanged += new EventHandler(frmLabel_LabelChanged);
            frmLabel.ShowDialog();
        }
        private void btnAddMarkMenu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var frmMark = new frm_MapMark();
            frmMark.Databind(mapViewer1, mapViewer1.PointToMapCoors(mapViewer1.LastContextMenuLocation));
            frmMark.MarkChanged += new EventHandler(frmLabel_LabelChanged);
            frmMark.ShowDialog();
        }
        private void btnEditLabel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var mapLabel = Settings.Default.Labels.GetLabel(mapViewer1.LastContextMenuLocation);
            if (mapLabel != null) {
                var frmLabel = new frm_MapLabel();
                frmLabel.DataBindLabel(mapLabel);
                frmLabel.LabelChanged += new EventHandler(frmLabel_LabelChanged);
                frmLabel.ShowDialog();
            }
        }
        private void btnEditMark_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var mapMark = Settings.Default.Marks.GetMark(mapViewer1.LastContextMenuLocation);
            if (mapMark != null) {
                var frmMark = new frm_MapMark();
                frmMark.DataBindMark(mapMark);
                frmMark.MarkChanged += new EventHandler(frmLabel_LabelChanged);
                frmMark.ShowDialog();
            }
        }
        private void btnDeleteLabel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var mapLabel = Settings.Default.Labels.GetLabel(mapViewer1.LastContextMenuLocation);
            if (mapLabel != null) {
                var res = MessageBox.Show("Are you shure?", "Delete Label", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes) {
                    Settings.Default.Labels.RemoveAt(Settings.Default.Labels.IndexOf(mapLabel));
                    Settings.Default.Save();
                    mapViewer1.Invalidate();
                }
            }
        }
        private void btnDeleteMark_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var mapMark = Settings.Default.Marks.GetMark(mapViewer1.LastContextMenuLocation);
            if (mapMark != null) {
                var res = MessageBox.Show("Are you shure?", "Delete Mark", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes) {
                    Settings.Default.Marks.RemoveAt(Settings.Default.Marks.IndexOf(mapMark));
                    Settings.Default.Save();
                    mapViewer1.Invalidate();
                }
            }
        }
        private void frmLabel_LabelChanged(object sender, EventArgs e) {
            mapViewer1.Invalidate();
        }
        #endregion








    }
}