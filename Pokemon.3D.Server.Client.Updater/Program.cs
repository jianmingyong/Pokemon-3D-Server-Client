using System;
using System.IO;
using SharpCompress.Archive;
using SharpCompress.Archive.Zip;
using SharpCompress.Common;
using SharpCompress.Reader;

namespace Pokemon.Server.Client.Updater
{
    /// <summary>
    /// Class containing Start Entry Point.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                if (File.Exists(Environment.CurrentDirectory + "\\Downloads\\Release.zip"))
                {
                    if (ZipArchive.IsZipFile(Environment.CurrentDirectory + "\\Downloads\\Release.zip"))
                    {
                        using (Stream stream = File.OpenRead(Environment.CurrentDirectory + "\\Downloads\\Release.zip"))
                        {
                            var reader = ReaderFactory.Open(stream);
                            while (reader.MoveToNextEntry())
                            {
                                if (!reader.Entry.IsDirectory)
                                {
                                    reader.WriteEntryToDirectory(Environment.CurrentDirectory, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                                }
                            }
                        }
                    }
                }

                Functions.Run(Environment.CurrentDirectory + "\\Pokemon.3D.Server.Client.GUI.exe");
            }
            catch (Exception)
            {
                Functions.Run(Environment.CurrentDirectory + "\\Pokemon.3D.Server.Client.GUI.exe");
            }
        }
    }
}
