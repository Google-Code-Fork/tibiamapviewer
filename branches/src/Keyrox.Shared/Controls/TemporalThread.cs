using System;
using System.ComponentModel;
using System.Threading;

namespace Keyrox.Shared.Controls {
    public sealed class TemporalThread : Component {
        
        /// <summary>
        /// Initializes a new instance of the TemporalThread class.
        /// </summary>
        public TemporalThread() {
        }

        #region "[rgn] Types and Properties "
        private Thread ThInternal { get; set; }
        public Callback ExternalMethod { get; set; }
        public event EventHandler CallFinished;
        #endregion

        /// <summary>
        /// Calls the specified method.
        /// </summary>
        /// <param name="method">The method.</param>
        public void Call(Callback method) {
            ExternalMethod = method;
            ThInternal = new Thread(InternalCall);
            ThInternal.IsBackground = true;
            ThInternal.Start();
        }
        
        /// <summary>
        /// Internal Call.
        /// </summary>
        private void InternalCall() {
            if (ExternalMethod != null) {
                ExternalMethod.BeginInvoke(new AsyncCallback(EndCall), ExternalMethod);
            }
        }
        
        /// <summary>
        /// Occours when the call ends.
        /// </summary>
        /// <param name="result">The result.</param>
        private void EndCall(IAsyncResult result) {
            if (CallFinished != null) CallFinished(this, EventArgs.Empty);
        }        
    }
}
