using System;
using System.Collections.Generic;
using System.Linq;

namespace YAM
{
    public partial class DataContext : BaseClass
    {
        #region Property variables

        private ObservableCollectionEx<Title> _GlobalMusic;
        private ObservableCollectionEx<Title> _PlaylistMusic;
        private ObservableCollectionEx<Artist> _ArtistCollection;
        private Dictionary<string, string> _Templates;
        private Artist _SelectedArtist;
        private Title _SelectedMusic;
        private IEnumerable<Lang> _Language;
        private IEnumerable<Genre> _Genres;

        #endregion

        #region read only properties

        public ObservableCollectionEx<Title> GlobalMusic { get { return _GlobalMusic ?? (_GlobalMusic = new ObservableCollectionEx<Title>(db.Titles)); } }

        public ObservableCollectionEx<Title> PlaylistMusic { get { return _PlaylistMusic ?? (_PlaylistMusic = new ObservableCollectionEx<Title>()); } }

        public ObservableCollectionEx<Artist> ArtistCollection { get { return _ArtistCollection ?? (_ArtistCollection = new ObservableCollectionEx<Artist>(db.Artists)); } }

        public String Notification { get { return String.Format(NotificationTemplate, NotificationCurrentSongCount, NotificationMaxSongCount); } }

        public Int32 GlobalMusicCount { get { return this.GlobalMusic.Count; } }

        public Int32 PlaylistMusicCount { get { return this.PlaylistMusic.Count; } }

        //public String DeletePlayListButton { get { return "Images/" + ((false) ? "HP-Recycle-Empty-Dock" : "HP-Recycle-Full-Dock") + ".png"; } }

        //public String SelectedSongCover { get { return @"D:\Eigene Dokumente\GitHub\YAM\YAM\Images\rammstein-made-in-germany-album-cover.jpg"; } }

        public String PlaylistPlayTimeString { get { return "Gesamte Spieldauer: " + new TimeSpan(PlaylistMusic.Sum(p => p.Playtime)).ToString(@"mm\:ss"); } }

        public TimeSpan PlaylistPlayTime { get { return new TimeSpan(PlaylistMusic.Sum(p => p.Playtime)); } }

        public IEnumerable<Title> SelectedGlobalMusic { get { return GlobalMusic.Where(o => o.IsSelected); } }

        public IEnumerable<Title> SelectedPlaylistMusic { get { return PlaylistMusic.Where(o => o.IsSelected); } }

        public IEnumerable<Lang> Language { get { return _Language ?? (_Language = new List<Lang>().Union(db.Langs)); } }

        public IEnumerable<Genre> Genres { get { return _Genres ?? (_Genres = db.Genres); } }

        public String ConvertedGenre { get { return (SelectedMusic.Genre.HasValue) ? Genres.First(g => g.Id == SelectedMusic.Genre).Genrename : String.Empty; } }

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
