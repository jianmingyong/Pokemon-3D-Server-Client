namespace Pokemon_3D_Server_Core.Server_Client_Listener.Settings.Data
{
    /// <summary>
    /// Class containing Common Data [Must inherit]
    /// </summary>
    public abstract class CommonData
    {
        /// <summary>
        /// Player Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// GameJolt ID
        /// </summary>
        public int GameJoltID { get; set; }

        /// <summary>
        /// Get/Set BlackList Reason
        /// </summary>
        public string Reason { get; set; }
    }
}