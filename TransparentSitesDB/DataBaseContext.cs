using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq;

namespace TransparentSitesDB
{
    class DataBaseContext : DataContext
    {
        public static string ConnectionString = "Data Source=isostore:/Icons.sdf";

        private Table<IconsModel> _IconsModel;

        public Table<IconsModel> iconsModel
        {
            get
            {
                if (_IconsModel == null)
                    _IconsModel = GetTable<IconsModel>();

                return _IconsModel;
            }
        }

        public DataBaseContext(string connectionString)
            : base(connectionString)
        {
            if (!this.DatabaseExists())
            {
                this.CreateDatabase();
                GenerateData();
            }
        }

        private void GenerateData()
        {
            IconsModel iconsModel = new IconsModel();
            iconsModel.IconPath = "/Assets/Icons/Wheel.png";
            iconsModel.Save();

            IconsModel iconsModel1 = new IconsModel();
            iconsModel1.IconPath = "/Assets/Icons/Wheel.png";
            iconsModel1.Save();
        }
    }
}
