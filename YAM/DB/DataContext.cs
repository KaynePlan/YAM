using System;
using System.Linq;

namespace YAM
{
    public partial class DataContext
    {
        #region public methoden

        public DataContext() { }

        //Methode um den aktuellen Song, in der Datenbank zu aktualisieren.
        public void UpdateTitleEntry(Title newValue)
        {
            var song = db.Titles.FirstOrDefault(t => t.Id == newValue.Id);

            if (song != null)
            {
                song.Lastplayed = DateTime.Now;
                song.Playcounter = song.Playcounter + 1;

                try
                {
                    db.SaveChanges();
                    OnPropertyChanged("db");
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

        #endregion
    }
}
