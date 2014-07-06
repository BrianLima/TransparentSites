using System;
using System.Data.Linq.Mapping;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TransparentSitesDB
{        
    /*
     * Base class for database
     * Table: Icons
     */
    [Table(Name = "Icons")]

    public class IconsModel
    {
        private int _id;
        [Column(Name = "id", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
        public int id { get { return _id; } set { _id = value; } }

        private string _IconPath;
        [Column(Name = "IconPath", CanBeNull = false)]
        public string IconPath { get { return _IconPath; } set { _IconPath = value; } }

        public static System.Collections.IEnumerable GetIcons()
        {
            DAOIcons daoIcons = new DAOIcons();
            return daoIcons.getAllIcons();
        }

        public bool Save()
        {
            DAOIcons DAOIcons = new DAOIcons();
            return DAOIcons.Save(this);
        }
    }
}
