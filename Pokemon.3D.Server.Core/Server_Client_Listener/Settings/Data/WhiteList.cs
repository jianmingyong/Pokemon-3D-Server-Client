namespace Pokemon_3D_Server_Core.Server_Client_Listener.Settings.Data
{
    /// <summary>
    /// Class containing WhiteList data
    /// </summary>
    public class WhiteList : CommonData
    {
        /// <summary>
        /// New WhiteList
        /// </summary>
        /// <param name="Name">Player Name</param>
        /// <param name="GameJoltID">Player GameJolt ID</param>
        /// <param name="Reason">WhiteList Reason</param>
        public WhiteList(string Name, int GameJoltID, string Reason)
        {
            this.Name = Name;
            this.GameJoltID = GameJoltID;
            this.Reason = Reason;
        }
    }
}