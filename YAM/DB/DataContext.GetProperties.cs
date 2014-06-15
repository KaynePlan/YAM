using System;
using System.Collections.Generic;
using System.Linq;

namespace YAM
{
    public partial class DataContext : BaseClass
    {
        private ObservableCollectionEx<Title> _GlobalMusic;
        private ObservableCollectionEx<Title> _PlaylistMusic;
        private ObservableCollectionEx<Artist> _ArtistCollection;
        private Dictionary<string, string> _Templates;
        private Artist _SelectedArtist;
        private Title _SelectedMusic;

        #region read only properties

        public ObservableCollectionEx<Title> GlobalMusic { get { return _GlobalMusic ?? (_GlobalMusic = new ObservableCollectionEx<Title>(db.Titles)); } }

        public ObservableCollectionEx<Title> PlaylistMusic { get { return _PlaylistMusic ?? (_PlaylistMusic = new ObservableCollectionEx<Title>()); } }

        public ObservableCollectionEx<Artist> ArtistCollection { get { return _ArtistCollection ?? (_ArtistCollection = new ObservableCollectionEx<Artist>(db.Artists)); } }

        public Int32 GlobalMusicCount { get { return this.GlobalMusic.Count; } }

        public Int32 PlaylistMusicCount { get { return this.PlaylistMusic.Count; } }

        //public String DeletePlayListButton { get { return "Images/" + ((false) ? "HP-Recycle-Empty-Dock" : "HP-Recycle-Full-Dock") + ".png"; } }

        //public String SelectedSongCover { get { return @"D:\Eigene Dokumente\GitHub\YAM\YAM\Images\rammstein-made-in-germany-album-cover.jpg"; } }

        public IEnumerable<Title> SelectedGlobalMusic { get { return GlobalMusic.Where(o => o.IsSelected); } }

        public IEnumerable<Title> SelectedPlaylistMusic { get { return PlaylistMusic.Where(o => o.IsSelected); } }

        public Title SelectedMusic
        {
            get { return _SelectedMusic; }
            set
            {
                if (_SelectedMusic == value)
                    return;

                _SelectedMusic = value;

                //Änderungen in dem Info Bereich, in die DB speichern
                db.SaveChanges();

                OnPropertyChanged("SelectedMusic");
                OnPropertyChanged("ArtistCollection");
            }
        }

        public String NewArtist
        {
            set
            {
                if (SelectedArtist != null)
                    return;

                if (!string.IsNullOrEmpty(value))
                {
                    var existsArtist = db.Artists.FirstOrDefault(a => a.Artistname == value);

                    if (existsArtist == null)
                    {
                        var newArtist = new Artist()
                        {
                            Id = db.Artists.Max(a => a.Id) + 1,
                            Artistname = value,
                            Titles = (SelectedMusic as ICollection<Title>)
                        };

                        this.ArtistCollection.Add(newArtist);
                        db.Artists.Add(newArtist);
                    }
                    else
                        existsArtist.Titles.Add(SelectedMusic);

                    try
                    {
                        db.SaveChanges();

                        UpdateGlobalMusicCollection();
                        UpdatePlaylistMusicCollection();
                    }
                    catch (Exception ex) { }

                    OnPropertyChanged("ArtistCollection");
                    OnPropertyChanged("NewArtist");
                }
            }
        }

        public Artist SelectedArtist
        {
            get { return this._SelectedMusic.Artists.FirstOrDefault(); }
            set
            {
                if (_SelectedArtist == value)
                    return;

                _SelectedArtist = value;

                OnPropertyChanged("SelectedArtist");
                OnPropertyChanged("ArtistCollection");
            }
        }

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
