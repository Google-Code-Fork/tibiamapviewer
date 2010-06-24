using System;
using System.Windows.Forms;

namespace Keyrox.Shared.Controls {
    public static class MethodCall {

        /// <summary>
        /// Executes as thread.
        /// </summary>
        /// <param name="method">The method.</param>
        public static void ExecuteAsThread(Callback method) {
            new TemporalThread().Call(method);
        }

        /// <summary>
        /// Executes the safe thread in.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="timeToStart">The time to start.</param>
        public static void ExecuteSafeThreadIn(Callback method, int timeToStart) {
            new TemporalTimer().ExecuteIn(method, timeToStart);
        }

        /// <summary>
        /// Executes as safe thread.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="method">The method.</param>
        public static void ExecuteAsSafeThread(Control control, Callback method) {
            if (control != null) {
                control.BeginInvoke(method);
            }
        }
        
    }
}
