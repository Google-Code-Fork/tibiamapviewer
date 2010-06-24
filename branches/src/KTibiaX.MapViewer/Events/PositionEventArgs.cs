using System;
using System.Drawing;
namespace KTibiaX.Analyzer.Events {
    public class PositionEventArgs : EventArgs {

        /// <summary>
        /// Initializes a new instance of the <see cref="PositionEventArgs"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        public PositionEventArgs(Point position) {
            Position = position;
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        public Point Position { get; set; }
    }
}
