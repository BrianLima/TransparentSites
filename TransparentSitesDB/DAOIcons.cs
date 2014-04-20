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
                data = (from cards in db.iconsModel
                        orderby cards.id
                        select cards).ToList();
            }
            return data;
        }


        internal static bool Save(IconsModel iconsModel)
        {
            try
            {
                using (DataBaseContext db = new DataBaseContext(DataBaseContext.ConnectionString))
                {
                    db.iconsModel.InsertOnSubmit(iconsModel);
                    db.SubmitChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
