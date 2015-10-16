namespace Pokemon_3D_Server_Core.Server_Client_Listener.Settings.Data
{
    /// <summary>
    /// Class containing Swear Infraction Filter List
    /// </summary>
    public class SwearInfractionFilterList
    {
        /// <summary>
        /// Get/Set Swearing Word
        /// </summary>
        public string Word { get; set; }

        /// <summary>
        /// Get/Set Case Sensitiveness of the word
        /// </summary>
        public bool CaseSensitive { get; set; }

        /// <summary>
        /// Get/Set Regex Expression
        /// </summary>
        public string Regex { get; set; }

        /// <summary>
        /// New SwearInfractionFilterList
        /// </summary>
        /// <param name="Word">Swearing Word</param>
        /// <param name="CaseSensitive">Case Sensitive flag?</param>
        public SwearInfractionFilterList(string Word, bool CaseSensitive)
        {
            this.Word = Word;
            this.CaseSensitive = CaseSensitive;
            Regex = @"\b(" + System.Text.RegularExpressions.Regex.Escape(Word) + @")\b";
        }
    }
}