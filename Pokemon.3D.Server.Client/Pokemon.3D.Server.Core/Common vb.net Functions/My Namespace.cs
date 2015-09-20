using System.Configuration;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Devices;

namespace Pokemon_3D_Server_Core.Modules
{
    /// <summary>
    /// Class containing My Namespace.
    /// </summary>
    public static class My
    {
        /// <summary>
        /// Provides properties for manipulating computer components such as audio, the clock, the keyboard, the file system, and so on.
        /// </summary>
        public static ServerComputer Computer = new ServerComputer();

        /// <summary>
        /// Provides properties, methods, and events related to the current application.
        /// </summary>
        public static ApplicationBase Application = new ApplicationBase();

        /// <summary>
        /// Provides properties and methods for accessing the application's settings.
        /// </summary>
        public static ApplicationSettingsBase Settings;
    }

}