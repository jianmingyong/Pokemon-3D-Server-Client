using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using Pokemon_3D_Server_Core.Server_Client_Listener.Events;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Modules
{
    /// <summary>
    /// Class containing commonly used functions.
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// Gets the newline string defined for this environment.
        /// </summary>
        public static readonly string vbNewLine = Environment.NewLine;

        /// <summary>
        /// Catch Ex Exception and create a crash log.
        /// </summary>
        /// <param name="ex">Ex Exception.</param>
        public static void CatchError(this Exception ex)
        {
            try
            {
                SystemSounds.Asterisk.Play();

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
Runtime language: {13}

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

Go To: http://pokemon3d.net/forum/threads/8234/ or http://www.aggressivegaming.org/pokemon/forums/bug-reports.204/ to report this crash.
[/CODE]",
                Core.Setting.ApplicationVersion,
                My.Computer.Info.OSFullName,
                My.Computer.Info.OSVersion,
                Environment.Is64BitOperatingSystem ? "64 Bit" : "32 Bit",
                DateTime.Now.ToString(),
                CultureInfo.CurrentCulture.EnglishName.ToString(),
                string.Format("{0} GB / {1} GB", Math.Round((double)My.Computer.Info.AvailablePhysicalMemory / 1073741824, 2).ToString(), Math.Round((double)My.Computer.Info.TotalPhysicalMemory / 1073741824, 2).ToString()),
                Environment.ProcessorCount.ToString(),
                ex.Message,
                ex.InnerException == null ? "Nothing" : ex.InnerException.Message,
                string.IsNullOrWhiteSpace(ex.HelpLink) ? "Nothing" : ex.HelpLink,
                ex.Source,
                ex.InnerException == null ? ex.StackTrace : ex.InnerException.StackTrace + vbNewLine + ex.StackTrace,
                Type.GetType("Mono.Runtime") != null ? "Mono" : ".Net"
                );

                if (!Directory.Exists(Core.Setting.ApplicationDirectory + "\\CrashLogs"))
                {
                    Directory.CreateDirectory(Core.Setting.ApplicationDirectory + "\\CrashLogs");
                }

                DateTime ErrorTime = DateTime.Now;
                int RandomIndetifier = MathHelper.Random(0, int.MaxValue);

                File.WriteAllText(Core.Setting.ApplicationDirectory + "\\CrashLogs\\Crash_" + ErrorTime.Day.ToString() + "-" + ErrorTime.Month.ToString() + "-" + ErrorTime.Year.ToString() + "_" + ErrorTime.Hour.ToString() + "." + ErrorTime.Minute.ToString() + "." + ErrorTime.Second.ToString() + "." + RandomIndetifier.ToString("0000000000") + ".dat", ErrorLog, Encoding.UTF8);
                Core.Logger.Log(ex.Message + vbNewLine + "Error Log saved at: " + Core.Setting.ApplicationDirectory + "\\CrashLogs\\Crash_" + ErrorTime.Day.ToString() + "-" + ErrorTime.Month.ToString() + "-" + ErrorTime.Year.ToString() + "_" + ErrorTime.Hour.ToString() + "." + ErrorTime.Minute.ToString() + "." + ErrorTime.Second.ToString() + "." + RandomIndetifier + ".dat", Logger.LogTypes.Warning);
            }
            catch (Exception exc)
            {
                Core.Logger.Log(exc.Message, Logger.LogTypes.Warning);
            }
        }

        /// <summary>
        /// Count the number of index after spliting "|" in the full string.
        /// </summary>
        /// <param name="fullString">The full string to count the number of index.</param>
        public static int SplitCount(this string fullString)
        {
            return fullString.Contains("|") ? fullString.Split('|').Length : 1;
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
            else if (valueIndex < fullString.SplitCount())
            {
                return fullString.Split('|')[valueIndex];
            }
            else
            {
                return fullString.Split('|')[fullString.SplitCount() - 1];
            }
        }

        /// <summary>
        /// Get the nth index of the splited string after spliting the seperator in the full string.
        /// </summary>
        /// <param name="fullString">The full string to be splited.</param>
        /// <param name="valueIndex">The index to return.</param>
        /// <param name="seperator">The seperator.</param>
        public static string GetSplit(this string fullString, int valueIndex, string seperator)
        {
            if (fullString.SplitCount(seperator) == 1)
            {
                return fullString;
            }
            else if (valueIndex < fullString.SplitCount(seperator))
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
            using (WebClient Client = new WebClient())
            {
                try
                {
                    return Client.DownloadString("https://api.ipify.org");
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Get the private IP of the hosting computer.
        /// </summary>
        public static string GetPrivateIP()
        {
            try
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

                foreach (IPAddress address in host.AddressList)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return address.ToString();
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Check if the port is open.
        /// </summary>
        /// <param name="Port">Port to check.</param>
        public static bool CheckPortOpen(int Port)
        {
            using (TcpClient Client = new TcpClient())
            {
                try
                {
                    if (Client.ConnectAsync(GetPublicIP(), Port).Wait(5000))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Generate Md5 Checksum from string.
        /// </summary>
        /// <param name="Value">String to compute.</param>
        /// <param name="FilePath">Is it a file?</param>
        public static string Md5HashGenerator(this string Value, bool FilePath = false)
        {
            MD5 md5 = MD5.Create();

            if (FilePath)
            {
                FileStream FileStream = File.OpenRead(Value);
                FileStream.Position = 0;
                byte[] hash = md5.ComputeHash(FileStream);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }

                return sb.ToString();
            }
            else
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(Value);
                byte[] hash = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Generate SHA1 Checksum from string.
        /// </summary>
        /// <param name="Value">String to compute.</param>
        public static string SHA1HashGenerator(this string Value)
        {
            SHA1 sha1 = SHA1.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(Value);
            byte[] hash = sha1.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Generate SHA256 Checksum from string.
        /// </summary>
        /// <param name="Value">String to compute.</param>
        public static string SHA256HashGenerator(this string Value)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(Value);
            byte[] hash = sha256.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Starts a process resource by specifying the name of a document or application
        /// file and associates the resource with a new <see cref="Process"/> component.
        /// </summary>
        /// <param name="File">The name of a document or application file to run in the process.</param>
        /// <param name="Argument">Command-line arguments to pass when starting the process.</param>
        /// <param name="Close">Close the current process?</param>
        public static void Run(this string File, string Argument = null, bool Close = false)
        {
            try
            {
                if (Argument == null)
                {
                    Process.Start(File);
                }
                else
                {
                    Process.Start(File, Argument);
                }

                if (Close)
                {
                    ClientEvent.Invoke(ClientEvent.Types.Stop);
                }
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
 