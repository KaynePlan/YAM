using System;
using System.Collections.Generic;
using System.Linq;

namespace YAM
{
    public partial class DataContext : BaseClass
    {
        private ObservableCollectionEx<Title> _GlobalMusic;
        private ObservableCollectionEx<Title> _PlaylistMusic;
        private Dictionary<string, string> _Templates;

        #region read only properties

        public ObservableCollectionEx<Title> GlobalMusic { get { return _GlobalMusic ?? (_GlobalMusic = new ObservableCollectionEx<Title>(db.Titles)); } }

        public ObservableCollectionEx<Title> PlaylistMusic { get { return _PlaylistMusic ?? (_PlaylistMusic = new ObservableCollectionEx<Title>()); } }

        public String GlobalMusicCount { get { return String.Format("Vorhandene Musik : ({0})", this.GlobalMusic.Count); } }

        public String PlaylistMusicCount { get { return String.Format("Playlist : ({0})", this.PlaylistMusic.Count); } }

        public String DeletePlayListButton { get { return "Images/" + ((false) ? "HP-Recycle-Empty-Dock" : "HP-Recycle-Full-Dock") + ".png"; } }

        public String SelectedSongCover { get { return @"D:\Eigene Dokumente\GitHub\YAM\YAM\Images\rammstein-made-in-germany-album-cover.jpg"; } }

        public IEnumerable<Title> SelectedGlobalMusic { get { return GlobalMusic.Where(o => o.IsSelected); } }

        public IEnumerable<Title> SelectedPlaylistMusic { get { return PlaylistMusic.Where(o => o.IsSelected); } }

        public String Notification { get { return String.Format(NotificationTemplate, NotificationCurrentSongCount, NotificationMaxSongCount); } }

        public String PlaylistPlayTime { get { return "Gesamte Spieldauer: " + new TimeSpan(PlaylistMusic.Sum(p => p.Playtime)).ToString(@"mm\:ss"); } }
        
        public Dictionary<string, string> Templates
        {
            get
            {
                return _Templates ?? (_Templates = new Dictionary<string, string>() {
                    { "Import", "Lied {0} von {1} wird importiert." },
                    { "Delete", "Lied {0} von {1} wird aus der Datenbank entfernt." }
                });
            }
        }

        #endregion

    }
}
