namespace Pokemon_3D_Server_Core.Server_Client_Listener.Settings.Data
{
    /// <summary>
    /// Class containing OperatorList data
    /// </summary>
    public class OperatorList : CommonData
    {
        /// <summary>
        /// Get/Set OperatorList Level
        /// </summary>
        public int OperatorLevel { get; set; }

        /// <summary>
        /// New OperatorList
        /// </summary>
        /// <param name="Name">Player Name</param>
        /// <param name="GameJoltID">Player GameJolt ID</param>
        /// <param name="Reason">OperatorList Reason</param>
        /// <param name="OperatorLevel">OperatorList Level</param>
        public OperatorList(string Name, int GameJoltID, string Reason, int OperatorLevel)
        {
            this.Name = Name;
            this.GameJoltID = GameJoltID;
            this.Reason = Reason;
            this.OperatorLevel = OperatorLevel;
        }
    }
}
