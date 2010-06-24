using System;
namespace KTibiaX.MapViewer.Events {
    public class PercentEventArgs : EventArgs {

        /// <summary>
        /// Initializes a new instance of the <see cref="PercentEventArgs"/> class.
        /// </summary>
        /// <param name="percent">The percent.</param>
        public PercentEventArgs(int percent) {
            Percent = percent;
        }

        /// <summary>
        /// Gets or sets the percent.
        /// </summary>
        /// <value>The percent.</value>
        public int Percent { get; set; }
    }
}
