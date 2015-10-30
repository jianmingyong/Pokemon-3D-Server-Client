using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using Pokemon_3D_Server_Core.Server_Client_Listener.Interface;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Commands
{
    /// <summary>
    /// Class containing list of commands.
    /// </summary>
    public class CommandCollection : List<ICommand>
    {
        /// <summary>
        /// Check for all possible command implemented and just use apply into the list.
        /// </summary>
        public void AddCommand()
        {
            if (Count > 0)
            {
                RemoveRange(0, Count);
            }

            AddRange(from t in Assembly.GetExecutingAssembly().GetTypes()
                     where t.GetInterfaces().Contains(typeof(ICommand)) && t.GetConstructor(Type.EmptyTypes) != null
                     select Activator.CreateInstance(t) as ICommand);
        }

        /// <summary>
        /// Handle All possible command.
        /// </summary>
        /// <param name="p">Package</param>
        public void HandleAllCommand(Package p)
        {
            for (int i = 0; i < Count; i++)
            {
                this[i].Handle(p, p.Client == null ? null : Core.Player.GetPlayer(p.Client));
            }
        }
    }
}