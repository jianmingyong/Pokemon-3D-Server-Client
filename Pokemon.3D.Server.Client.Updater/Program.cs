using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using SharpCompress.Common;
using SharpCompress.Reader;

namespace Pokémon_3D_Server_Client
{
    /// <summary>
    /// Class containing the Main Access point.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main Access Point.
        /// </summary>
        /// <param name="args">Program Start Argument.</param>
        public static void Main(string[] args)
        {
            Thread.Sleep(2000);

            if (args.GetLength(0) > 0 && Directory.Exists(args[0].Replace("%20", " ")))
            {
                try
                {
                    using (Stream stream = File.OpenRead(args[0].Replace("%20", " ") + "\\Release.zip"))
                    {
                        var reader = ReaderFactory.Open(stream);
                        while (reader.MoveToNextEntry())
                        {
                            if (!reader.Entry.IsDirectory)
                            {
                                reader.WriteEntryToDirectory(args[0].Replace("%20", " "), ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                            }
                        }
                    }

                    if (File.Exists(args[0].Replace("%20", " ") + "\\Release.zip"))
                    {
                        File.Delete(args[0].Replace("%20", " ") + "\\Release.zip");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadKey();
                }

                Process.Start(args[0].Replace("%20", " ") + "\\Pokemon.3D.Server.Client.GUI.exe");
            }

        }
    }
}
