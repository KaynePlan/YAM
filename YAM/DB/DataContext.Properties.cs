using System;
using System.Collections.Generic;
using System.Linq;

namespace YAM
{
    public partial class DataContext
    {
        #region Property variables

        //private String _CurrentPlaylistName;
        private Lang _SelectedLanguage;

        #endregion

        #region Private Properties

        private String NotificationTemplate { get; set; }

        private Int32 NotificationCurrentSongCount { get; set; }

        private Int32 NotificationMaxSongCount { get; set; }

        #endregion

        #region Public Properties

        //public String CurrentPlaylistName
        //{
        //    get { return _CurrentPlaylistName; }
        //    set
        //    {
        //        if (_CurrentPlaylistName == value)
        //            return;

        //        _CurrentPlaylistName = value;

        //        OnPropertyChanged("CurrentPlaylistName");
        //    }
        //}

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
                OnPropertyChanged("SelectedArtist");
                OnPropertyChanged("SelectedLanguage");
                OnPropertyChanged("ConvertedGenre");
            }
        }

        public String NewArtist
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var existsArtist = db.Artists.FirstOrDefault(a => a.Artistname == value);

                    if (SelectedMusic.Artists.FirstOrDefault() != null
                        && SelectedMusic.Artists.FirstOrDefault().Artistname == value)
                        return;

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
                    {
                        //Wechsel der Interpreten gestalltet sich schwieriger als gedacht.
                        //Die Tabellen Struktur lässt es nicht zu einen Eintrag zu löschen
                        //ohne den Interpreten aus der Datenbank zu entfernen
                        
                        //SelectedMusic.Artists = null;
                        //existsArtist.Titles.Add(SelectedMusic);
                    }

                    try
                    {
                        db.SaveChanges();

                        UpdateGlobalMusicCollection();
                        UpdatePlaylistMusicCollection();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Database Error on Artist changes", ex);
                    }
                }

                OnPropertyChanged("ArtistCollection");
                OnPropertyChanged("NewArtist");
                OnPropertyChanged("SelectedArtist");
            }
        }

        public Artist SelectedArtist
        {
            get { return this._SelectedMusic.Artists.FirstOrDefault(); }
            set
            {
                _SelectedArtist = value;

                OnPropertyChanged("SelectedArtist");
                OnPropertyChanged("ArtistCollection");

                //UpdateGlobalMusicCollection();
                //UpdatePlaylistMusicCollection();
            }
        }

        public Lang SelectedLanguage
        {
            get { return _SelectedLanguage; }
            set
            {
                _SelectedLanguage = value;

                if (SelectedMusic.Lang != value.Id)
                {
                    SelectedMusic.Lang = (value != null) ? (Int32?)value.Id : null;
                    db.SaveChanges();
                }

                OnPropertyChanged("SelectedLanguage");
                //OnPropertyChanged("ArtistCollection");
            }
        }

        #endregion
    }
}
