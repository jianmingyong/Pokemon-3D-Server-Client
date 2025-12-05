using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using Pokemon_3D_Server_Core.Server_Client_Listener.Events;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;

namespace Pokemon_3D_Server_Core.Shared.jianmingyong.Modules
{
    /// <summary>
    ///     Class containing commonly used functions.
    /// </summary>
    public static class Functions
    {
        /// <summary>
        ///     Catch Ex Exception and create a crash log.
        /// </summary>
        /// <param name="ex">Ex Exception.</param>
        public static void CatchError(this Exception ex)
        {
            try
            {
                var ErrorLog = $@"
```
Pokémon 3D Server Client Crash Log v {Core.Setting.ApplicationVersion}
--------------------------------------------------

System specifications:

Operating system: {Environment.OSVersion}
Core architecture: {(Environment.Is64BitOperatingSystem ? "64 Bit" : "32 Bit")}
System time: {DateTime.Now.ToString(CultureInfo.CurrentCulture)}
System language: {CultureInfo.CurrentCulture.EnglishName}
Logical processors: {Environment.ProcessorCount}

--------------------------------------------------

Error information:

Message: {ex.Message}
InnerException: {(ex.InnerException == null ? "Nothing" : ex.InnerException.Message)}
HelpLink: {(string.IsNullOrWhiteSpace(ex.HelpLink) ? "Nothing" : ex.HelpLink)}
Source: {ex.Source}

--------------------------------------------------

CallStack:

{(ex.InnerException == null ? ex.StackTrace : ex.InnerException.StackTrace + Environment.NewLine + ex.StackTrace)}

--------------------------------------------------

You should report this error if it is reproduceable or you could not solve it by yourself.
```
";

                if (!Directory.Exists(Core.Setting.ApplicationDirectory + "\\CrashLogs"))
                {
                    Directory.CreateDirectory(Core.Setting.ApplicationDirectory + "\\CrashLogs");
                }

                var ErrorTime = DateTime.Now;
                var RandomIndetifier = MathHelper.Random(0, int.MaxValue);

                File.WriteAllText($@"{Core.Setting.ApplicationDirectory}\CrashLogs\Crash_{ErrorTime.Day}-{ErrorTime.Month}-{ErrorTime.Year}_{ErrorTime.Hour}.{ErrorTime.Minute}.{ErrorTime.Second}.{RandomIndetifier:0000000000}.dat", ErrorLog, Encoding.UTF8);
                Core.Logger.Log(ex.Message + Environment.NewLine + "Error Log saved at: " + Core.Setting.ApplicationDirectory + "\\CrashLogs\\Crash_" + ErrorTime.Day + "-" + ErrorTime.Month + "-" + ErrorTime.Year + "_" + ErrorTime.Hour + "." + ErrorTime.Minute + "." + ErrorTime.Second + "." + RandomIndetifier + ".dat", Logger.LogTypes.Warning);
            }
            catch (Exception exc)
            {
                Core.Logger.Log(exc.Message, Logger.LogTypes.Warning);
            }
        }

        /// <summary>
        ///     Count the number of index after spliting "|" in the full string.
        /// </summary>
        /// <param name="fullString">The full string to count the number of index.</param>
        public static int SplitCount(this string fullString)
        {
            return fullString.Contains("|") ? fullString.Split('|').Length : 1;
        }

        /// <summary>
        ///     Count the number of index after spliting the seperator in the full string.
        /// </summary>
        /// <param name="fullString">The full string to count the number of index.</param>
        /// <param name="separator">The seperator to split the string.</param>
        public static int SplitCount(this string fullString, string separator)
        {
            return fullString.Contains(separator) ? fullString.Split(separator.ToCharArray()).Length : 1;
        }

        /// <summary>
        ///     Get the nth index of the splited string after spliting "|" in the full string.
        /// </summary>
        /// <param name="fullString">The full string to be splited.</param>
        /// <param name="valueIndex">The index to return.</param>
        public static string GetSplit(this string fullString, int valueIndex)
        {
            if (fullString.SplitCount() == 1)
            {
                return fullString;
            }

            if (valueIndex < fullString.SplitCount())
            {
                return fullString.Split('|')[valueIndex];
            }

            return fullString.Split('|')[fullString.SplitCount() - 1];
        }

        /// <summary>
        ///     Get the nth index of the splited string after spliting the seperator in the full string.
        /// </summary>
        /// <param name="fullString">The full string to be splited.</param>
        /// <param name="valueIndex">The index to return.</param>
        /// <param name="separator">The seperator.</param>
        public static string GetSplit(this string fullString, int valueIndex, string separator)
        {
            if (fullString.SplitCount(separator) == 1)
            {
                return fullString;
            }

            if (valueIndex < fullString.SplitCount(separator))
            {
                return fullString.Split(separator.ToCharArray())[valueIndex];
            }

            return fullString.Split(separator.ToCharArray())[fullString.SplitCount() - 1];
        }

        /// <summary>
        ///     Get the public IP of the hosting computer.
        /// </summary>
        public static string GetPublicIP()
        {
            using (var Client = new WebClient())
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
        ///     Get the private IP of the hosting computer.
        /// </summary>
        public static string GetPrivateIP()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());

                foreach (var address in host.AddressList)
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
        ///     Check if the port is open.
        /// </summary>
        /// <param name="Port">Port to check.</param>
        public static bool CheckPortOpen(int Port)
        {
            using (var Client = new TcpClient())
            {
                try
                {
                    if (Client.ConnectAsync(Core.Setting.IPAddress, Port).Wait(5000))
                    {
                        return true;
                    }

                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        ///     Generate Md5 Checksum from string.
        /// </summary>
        /// <param name="Value">String to compute.</param>
        /// <param name="FilePath">Is it a file?</param>
        public static string Md5HashGenerator(this string Value, bool FilePath = false)
        {
            var md5 = MD5.Create();

            if (FilePath)
            {
                var FileStream = File.OpenRead(Value);
                FileStream.Position = 0;
                var hash = md5.ComputeHash(FileStream);

                var sb = new StringBuilder();

                for (var i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }

                return sb.ToString();
            }
            else
            {
                var inputBytes = Encoding.UTF8.GetBytes(Value);
                var hash = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();

                for (var i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }

        /// <summary>
        ///     Generate SHA1 Checksum from string.
        /// </summary>
        /// <param name="Value">String to compute.</param>
        public static string SHA1HashGenerator(this string Value)
        {
            var sha1 = SHA1.Create();
            var inputBytes = Encoding.UTF8.GetBytes(Value);
            var hash = sha1.ComputeHash(inputBytes);

            var sb = new StringBuilder();

            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Generate SHA256 Checksum from string.
        /// </summary>
        /// <param name="Value">String to compute.</param>
        public static string SHA256HashGenerator(this string Value)
        {
            var sha256 = SHA256.Create();
            var inputBytes = Encoding.UTF8.GetBytes(Value);
            var hash = sha256.ComputeHash(inputBytes);

            var sb = new StringBuilder();

            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Starts a process resource by specifying the name of a document or application
        ///     file and associates the resource with a new <see cref="Process" /> component.
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
            }
        }
    }
}