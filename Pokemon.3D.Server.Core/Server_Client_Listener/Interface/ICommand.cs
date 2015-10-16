using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Players;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Interface
{
    /// <summary>
    /// Implements Command
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Name of the command. [To use, add "/" before the name]
        /// </summary>
        string Name { get;}

        /// <summary>
        /// Short Description of the command.
        /// </summary>
        string Description { get;}

        /// <summary>
        /// Minimum Permission require to use this command.
        /// </summary>
        Player.OperatorTypes RequiredPermission { get;}

        /// <summary>
        /// Handle the Package data.
        /// </summary>
        /// <param name="p">Package data.</param>
        /// <param name="Player">Player.</param>
        void Handle(Package p, Player Player);

        /// <summary>
        /// Create a Help Page.
        /// </summary>
        /// <param name="Pages">Page Number. Start from Zero.</param>
        /// <param name="Player">Player.</param>
        void Help(int Pages, Player Player);
    }
}