using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars.Ribbon.ViewInfo;

namespace KTibiaX.MapViewer.Controls {

    public class CustomRibbon : RibbonControl {
        protected override RibbonViewInfo CreateViewInfo() {
            return new CustomRibbonViewInfo(this);
        }
    }

    public class CustomRibbonViewInfo : RibbonViewInfo {
        public CustomRibbonViewInfo(RibbonControl ribbon)
            : base(ribbon) {
        }
        protected override int LargeButtonTextLinesCount {
            get { return 1; }
        }
        public override bool IsAllowDisplayRibbon {
            get { return true; }
        }
    }

}
