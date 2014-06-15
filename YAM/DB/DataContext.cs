using System;
using System.Linq;
using YAM_Player;

namespace YAM
{
    public partial class DataContext
    {
        #region public methoden

        public DataContext() { }

        public UserControl1 ChildPlayer { get; set; }

        //Methode um den aktuellen Song, in der Datenbank zu aktualisieren.
        public void UpdateTitleEntry(YAM_Player.Playlist valueChange)
        {
            var song = db.Titles.FirstOrDefault(t => t.Id == valueChange.Id);

            if (song != null)
            {
                song.Lastplayed = DateTime.Now;
                song.Playcounter = song.Playcounter + 1;

                try
                {
                    db.SaveChanges();

                    UpdateGlobalMusicCollection();
                    UpdatePlaylistMusicCollection();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void UpdateGlobalMusicCollection()
        {
            //Workaround für die automatische Nummerierung der Liste
            _GlobalMusic = null;

            OnPropertyChanged("GlobalMusic");
            OnPropertyChanged("GlobalMusicCount");
            OnPropertyChanged("SelectedGlobalMusic");
        }

        private void UpdatePlaylistMusicCollection()
        {
            //Workaround für die automatische Nummerierung der Liste
            var oldList = _PlaylistMusic;
            _PlaylistMusic = null;

            OnPropertyChanged("PlaylistMusic");
            OnPropertyChanged("PlaylistMusicCount");
            OnPropertyChanged("PlaylistPlayTime");

            _PlaylistMusic = oldList;

            OnPropertyChanged("PlaylistMusic");
            OnPropertyChanged("PlaylistMusicCount");
            OnPropertyChanged("PlaylistPlayTime");
        }

        #endregion
    }
}
