﻿using System;
using System.Collections.Generic;
using Pokemon_3D_Server_Core.Server_Client_Listener.Interface;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Players;
using Pokemon_3D_Server_Core.Server_Client_Listener.Settings.Data;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Commands.Data
{
    /// <summary>
    /// Class containing Season Function.
    /// </summary>
    public class Player_Season : ICommand
    {
        /// <summary>
        /// Name of the command. [To use, add "/" before the name]
        /// </summary>
        public string Name { get; } = "Player.Season";

        /// <summary>
        /// Short Description of the command.
        /// </summary>
        public string Description { get; } = "Change the player season.";

        /// <summary>
        /// Minimum Permission require to use this command.
        /// </summary>
        public Player.OperatorTypes RequiredPermission { get; } = Player.OperatorTypes.Player;

        /// <summary>
        /// Handle the Package data.
        /// </summary>
        /// <param name="p">Package data.</param>
        /// <param name="Player">Player.</param>
        public void Handle(Package p, Player Player = null)
        {
            // Start from the most inner depth Command.
            #region /Player.Season <id>

            if (this.MatchRequiredParam(p, Functions.CommandParamType.Integer))
            {
                List<string> Group = this.Groups(p, Functions.CommandParamType.Integer);

                if (Player != null && this.MatchRequiredPermission(Player) && Player.isGameJoltPlayer)
                {
                    OnlineSetting Settings = Player.GetOnlineSetting();
                    Settings.Season = Group[0].Toint().RollOver(-4, 3);
                    Settings.CurrentWorldSeason = Core.World.GenerateSeason(Settings.Season);
                    Settings.LastWorldUpdate = DateTime.Now;
                    
                    Player.CommandFeedback(Core.World.ToString(), string.Format("have changed the Player Season."));
                }
                else if (Player == null)
                {
                    Core.World.Season = Core.World.GenerateSeason(Group[0].Toint());

                    Core.Logger.Log(Core.World.ToString(), Logger.LogTypes.Info);
                }
            }
            #endregion /Player.Season <id>
        }

        /// <summary>
        /// Create a Help Page.
        /// </summary>
        /// <param name="Pages">Page Number. Start from Zero.</param>
        /// <param name="Player">Player.</param>
        public void Help(int Pages, Player Player = null)
        {
            switch (Pages)
            {
                default:
                    this.HelpPageGenerator(Player,
                        string.Format("---------- Help: {0} ----------", Name),
                        string.Format("Usage: /Global.Season [ID]"),
                        string.Format("-------------------------------------"),
                        string.Format("ID: Season ID."),
                        string.Format("Winter = 0 | Spring = 1 | Summer = 2 | Fall = 3 | Random = -1 | Default Season = -2 | SeasonMonth = -3"),
                        string.Format("-------------------------------------"),
                        string.Format("Description: {0}", Description),
                        string.Format("Required Permission: {0} and above.", RequiredPermission.ToString().Replace("Moderator", " Moderator"))
                        );
                    break;
            }
        }
    }
}