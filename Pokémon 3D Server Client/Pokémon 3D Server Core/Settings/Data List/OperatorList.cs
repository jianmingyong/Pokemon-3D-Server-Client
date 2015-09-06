namespace Global
{
    /// <summary>
    /// Class containing OperatorList data
    /// </summary>
    public class OperatorList : CommonList
    {
        /// <summary>
        /// New OperatorList
        /// </summary>
        /// <param name="Name">Player Name</param>
        /// <param name="GameJoltID">Player GameJolt ID</param>
        /// <param name="Reason">Player Operator Reason</param>
        /// <param name="OperatorLevel">Player Operator Level</param>
        public OperatorList(string Name,int GameJoltID,string Reason,int OperatorLevel)
        {
            this.Name = Name;
            this.GameJoltID = GameJoltID;
            this.Reason = Reason;
            this.OperatorLevel = OperatorLevel;
        }
    }
}
