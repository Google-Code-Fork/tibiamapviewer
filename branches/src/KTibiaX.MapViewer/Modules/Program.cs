using System;
using System.Windows.Forms;
using KTibiaX.MapViewer.Modules;
using KTibiaX.MapViewer.Windows.Graphics;
using KTibiaX.MapViewer.Collections;
using KTibiaX.MapViewer.Model;
using System.Drawing;
using Keyrox.Shared.Objects;

namespace KTibiaX.MapViewer {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);

            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.UserSkins.OfficeSkins.Register();
            Data.Initialize();

            Application.Run(new frm_MapViewer());
        }

        private static void Application_ApplicationExit(object sender, EventArgs e) {
            Data.PersistValues();
        }
    }
}
