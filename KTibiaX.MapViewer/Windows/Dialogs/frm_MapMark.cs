using System;
using System.Collections.Generic;
using System.Drawing;
using DevExpress.XtraEditors.Controls;
using KTibiaX.MapViewer.Controls;
using KTibiaX.MapViewer.Model;
using KTibiaX.MapViewer.Properties;

namespace KTibiaX.MapViewer.Windows.Dialogs {
    public partial class frm_MapMark : DevExpress.XtraEditors.XtraForm {
        /// <summary>
        /// Initializes a new instance of the <see cref="frm_MapMark"/> class.
        /// </summary>
        public frm_MapMark() {
            InitializeComponent();
        }

        #region "[rgn] Properties "
        public MapMark CurrentMark { get; set; }
        public Location MapLocation { get; set; }
        public Controls.MapViewer Map { get; set; }
        public event EventHandler MarkChanged;
        #endregion

        /// <summary>
        /// Handles the Load event of the frm_MapMark control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void frm_MapMark_Load(object sender, EventArgs e) {
            LoadImages();
        }

        /// <summary>
        /// Loads the images.
        /// </summary>
        private void LoadImages() {
            var paths = Modules.Data.GetImagesPath();
            var images = Modules.Data.GetImagesList(paths);

            var items = new List<ImageComboBoxItem>();
            foreach (var path in paths) {
                var file = new System.IO.FileInfo(path.Value);
                items.Add(new ImageComboBoxItem(file.Name, file.FullName, path.Key));
                imgItems.Images.Add(Image.FromFile(path.Value));
            }
            ddlImages.Properties.BeginUpdate();
            ddlImages.Properties.Items.AddRange(items.ToArray());
            ddlImages.Properties.EndUpdate();
            if (items.Count > 0) ddlImages.SelectedIndex = 0;
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnSave_Click(object sender, EventArgs e) {
            if (txtDescription.Text.Length < 3) { dxErrorProvider1.SetError(txtDescription, "Invalid description!"); return; }
            else { dxErrorProvider1.SetError(txtDescription, ""); }

            var file = new System.IO.FileInfo(ddlImages.Properties.Items[ddlImages.SelectedIndex].Value.ToString());

            if (CurrentMark != null && CurrentMark.OID > 0) {
                var mark = Settings.Default.Marks.GetMark(CurrentMark.OID);
                mark.ImageName = file.FullName;
                mark.Name = file.Name;
                mark.Description = txtDescription.Text;
                Settings.Default.Save();
            }
            else {
                CurrentMark = new MapMark(file.FullName, txtDescription.Text, file.FullName, this.MapLocation, this.Map);
                Settings.Default.Marks.Add(CurrentMark);
                Settings.Default.Save();
            }
            if (MarkChanged != null) { MarkChanged(this, EventArgs.Empty); }
            this.Close();
        }

        /// <summary>
        /// Datas the bind.
        /// </summary>
        /// <param name="label">The mark.</param>
        public void DataBindMark(MapMark mark) {
            LoadImages();
            CurrentMark = mark;
            txtDescription.Text = mark.Description;
            ddlImages.SelectedIndex = GetImageIndex(CurrentMark.ImageName);
            txtDescription.Focus();
        }

        /// <summary>
        /// Databinds the specified map.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="loc">The loc.</param>
        public void Databind(Controls.MapViewer map, Location loc) {
            LoadImages();
            Map = map;
            MapLocation = loc;
            txtDescription.Focus();
        }

        /// <summary>
        /// Gets the index of the image.
        /// </summary>
        /// <param name="imagepath">The imagepath.</param>
        /// <returns></returns>
        public int GetImageIndex(string imagepath) {
            var paths = Modules.Data.GetImagesPath();
            foreach (var path in paths) {
                if (path.Value == imagepath) { return path.Key; }
            }
            return -1;
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnCancel_Click(object sender, EventArgs e) {
            this.Close();
        }

    }
}