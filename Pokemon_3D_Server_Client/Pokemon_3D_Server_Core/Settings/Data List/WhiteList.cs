namespace Global
{
    /// <summary>
    /// Class containing WhiteList data
    /// </summary>
    public class WhiteList : CommonList
    {
        /// <summary>
        /// New WhiteList
        /// </summary>
        /// <param name="Name">Player Name</param>
        /// <param name="GameJoltID">Player GameJolt ID</param>
        /// <param name="Reason">Player WhiteList Reason</param>
        public WhiteList(string Name,int GameJoltID,string Reason)
        {
            this.Name = Name;
            this.GameJoltID = GameJoltID;
            this.Reason = Reason;
        }
    }
}
