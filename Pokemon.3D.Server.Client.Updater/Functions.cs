using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace Pokemon.Server.Client.Updater
{
    /// <summary>
    /// Class containing commonly used functions.
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// Represents a newline character for print and display functions.
        /// </summary>
        public static readonly string vbNewLine = Environment.NewLine;

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

        public static void Run(this string File, string Argument = null)
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
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
 