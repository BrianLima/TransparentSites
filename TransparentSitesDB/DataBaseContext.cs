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

        private static String[] files = new String[25]
        {
            "/Assets/Icons/box-256.png",
            "/Assets/Icons/chat-256.png",
            "/Assets/Icons/food-256.png",
            "/Assets/Icons/github-256.png",
            "/Assets/Icons/google_drive-256.png",
            "/Assets/Icons/home-256.png",
            "/Assets/Icons/link-256.png",
            "/Assets/Icons/lock-256.png",
            "/Assets/Icons/magazine-256.png",
            "/Assets/Icons/metamorphose-256.png",
            "/Assets/Icons/music_record-256.png",
            "/Assets/Icons/news-256.png",
            "/Assets/Icons/note_music-256.png",
            "/Assets/Icons/private-256.png",
            "/Assets/Icons/reddit-256.png",
            "/Assets/Icons/speech_bubble-256.png",
            "/Assets/Icons/star-256.png",
            "/Assets/Icons/steam-256.png",
            "/Assets/Icons/suitcase-256.png",
            "/Assets/Icons/tire-256.png",
            "/Assets/Icons/tumblr-256.png",
            "/Assets/Icons/utorrent-256.png",
            "/Assets/Icons/Wheel.png",
            "/Assets/Icons/wikipedia-256.png",
            "/Assets/Icons/wordpress-256.png",
        };
    

        private void GenerateData()
        {   
            IconsModel iconsModel = new IconsModel();

            for (int i = 0; i < files.Length; i++)
            {
                iconsModel.IconPath = files[i];
                iconsModel.Save();
            }
        }
    }
}
