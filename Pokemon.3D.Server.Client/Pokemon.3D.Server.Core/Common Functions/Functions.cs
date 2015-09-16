using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Media;

namespace Global
{
    /// <summary>
    /// Class containing common functions
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// Represents a newline character for print and display functions.
        /// </summary>
        public static readonly string vbNewLine = Environment.NewLine;

        //private static bool IsPortOpen = false;

        /// <summary>
        /// Play the system sound for error input.
        /// </summary>
        private static void PlaySystemSound()
        {
            SystemSounds.Asterisk.Play();
        }

        /// <summary>
        /// Catch Ex Exception and create a crash log.
        /// </summary>
        /// <param name="ex">Ex Exception.</param>
        public static void CatchError(this Exception ex)
        {
            PlaySystemSound();

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
Environment.Is64BitOperatingSystem ? "64 Bit" : "32 Bit",
DateTime.Now.ToString(),
System.Globalization.CultureInfo.CurrentCulture.EnglishName.ToString(),
Math.Round((double)(My.Computer.Info.AvailablePhysicalMemory / 1073741824), 2).ToString() + " GB / " + Math.Round((double)(My.Computer.Info.TotalPhysicalMemory / 1073741824), 2).ToString() + " GB",
Environment.ProcessorCount.ToString(),
ex.Message,
ex.InnerException == null ? "Nothing" : ex.InnerException.Message,
string.IsNullOrWhiteSpace(ex.HelpLink) ? "Nothing" : ex.HelpLink,
ex.Source,
ex.InnerException == null ? ex.StackTrace : ex.InnerException.StackTrace + vbNewLine + ex.StackTrace);

            if (!Directory.Exists(Settings.ApplicationDirectory + "\\CrashLogs"))
            {
                Directory.CreateDirectory(Settings.ApplicationDirectory + "\\CrashLogs");
            }

            DateTime ErrorTime = DateTime.Now;
            int RandomIndetifier = MathHelper.Random(0, int.MaxValue);

            try
            {
                File.WriteAllText(Settings.ApplicationDirectory + "\\CrashLogs\\Crash_" + ErrorTime.Day.ToString() + "-" + ErrorTime.Month.ToString() + "-" + ErrorTime.Year.ToString() + "_" + ErrorTime.Hour.ToString() + "." + ErrorTime.Minute.ToString() + "." + ErrorTime.Second.ToString() + "." + RandomIndetifier + ".dat", ErrorLog, Encoding.Unicode);
                QueueMessage.Add(ex.Message + vbNewLine + "Error Log saved at: " + Settings.ApplicationDirectory + "\\CrashLogs\\Crash_" + ErrorTime.Day.ToString() + "-" + ErrorTime.Month.ToString() + "-" + ErrorTime.Year.ToString() + "_" + ErrorTime.Hour.ToString() + "." + ErrorTime.Minute.ToString() + "." + ErrorTime.Second.ToString() + "." + RandomIndetifier + ".dat", MessageEventArgs.LogType.Warning);
            }
            catch (Exception exc)
            {
                QueueMessage.Add(exc.Message, MessageEventArgs.LogType.Warning);
            }
        }

        /// <summary>
        /// Count the number of index after spliting "|" in the full string.
        /// </summary>
        /// <param name="fullString">The full string to count the number of index.</param>
        /// <returns></returns>
        public static int SplitCount(this string fullString)
        {
            return fullString.Contains("|") ? fullString.Split("|".ToCharArray()).Length : 1;
        }

        /// <summary>
        /// Count the number of index after spliting the seperator in the full string.
        /// </summary>
        /// <param name="fullString">The full string to count the number of index.</param>
        /// <param name="seperator">The seperator to split the string.</param>
        public static int SplitCount(this string fullString, string seperator)
        {
            return fullString.Contains(seperator) ? fullString.Split(seperator.ToCharArray()).Length : 1;
        }

        /// <summary>
        /// Get the nth index of the splited string after spliting "|" in the full string.
        /// </summary>
        /// <param name="fullString">The full string to be splited.</param>
        /// <param name="valueIndex">The index to return.</param>
        public static string GetSplit(this string fullString, int valueIndex)
        {
            if (fullString.SplitCount() == 1)
            {
                return fullString;
            }
            else if (fullString.SplitCount() < valueIndex)
            {
                return fullString.Split("|".ToCharArray())[valueIndex];
            }
            else
            {
                return fullString.Split("|".ToCharArray())[fullString.SplitCount() - 1];
            }
        }

        /// <summary>
        /// Get the nth index of the splited string after spliting the seperator in the full string.
        /// </summary>
        /// <param name="fullString">The full string to be splited.</param>
        /// <param name="valueIndex">The index to return.</param>
        /// <param name="seperator">The seperator.</param>
        /// <returns></returns>
        public static string GetSplit(this string fullString, int valueIndex, string seperator)
        {
            if (fullString.SplitCount(seperator) == 1)
            {
                return fullString;
            }
            else if (fullString.SplitCount(seperator) < valueIndex)
            {
                return fullString.Split(seperator.ToCharArray())[valueIndex];
            }
            else
            {
                return fullString.Split(seperator.ToCharArray())[fullString.SplitCount() - 1];
            }
        }

        /// <summary>
        /// Get the public IP of the hosting computer.
        /// </summary>
        public static string GetPublicIP()
        {
            try
            {
                using (WebClient Client = new WebClient())
                {
                    return Client.DownloadString("https://api.ipify.org");
                }
            }
            catch (Exception ex)
            {
                ex.CatchError();
                return null;
            }
        }

        /// <summary>
        /// Get the private IP of the hosting computer.
        /// </summary>
        public static string GetPrivateIP()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress address in host.AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork )
                {
                    return address.ToString();
                }
            }
            return null;
        }

        /// <summary>
        /// Check if the port is open.
        /// </summary>
        public static bool CheckPortOpen()
        {
            try
            {
                using (TcpClient Client = new TcpClient())
                {
                    if (Client.ConnectAsync(Settings._IPAddress, Settings.Port).Wait(1000))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}