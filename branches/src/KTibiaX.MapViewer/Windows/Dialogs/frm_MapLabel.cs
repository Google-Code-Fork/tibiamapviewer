using System;
using System.Linq;
using System.Windows.Forms;
using KTibiaX.MapViewer.Controls;
using KTibiaX.MapViewer.Model;
using KTibiaX.MapViewer.Modules;
using KTibiaX.MapViewer.Properties;

namespace KTibiaX.MapViewer.Windows.Dialogs {
    public partial class frm_MapLabel : DevExpress.XtraEditors.XtraForm {
        /// <summary>
        /// Initializes a new instance of the <see cref="frm_MapLabel"/> class.
        /// </summary>
        public frm_MapLabel() {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Load event of the frm_MapLabel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void frm_MapLabel_Load(object sender, EventArgs e) {
            if (ddlThemes.Properties.Items.Count == 0) LoadThemes();
        }

        #region "[rgn] Public Properties   "
        public MapLabel CurrentLabel { get; set; }
        public Location MapLocation { get; set; }
        public Controls.MapViewer Map { get; set; }
        public event EventHandler LabelChanged;
        public frm_ThemeEditor ThemeEditorForm { get; set; }
        #endregion
                
        #region "[rgn] Button Handlers     "
        private void ddlThemes_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e) {
            switch (e.Button.Kind) {
                case DevExpress.XtraEditors.Controls.ButtonPredefines.Plus:
                    ThemeEditorForm = new frm_ThemeEditor();
                    ThemeEditorForm.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmTheme1_FormClosed);
                    ThemeEditorForm.Show();
                    break;
                case DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis:
                    if (ddlThemes.SelectedIndex > -1) {
                        ThemeEditorForm = new frm_ThemeEditor();
                        ThemeEditorForm.DataBind(ddlThemes.GetSelectedValue<LabelTheme>());
                        ThemeEditorForm.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmTheme1_FormClosed);
                        ThemeEditorForm.Show();
                    }
                    break;
                case DevExpress.XtraEditors.Controls.ButtonPredefines.Close:
                    if (ddlThemes.SelectedIndex > -1) {
                        DeleteTheme(ddlThemes.GetSelectedValue<LabelTheme>());
                    }
                    break;
                case DevExpress.XtraEditors.Controls.ButtonPredefines.Combo:
                    ddlThemes.ShowPopup();
                    break;
            }
        }
        private void frmTheme1_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e) {
            if (ThemeEditorForm.CurrentTheme != null && !string.IsNullOrEmpty(ThemeEditorForm.CurrentTheme.Name)) {
                var themes = from theme in Settings.Default.Themes where theme.Name == ThemeEditorForm.CurrentTheme.Name select theme;
                if (themes.Count() > 0) {
                    Settings.Default.Themes[Settings.Default.Themes.IndexOf(themes.First())] = ThemeEditorForm.CurrentTheme;
                }
                else { Settings.Default.Themes.Add(ThemeEditorForm.CurrentTheme); }
                Settings.Default.Save();
                LoadThemes();
                ddlThemes.SelectComboItem(ThemeEditorForm.CurrentTheme.Name);
            }
        }
        private void btnSave_Click(object sender, EventArgs e) {
            Save(false);
        }
        private void btnApply_Click(object sender, EventArgs e) {
            Save(true);
        }
        private void btnCancel_Click(object sender, EventArgs e) {
            Cancel();
        }
        #endregion

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save(bool apply) {
            if (IsValid()) {
                if (CurrentLabel != null && CurrentLabel.OID > 0) {
                    var label = Settings.Default.Labels.GetLabel(CurrentLabel.OID);
                    label.Text = txtName.Text;
                    label.Description = txtDescription.Text;
                    label.Theme = ddlThemes.GetSelectedValue<LabelTheme>();
                    Settings.Default.Save();
                }
                else {
                    CurrentLabel = new MapLabel(txtName.Text, txtDescription.Text, ddlThemes.GetSelectedValue<LabelTheme>(), MapLocation, Map);
                    Settings.Default.Labels.Add(CurrentLabel);
                    Settings.Default.Save();
                }
                if (LabelChanged != null) { LabelChanged(this, EventArgs.Empty); }
                if (!apply) { Close(); }
            }
        }

        /// <summary>
        /// Datas the bind.
        /// </summary>
        /// <param name="label">The label.</param>
        public void DataBindLabel(MapLabel label) {
            LoadThemes();
            CurrentLabel = label;
            txtName.Text = label.Text;
            txtDescription.Text = label.Description;
            ddlThemes.SelectComboItem(label.Theme.Name);
            txtLocation.Text = label.MapLocation.ToFormString();
        }

        /// <summary>
        /// Databinds the specified map.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="loc">The loc.</param>
        public void Databind(Controls.MapViewer map, Location loc) {
            LoadThemes();
            Map = map;
            MapLocation = loc;
            txtLocation.Text = loc.ToFormString();
            txtName.Focus();
        }

        /// <summary>
        /// Cancels this instance.
        /// </summary>
        public void Cancel() {
            Close();
        }

        /// <summary>
        /// Loads the themes.
        /// </summary>
        private void LoadThemes() {
            ddlThemes.Clear();
            foreach (var theme in Settings.Default.Themes) {
                ddlThemes.AddNewItem(theme.Name, theme);
            }
            if (ddlThemes.Properties.Items.Count > 0) ddlThemes.SelectedIndex = 0;
        }

        /// <summary>
        /// Deletes the theme.
        /// </summary>
        /// <param name="theme">The theme.</param>
        private void DeleteTheme(LabelTheme theme) {
            var res = MessageBox.Show("Are you shure?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes) {
                Settings.Default.Themes.RemoveAt(Settings.Default.Themes.IndexOf(theme));
                Settings.Default.Save();
                LoadThemes();
            }
        }

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValid() {
            if (txtName.Text.Length < 3) { dxErrorProvider1.SetError(txtName, "Invalid Name!"); return false; }
            else { dxErrorProvider1.SetError(txtName, ""); }

            if (ddlThemes.SelectedIndex < 0) { dxErrorProvider1.SetError(ddlThemes, "Invalid Theme!"); return false; }
            else { dxErrorProvider1.SetError(ddlThemes, ""); }
            return true;
        }

    }
}