using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokémon_3D_Server_Core
{
    // Public Module Functions
    static class Functions
    {
        /// <summary>
        /// Represents a newline character for print and display functions.
        /// </summary>
        public static string vbNewLine
        {
            get
            {
                return Environment.NewLine;
            }
        }

        /// <summary>
        /// Play the system sound for error input.
        /// </summary>
        private static void PlaySystemSound()
        {
            System.Media.SystemSounds.Asterisk.Play();
        }

        /// <summary>
        /// Catch Ex Exception and create a crash log.
        /// </summary>
        /// <param name="ex"></param>
        public static void CatchError(this Exception ex)
        {
            PlaySystemSound();

            string CoreArchitecture = Environment.Is64BitOperatingSystem ? "64 Bit" : "32 Bit";
            string HelpLink = string.IsNullOrWhiteSpace(ex.HelpLink) ? "No helplink available." : ex.HelpLink;
            string InnerException = string.IsNullOrWhiteSpace(ex.InnerException.Message) ? "Nothing" : ex.InnerException.Message;
            string StackTrace = string.IsNullOrWhiteSpace(ex.InnerException.StackTrace) ? ex.StackTrace : ex.InnerException.StackTrace + vbNewLine + ex.StackTrace;

            string ErrorLog = @"[CODE]
Pokémon 3D Server Client Crash Log v" + Environment.Version.ToString() + @"
--------------------------------------------------

System specifications:

Operating system: " + My.Computer.Info.OSFullName + " [" + My.Computer.Info.OSVersion + @"]
Core architecture: " + CoreArchitecture + @"
System time: ";

        }
    }
}
