using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace Pokemon_3D_Server_Client_Updater
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0 && Directory.Exists(args[0].Replace("%20", " ")))
            {
                Console.WriteLine("Updating Pokemon 3D Server...");
                
                if (args.Length > 1)
                {
                    try
                    {
                        Process.GetProcessById(int.Parse(args[1])).WaitForExit();
                    }
                    catch (ArgumentException e)
                    {
                    }
                }
                else
                {
                    Thread.Sleep(5000);
                }
                
                try
                {
                    using (Stream stream = File.OpenRead(args[0].Replace("%20", " ") + "\\Pokemon.3D.Server.Client.GUI.zip"))
                    {
                        var reader = ReaderFactory.Open(stream);

                        while (reader.MoveToNextEntry())
                        {
                            try
                            {
                                if (!reader.Entry.IsDirectory)
                                {
                                    reader.WriteEntryToDirectory(args[0].Replace("%20", " "), new ExtractionOptions {ExtractFullPath = true, Overwrite = true});
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }

                    if (File.Exists(args[0].Replace("%20", " ") + "\\Pokemon.3D.Server.Client.GUI.zip"))
                    {
                        File.Delete(args[0].Replace("%20", " ") + "\\Pokemon.3D.Server.Client.GUI.zip");
                    }
                }
                catch (Exception)
                {
                }

                Process.Start(args[0].Replace("%20", " ") + "\\Pokemon.3D.Server.Client.GUI.exe");
            }
        }
    }
}