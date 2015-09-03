using System;
using System.IO;

namespace Pokémon_3D_Server_Core
{
    /// <summary>
    /// Public Module Functions
    /// </summary>
    public static class Functions
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

            string ErrorLog = string.Format(@"[CODE]
Pokémon 3D Server Client Crash Log v {0}
--------------------------------------------------

System specifications:

Operating system: {1} [{2}]
Core architecture: {3}
System time: {4}
System language: {5}
Physical memory: {6}
Logical processors: {7}

--------------------------------------------------
            
Error information:

Message: {8}
InnerException: {9}
HelpLink: {10}
Source: {11}

--------------------------------------------------

CallStack:

{12}

--------------------------------------------------

You should report this error if it is reproduceable or you could not solve it by yourself.

Go To: http://pokemon3d.net/forum/threads/8234/ to report this crash there.
[/CODE]",
Environment.Version.ToString(),
My.Computer.Info.OSFullName,
My.Computer.Info.OSVersion,
CoreArchitecture,
DateTime.Now.ToString(),
System.Globalization.CultureInfo.CurrentCulture.EnglishName.ToString(),
Environment.ProcessorCount.ToString(),
ex.Message,
InnerException,
HelpLink,
ex.Source,
StackTrace);
        
            if (!Directory.Exists(Environment.CurrentDirectory + "\\CrashLogs"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\CrashLogs");
            }
        }
    }
}
