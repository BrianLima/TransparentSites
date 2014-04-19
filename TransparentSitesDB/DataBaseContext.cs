using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace TransparentSitesDB
{
    public class DataBaseContext : DataContext
    {
        public static string ConnectionString = "Data Source=isostore:/Icons.sdf";

        private Table<IconsModel> _Cards;

        public Table<IconsModel> cards
        {
            get
            {
                if (_Cards == null)
                    _Cards = GetTable<IconsModel>();

                return _Cards;
            }
        }

        public DataBaseContext(string connectionString)
            : base(connectionString)
        {
            if (!this.DatabaseExists())
            {
                this.CreateDatabase();
            }
        }
    }
}
