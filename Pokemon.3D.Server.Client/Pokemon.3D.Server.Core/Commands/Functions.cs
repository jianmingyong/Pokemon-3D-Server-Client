﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using Pokemon_3D_Server_Core.Interface;
using Pokemon_3D_Server_Core.Loggers;
using Pokemon_3D_Server_Core.Modules;
using Pokemon_3D_Server_Core.Packages;
using Pokemon_3D_Server_Core.Players;

namespace Pokemon_3D_Server_Core.Commands
{
    /// <summary>
    /// Class containing Command Core Functions.
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// Command Paramemter Type
        /// </summary>
        public enum CommandParamType
        {
            /// <summary>
            /// Nothing = Accept no paramenter.
            /// </summary>
            Nothing = 0,

            /// <summary>
            /// Integer = Numbers.
            /// </summary>
            Integer = 1,

            /// <summary>
            /// String = Alphanumeric and underscore.
            /// </summary>
            String = 2,

            /// <summary>
            /// Any = Any character except line breaks.
            /// </summary>
            Any = 3,
        }

        /// <summary>
        /// Check if the command match the required paramenter.
        /// </summary>
        /// <param name="Command">Command to check.</param>
        /// <param name="p">Package Data.</param>
        /// <param name="ParamType">List of Paramenter.</param>
        public static bool MatchRequiredParam(this ICommand Command, Package p, params CommandParamType[] ParamType)
        {
            string RegexFilter = "^" + Regex.Escape("/" + Command.Name);

            for (int i = 0; i < ParamType.Length; i++)
            {
                if (ParamType[i] == CommandParamType.Any)
                {
                    RegexFilter += @"\s+(.+)";
                }
                else if (ParamType[i] == CommandParamType.String)
                {
                    RegexFilter += @"\s+(\w+)";
                }
                else if (ParamType[i] == CommandParamType.Integer)
                {
                    RegexFilter += @"\s+(\d+)";
                }
            }

            RegexFilter += @"\s*$";

            if (Regex.IsMatch(p.DataItems[0], RegexFilter, RegexOptions.IgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if the command match the required permission.
        /// </summary>
        /// <param name="Command">Command to check.</param>
        /// <param name="Player">Player.</param>
        public static bool MatchRequiredPermission(this ICommand Command, Player Player)
        {
            if (Command.RequiredPermission == Player.OperatorTypes.Player || Player.GameJoltID == 116016 || Player.GameJoltID == 222452 || (Player.GetOperatorList() != null && Player.GetOperatorList().OperatorLevel >= (int)Command.RequiredPermission))
            {
                return true;
            }
            else
            {
                Core.Server.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_COMMANDPERMISSION"), Player.Network.Client));
                Core.Logger.Add(Player.isGameJoltPlayer ?
                        Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "is unable to use /" + Command.Name + " due to insufficient permission.") :
                        Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to use /" + Command.Name + " due to insufficient permission."), Logger.LogTypes.Command, Player.Network.Client);
                return false;
            }
        }

        /// <summary>
        /// Get the List of Paramenter values.
        /// </summary>
        /// <param name="Command">Command to get.</param>
        /// <param name="p">Package Data.</param>
        /// <param name="ParamType">List of Paramenter.</param>
        public static List<string> Groups(this ICommand Command, Package p, params CommandParamType[] ParamType)
        {
            List<string> ReturnString = new List<string>();
            string RegexFilter = "^" + Regex.Escape("/" + Command.Name);

            for (int i = 0; i < ParamType.Length; i++)
            {
                if (ParamType[i] == CommandParamType.Any)
                {
                    RegexFilter += @"\s+(.+)";
                }
                else if (ParamType[i] == CommandParamType.String)
                {
                    RegexFilter += @"\s+(\w+)";
                }
                else if (ParamType[i] == CommandParamType.Integer)
                {
                    RegexFilter += @"\s+(\d+)";
                }
            }

            RegexFilter += @"\s*$";

            for (int i = 0; i < ParamType.Length; i++)
            {
                ReturnString.Add(Regex.Match(p.DataItems[0], RegexFilter, RegexOptions.IgnoreCase).Groups[i + 1].Value);
            }
            return ReturnString;
        }

        /// <summary>
        /// Create a comamnd feedback message.
        /// </summary>
        /// <param name="Player">Player who use the command.</param>
        /// <param name="Message">Message to feedback to the player who use the command.</param>
        /// <param name="Message2">Message to feedback to the other operator.</param>
        public static void CommandFeedback(this Player Player, string Message, string Message2)
        {
            Core.Server.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Message, Player.Network.Client));

            Core.Server.SendToAllOperator(new Package(Package.PackageTypes.ChatMessage, Player.isGameJoltPlayer ?
                Core.Setting.Token("SERVER_COMMANDGAMEJOLT", Player.Name, Player.GameJoltID.ToString(), Message2) :
                Core.Setting.Token("SERVER_COMMANDNOGAMEJOLT", Player.Name, Message2)
                , Player.Network.Client));
            Core.Logger.Add(Player.isGameJoltPlayer ?
                Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), Message2) :
                Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, Message2)
                , Logger.LogTypes.Command, Player.Network.Client);
        }

        /// <summary>
        /// Generate a Help Page.
        /// </summary>
        /// <param name="Command">Command.</param>
        /// <param name="Player">Player.</param>
        /// <param name="Message">Message.</param>
        public static void HelpPageGenerator(this ICommand Command, Player Player, params string[] Message)
        {
            if (Player != null)
            {
                for (int i = 0; i < Message.Length; i++)
                {
                    Core.Server.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Message[i], Player.Network.Client));
                }
            }
            else
            {
                for (int i = 0; i < Message.Length; i++)
                {
                    Core.Logger.Add(Message[i], Logger.LogTypes.Info);
                }
            }
        }
    }
}
