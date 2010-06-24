using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using KTibiaX.MapViewer.Model;
using KTibiaX.MapViewer.Modules;
using KTibiaX.MapViewer.Properties;

namespace KTibiaX.MapViewer.Windows.Dialogs {
    public partial class frm_ThemeEditor : DevExpress.XtraEditors.XtraForm {
        /// <summary>
        /// Initializes a new instance of the <see cref="frm_ThemeEditor"/> class.
        /// </summary>
        public frm_ThemeEditor() {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Load event of the frm_ThemeEditor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void frm_ThemeEditor_Load(object sender, EventArgs e) {
            if (CurrentTheme == null) SetNew();
            previewPanel1.CurrentImage = Resources.preview_back;
        }

        #region "[rgn] Public Properties    "
        public LabelTheme CurrentTheme { get; set; }
        public Font CurrentFont { get; set; }
        #endregion

        #region "[rgn] Form Control Handler "
        private void txtName_EditValueChanged(object sender, EventArgs e) {
            if (CurrentTheme == null) return;
            CurrentTheme.Name = txtName.Text;
            previewPanel1.DrawLabel(CurrentTheme);
        }
        private void ddlFont_EditValueChanged(object sender, EventArgs e) {
            if (CurrentTheme == null) return;
            CurrentTheme.Font = CurrentTheme.Font.SetFamily(new FontFamily(ddlFont.SelectedItem.ToString()));
            previewPanel1.DrawLabel(CurrentTheme);
        }
        private void txtSize_EditValueChanged(object sender, EventArgs e) {
            if (CurrentTheme == null) return;
            CurrentTheme.Font = CurrentTheme.Font.SetSize((float)Convert.ToInt32(txtSize.EditValue));
            previewPanel1.DrawLabel(CurrentTheme);
        }
        private void txtShadowSize_EditValueChanged(object sender, EventArgs e) {
            if (CurrentTheme == null) return;
            CurrentTheme.ShadowSize = Convert.ToInt32(txtShadowSize.EditValue);
            previewPanel1.DrawLabel(CurrentTheme);
        }
        private void ckBold_CheckedChanged(object sender, EventArgs e) {
            if (CurrentTheme == null) return;
            CurrentTheme.Font = CurrentTheme.Font.SetStyle(GetSelectedStyles());
            previewPanel1.DrawLabel(CurrentTheme);
        }
        private void ckItalic_CheckedChanged(object sender, EventArgs e) {
            if (CurrentTheme == null) return;
            CurrentTheme.Font = CurrentTheme.Font.SetStyle(GetSelectedStyles());
            previewPanel1.DrawLabel(CurrentTheme);
        }
        private void ckUnderline_CheckedChanged(object sender, EventArgs e) {
            if (CurrentTheme == null) return;
            CurrentTheme.Font = CurrentTheme.Font.SetStyle(GetSelectedStyles());
            previewPanel1.DrawLabel(CurrentTheme);
        }
        private void ckStrikeout_CheckedChanged(object sender, EventArgs e) {
            if (CurrentTheme == null) return;
            CurrentTheme.Font = CurrentTheme.Font.SetStyle(GetSelectedStyles());
            previewPanel1.DrawLabel(CurrentTheme);
        }
        private void txtForeColor_EditValueChanged(object sender, EventArgs e) {
            if (CurrentTheme == null) return;
            CurrentTheme.ForeColor = txtForeColor.Color;
            previewPanel1.DrawLabel(CurrentTheme);
        }
        private void txtShadowColor_EditValueChanged(object sender, EventArgs e) {
            if (CurrentTheme == null) return;
            CurrentTheme.ShadowColor = txtShadowColor.Color;
            previewPanel1.DrawLabel(CurrentTheme);
        }
        private void btnSave_Click(object sender, EventArgs e) {
            Close();
        }
        private void btnMenuSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            Close();
        }
        private void btnMenuCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            CurrentTheme = null;
            Close();
        }
        #endregion

        /// <summary>
        /// Datas the bind.
        /// </summary>
        /// <param name="theme">The theme.</param>
        public void DataBind(LabelTheme theme) {
            if (theme != null) {
                txtName.Text = theme.Name;
                ddlFont.SelectComboItem(theme.Font.FontFamily.Name);
                txtSize.Text = theme.Font.Size.ToString();
                txtShadowSize.Text = theme.ShadowSize.ToString();
                txtForeColor.Color = theme.ForeColor;
                txtShadowColor.Color = theme.ShadowColor;
                ckBold.Checked = (theme.Font.Style & FontStyle.Bold) > 0;
                ckItalic.Checked = (theme.Font.Style & FontStyle.Italic) > 0;
                ckUnderline.Checked = (theme.Font.Style & FontStyle.Underline) > 0;
                ckStrikeout.Checked = (theme.Font.Style & FontStyle.Strikeout) > 0;
                CurrentTheme = theme;
                previewPanel1.DrawLabel(CurrentTheme);
            }
        }

        /// <summary>
        /// Sets the new.
        /// </summary>
        public void SetNew() {
            CurrentTheme = new LabelTheme();
            CurrentTheme.Font = new Font("Arial", 20, FontStyle.Bold, GraphicsUnit.Point);
            CurrentTheme.Name = "New Theme";
            CurrentTheme.ShadowSize = 2;
            CurrentTheme.ForeColor = Color.White;
            CurrentTheme.ShadowColor = Color.Black;
            DataBind(CurrentTheme);
        }

        /// <summary>
        /// Gets the selected styles.
        /// </summary>
        /// <returns></returns>
        private FontStyle GetSelectedStyles() {
            FontStyle res = FontStyle.Regular;
            if (ckBold.Checked) { res = FontStyle.Bold; }
            if (ckItalic.Checked) { if (res != FontStyle.Regular) { res |= FontStyle.Italic; } else { res = FontStyle.Italic; } }
            if (ckUnderline.Checked) { if (res != FontStyle.Regular) { res |= FontStyle.Underline; } else { res = FontStyle.Underline; } }
            if (ckStrikeout.Checked) { if (res != FontStyle.Regular) { res |= FontStyle.Strikeout; } else { res = FontStyle.Strikeout; } }
            return res;
        }

    }
}