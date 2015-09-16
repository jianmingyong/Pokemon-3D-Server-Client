using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    /// <summary>
    /// Class containing Player.
    /// </summary>
    public class PlayerCollection : List<Player>
    {
        /// <summary>
        /// Add New Player to the collection.
        /// </summary>
        /// <param name="p">Package Data</param>
        public void Add(Package p)
        {
            if (p.IsValid && p.IsFullPackageData())
            {

            }
        }

        /// <summary>
        /// Get the next valid id that is not in use.
        /// </summary>
        public int GetNextValidID()
        {
            if (Count == 0)
            {
                return 0;
            }
            else
            {
                int ValidID = 0;
                List<Player> ListOfPlayer = (from Player p in this orderby p.ID ascending select p).ToList();

                for (int i = 0; i < ListOfPlayer.Count; i++)
                {
                    if (ValidID == ListOfPlayer[i].ID)
                    {
                        ValidID++;
                    }
                    else
                    {
                        return ValidID;
                    }
                }
                return ValidID;
            }
        }
    }
}
