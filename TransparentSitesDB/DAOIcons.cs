using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransparentSitesDB
{
    public class DAOIcons
    {
        /// <summary>
        /// Get's every card on the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IconsModel> getAllIcons()
        {
            List<IconsModel> data = new List<IconsModel>();
            using (DataBaseContext db = new DataBaseContext(DataBaseContext.ConnectionString))
            {
                data = (from cards in db.cards
                        orderby cards.PlayerTeam
                        select cards).ToList();
            }
            return data;
        }

    }
}
