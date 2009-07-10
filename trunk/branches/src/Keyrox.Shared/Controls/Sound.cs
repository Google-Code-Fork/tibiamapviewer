using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Resources;
using System.IO;

namespace Keyrox.Shared {
    /// <summary>
    /// Sound manipulation class.
    /// </summary>
    public static class Sound {

        /// <summary>
        /// Plays the sound.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="hmod">The hmod.</param>
        /// <param name="flags">The flags.</param>
        /// <returns></returns>
        [DllImport("winmm.dll")]
        private static extern int PlaySound(string name, int hmod, int flags);

        /// <summary>
        /// Operation flag type.
        /// </summary>
        private const int SND_FILENAME = 0x20000;

        /// <summary>
        /// Default Wav Path.
        /// </summary>
        public static string DefaultWavPath = @"Sounds";

        /// <summary>
        /// Plays the specified wav path.
        /// </summary>
        /// <param name="wavPath">The wav path.</param>
        public static void Play(string wavPath) {
            var thPlay = new System.Threading.Thread(THPlay);
            thPlay.IsBackground = true;
            threadWavPath = wavPath;
            thPlay.Start();
        }

        private static string threadWavPath;
        /// <summary>
        /// THs the play.
        /// </summary>
        private static void THPlay() {
            if (!string.IsNullOrEmpty(threadWavPath)) {
                PlaySound(threadWavPath, 0, SND_FILENAME);
                threadWavPath = string.Empty;
            }
        }
    }
}
