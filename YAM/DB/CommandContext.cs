using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace YAM
{
    public partial class DataContext
    {
        //Variablen für die Button Commands
        #region Variables

        private Command _ImportSong;
        private Command _DeleteSongMain;

        #endregion

        //Properties für die Button Commands
        #region Commands Definitions

        public Command ImportSong { get { return _ImportSong ?? (_ImportSong = new Command(ImportSong_Executed, ImportSong_CanExecute)); } }
        public Command DeleteSongMain { get { return _DeleteSongMain ?? (_DeleteSongMain = new Command(DeleteSongMain_Executed, DeleteSongMain_CanExecute)); } }

        #endregion

        //CanExecute Methoden für die Button Commands
        #region Commands CanExecute

        private bool ImportSong_CanExecute(object arg)
        {
            return true;
        }

        private bool DeleteSongMain_CanExecute(object arg)
        {
            return SelectedGlobalMusic.Any();
        }

        #endregion

        //Executed Methoden für die Button Commands
        #region Commands Execute

        private void ImportSong_Executed(object obj)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.Multiselect = true;
            dlg.DefaultExt = ".mp3";
            dlg.Filter = "Audio file (.mp3)|*.mp3";

            Nullable<Boolean> result = dlg.ShowDialog();

            if (result == true)
            {
                String[] filename = dlg.FileNames;

                foreach (String file in filename)
                {
                    Title newMp3Entry = MP3TagReader.GetMP3Title(db, file);

                    if (newMp3Entry != null)
                    //{
                        db.Titles.Add(newMp3Entry);
                        //GlobalMusic.Add(newMp3Entry);
                    //}

                    db.SaveChanges();
                }

                UpdateGlobalMusicCollection();
            }
        }

        private void DeleteSongMain_Executed(object obj)
        {
            if (MessageBox.Show("Wollen Sie die ausgewählten Lieder, wirklich aus der Datenbank entfernen?", "Sicher?",
                 MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                var musicDeleteList = new List<Title>(SelectedGlobalMusic);

                foreach (var song in musicDeleteList)
                {
                    var currentAlbumId = song.Albumtitles.First().AlbumId;
                    var currentArtistId = song.Artists.First().Id;

                    //Genre löschen
                    var IsGenreExist = GlobalMusic.Where(g => g.Id != song.Id).Any(g => g.Genre == song.Genre);

                    if (!IsGenreExist && song.Genre.HasValue)
                        db.Genres.Remove(db.Genres.First(g => g.Id == song.Genre.Value));

                    //Lied aus DB löschen
                    db.Titles.Remove(song);
                    
                    //Album löschen
                    var IsAlbumExists = GlobalMusic.Where(t => t.Id != song.Id).Any(t => t.Albumtitles.Any(a => a.AlbumId == currentAlbumId));

                    if (!IsAlbumExists)
                    {
                        var album = db.Albums.FirstOrDefault(a => a.Id == currentAlbumId);
                        db.Albums.Remove(album);
                    }

                    var albumtitle = db.Albumtitles.FirstOrDefault(a => a.AlbumId == currentAlbumId);

                    if (albumtitle != null && !IsAlbumExists)
                        db.Albumtitles.Remove(albumtitle);

                    //Künstler löschen
                    var IsArtistExist = GlobalMusic.Where(t => t.Id != song.Id).Any(t => t.Artists.Any(a => a.Id == currentArtistId));

                    if (!IsArtistExist)
                    {
                        var artist = db.Artists.FirstOrDefault(a => a.Id == currentArtistId);
                        db.Artists.Remove(artist);
                    }
                    
                    //GlobalMusic.Remove(song);
                }

                db.SaveChanges();

                UpdateGlobalMusicCollection();
            }
        }
        #endregion
    }
}