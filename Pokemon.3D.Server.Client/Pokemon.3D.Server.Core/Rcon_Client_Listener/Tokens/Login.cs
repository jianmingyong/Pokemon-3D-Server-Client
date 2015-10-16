namespace Pokemon_3D_Server_Core.Rcon_Client_Listener.Tokens
{
    /// <summary>
    /// Class containing Login Tokens.
    /// </summary>
    public class Login
    {
        public string IPAddress { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// New Login
        /// </summary>
        /// <param name="IPAddress">IPAddress</param>
        /// <param name="Password">Password</param>
        public Login(string IPAddress, string Password)
        {
            this.IPAddress = IPAddress;
            this.Password = Password;
        }
    }
}