using System;
using System.IO;
using System.Windows.Forms;
using Keyrox.Shared;

namespace KTibiaX.MapViewer.Controls {
    public static class Cursors {

        /// <summary>
        /// Gets the cursor directory.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        private static string GetCursorDirectory(string fileName) {
            return Path.Combine(Environment.CurrentDirectory, "Resources\\Cursors\\" + fileName);
        }

        #region "[rgn] Private Variables   "
        private static Cursor busy;
        private static Cursor normal;
        private static Cursor hand;
        private static Cursor help;
        private static Cursor move;
        private static Cursor select;
        private static Cursor text;
        private static Cursor unavailable;
        private static Cursor working;
        #endregion
        
        /// <summary>
        /// Gets the normal cursor.
        /// </summary>
        /// <value>The normal.</value>
        public static Cursor Normal {
            get {
                if (normal == null) normal = new Cursor(WindowsAPI.LoadCursorFromFile(GetCursorDirectory("normal.cur")));
                return normal;
            }
        }

        /// <summary>
        /// Gets the hand cursor.
        /// </summary>
        /// <value>The hand.</value>
        public static Cursor Hand {
            get {
                if (hand == null) hand = new Cursor(WindowsAPI.LoadCursorFromFile(GetCursorDirectory("hand.cur")));
                return hand;
            }
        }

        /// <summary>
        /// Gets the help cursor.
        /// </summary>
        /// <value>The help.</value>
        public static Cursor Help {
            get {
                if (help == null) help = new Cursor(WindowsAPI.LoadCursorFromFile(GetCursorDirectory("help.cur")));
                return help;
            }
        }

        /// <summary>
        /// Gets the move cursor.
        /// </summary>
        /// <value>The move.</value>
        public static Cursor Move {
            get {
                if (move == null) move = new Cursor(WindowsAPI.LoadCursorFromFile(GetCursorDirectory("move.cur")));
                return move;
            }
        }

        /// <summary>
        /// Gets the select cursor.
        /// </summary>
        /// <value>The select.</value>
        public static Cursor Select {
            get {
                if (select == null) select = new Cursor(WindowsAPI.LoadCursorFromFile(GetCursorDirectory("select.cur")));
                return select;
            }
        }

        /// <summary>
        /// Gets the text cursor.
        /// </summary>
        /// <value>The text.</value>
        public static Cursor Text {
            get {
                if (text == null) text = new Cursor(WindowsAPI.LoadCursorFromFile(GetCursorDirectory("text.cur")));
                return text;
            }
        }

        /// <summary>
        /// Gets the unavailable cursor.
        /// </summary>
        /// <value>The unavailable.</value>
        public static Cursor Unavailable {
            get {
                if (unavailable == null) unavailable = new Cursor(WindowsAPI.LoadCursorFromFile(GetCursorDirectory("unavailable.cur")));
                return unavailable;
            }
        }


        /// <summary>
        /// Gets the working cursor.
        /// </summary>
        /// <value>The working.</value>
        public static Cursor Working {
            get {
                if (working == null) working = new Cursor(WindowsAPI.LoadCursorFromFile(GetCursorDirectory("working.cur")));
                return working;
            }
        }
        
        /// <summary>
        /// Gets the busy.
        /// </summary>
        /// <value>The busy.</value>
        public static Cursor Busy {
            get {
                if (busy == null) busy = new Cursor(WindowsAPI.LoadCursorFromFile(GetCursorDirectory("busy.ani")));
                return busy;
            }
        }

    }
}
