using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Keyrox.Shared.Controls {
    public class TemporalTimer : Component {

        /// <summary>
        /// Initializes a new instance of the <see cref="TemporalTimer"/> class.
        /// </summary>
        public TemporalTimer() { }

        /// <summary>
        /// Gets or sets the timer.
        /// </summary>
        /// <value>The timer.</value>
        private Timer Timer { get; set; }

        #region "[rgn] Component Types "
        public enum TimerState { Stoped = 0, Started = 1 }
        #endregion
        
        /// <summary>
        /// Gets or sets the external method to call.
        /// </summary>
        /// <value>The external method to call.</value>
        public Callback ExternalMethodToCall { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public TimerState State { get; set; }
        
        /// <summary>
        /// Executes the in.
        /// </summary>
        /// <param name="timeToStart">The time to start.</param>
        public void ExecuteIn(int timeToStart) {
            if (ExternalMethodToCall == null) throw new ArgumentException("The External Method cannot be null!");
            if (State == TimerState.Stoped) {
                Timer = this.Container != null ? new Timer(this.Container) : new Timer();
                Timer.Interval = timeToStart;
                Timer.Tick += new EventHandler(Timer_Tick);
                State = TimerState.Started;
                Timer.Start();
            }
        }

        /// <summary>
        /// Executes the in.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="timeToStart">The time to start.</param>
        public void ExecuteIn(Callback method, int timeToStart) {
            this.ExternalMethodToCall = method;
            ExecuteIn(timeToStart);
        }
        
        /// <summary>
        /// Handles the Tick event of the Timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Timer_Tick(object sender, EventArgs e) {
            ExternalMethodToCall.Invoke();
            Timer.Stop();
            State = TimerState.Stoped;
        }
    }
}
