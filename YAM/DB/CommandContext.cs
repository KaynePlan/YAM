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
        private Command _AddToPlaylist;
        private Command _RemoveFromPlaylist;

        #endregion

        //Properties für die Button Commands
        #region Commands Definitions

        public Command ImportSong { get { return _ImportSong ?? (_ImportSong = new Command(ImportSong_Executed, ImportSong_CanExecute)); } }
        public Command DeleteSongMain { get { return _DeleteSongMain ?? (_DeleteSongMain = new Command(DeleteSongMain_Executed, DeleteSongMain_CanExecute)); } }
        public Command AddToPlaylist { get { return _AddToPlaylist ?? (_AddToPlaylist = new Command(AddToPlaylist_Executed, AddToPlaylist_CanExecute)); } }
        public Command RemoveFromPlaylist { get { return _RemoveFromPlaylist ?? (_RemoveFromPlaylist = new Command(RemoveFromPlaylist_Executed, RemoveFromPlaylist_CanExecute)); } }

        #endregion

        //CanExecute Methoden für die Button Commands
        #region Commands CanExecute

        private bool ImportSong_CanExecute(object arg) { return true; }

        private bool DeleteSongMain_CanExecute(object arg) { return SelectedGlobalMusic.Any(); }

        private bool AddToPlaylist_CanExecute(object arg) { return SelectedGlobalMusic.Any(); }

        private bool RemoveFromPlaylist_CanExecute(object arg) { return SelectedPlaylistMusic.Any(); }

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

                var importWindow = InitiateWindow(Templates["Import"], filename.Count());
                importWindow.Show();

                foreach (String file in filename)
                {
                    Title newMp3Entry = MP3TagReader.GetMP3Title(db, file);

                    if (newMp3Entry != null)
                    {
                        UpdateWindow(importWindow);
                        db.Titles.Add(newMp3Entry);
                    }

                    db.SaveChanges();
                }

                UpdateGlobalMusicCollection();

                CloseWindow(importWindow);
            }
        }

        private void DeleteSongMain_Executed(object obj)
        {
            int delcount = SelectedGlobalMusic.Count();

            if (MessageBox.Show("Wollen Sie die (" + delcount + ") ausgewählten Lieder, wirklich aus der Datenbank entfernen?", "Sicher?",
                 MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                var musicDeleteList = new List<Title>(SelectedGlobalMusic);

                var deleteWindow = InitiateWindow(Templates["Delete"], musicDeleteList.Count());
                deleteWindow.Show();

                foreach (var song in musicDeleteList)
                {
                    UpdateWindow(deleteWindow);

                    #region CurrentAlbum

                    Nullable<Int32> currentAlbumId = null;

                    if (song.Albumtitles != null && song.Albumtitles.Any())
                    {
                        var currentAlbum = song.Albumtitles.FirstOrDefault();

                        if (currentAlbum != null)
                            currentAlbumId = currentAlbum.AlbumId;
                    }

                    #endregion

                    #region CurrentArtist

                    Nullable<Int32> currentArtistId = null;

                    if (song.Artists != null && song.Artists.Any())
                    {
                        var currentArtist = song.Artists.FirstOrDefault();

                        if (currentArtist != null)
                            currentArtistId = currentArtist.Id;
                    }

                    #endregion

                    //Genre löschen
                    var IsGenreExist = GlobalMusic.Where(g => g.Id != song.Id).Any(g => g.Genre == song.Genre);

                    if (!IsGenreExist && song.Genre.HasValue)
                        db.Genres.Remove(db.Genres.First(g => g.Id == song.Genre.Value));

                    //Lied aus DB löschen
                    db.Titles.Remove(song);

                    //Album löschen
                    if (currentAlbumId.HasValue)
                    {
                        var IsAlbumExists = db.Titles.Where(t => t.Id != song.Id).Any(t => t.Albumtitles.Any(a => a.AlbumId == currentAlbumId));

                        if (!IsAlbumExists)
                        {
                            var album = db.Albums.FirstOrDefault(a => a.Id == currentAlbumId);

                            if (album != null)
                                db.Albums.Remove(album);
                        }

                        var albumtitle = db.Albumtitles.FirstOrDefault(a => a.AlbumId == currentAlbumId);

                        if (albumtitle != null)
                            db.Albumtitles.Remove(albumtitle);
                    }

                    //Künstler löschen
                    if (currentArtistId.HasValue)
                    {
                        var IsArtistExist = db.Titles.Where(t => t.Id != song.Id).Any(t => t.Artists.Any(a => a.Id == currentArtistId));

                        if (!IsArtistExist)
                        {
                            var artist = db.Artists.FirstOrDefault(a => a.Id == currentArtistId);
                            db.Artists.Remove(artist);
                        }
                    }

                    db.SaveChanges();
                }

                db.SaveChanges();
                UpdateGlobalMusicCollection();

                CloseWindow(deleteWindow);
            }
        }

        private void AddToPlaylist_Executed(object obj)
        {
            foreach (var song in SelectedGlobalMusic)
                if (!PlaylistMusic.Any(s => s.Id == song.Id))
                    this.PlaylistMusic.Add(song);

            UpdatePlaylistMusicCollection();
        }

        private void RemoveFromPlaylist_Executed(object obj)
        {
            var selecteditems = new List<Title>(SelectedPlaylistMusic);

            foreach (var song in selecteditems)
                if (PlaylistMusic.Any(s => s.Id == song.Id))
                    this.PlaylistMusic.Remove(song);

            UpdatePlaylistMusicCollection();
        }

        #endregion

        private NotificationWindow InitiateWindow(String template, Int32 maxSongs)
        {
            var window = new NotificationWindow();
            window.Owner = App.Current.MainWindow;
            window.DataContext = this;

            NotificationCurrentSongCount = 0;
            NotificationMaxSongCount = maxSongs;
            NotificationTemplate = template;

            OnPropertyChanged("Notification");

            return window;
        }

        private void UpdateWindow(NotificationWindow window)
        {
            NotificationCurrentSongCount++;
            OnPropertyChanged("Notification");
            window.Refresh();
        }

        private void CloseWindow(NotificationWindow window)
        {
            window.Close();
            NotificationCurrentSongCount = 0;
            NotificationMaxSongCount = 0;
        }
    }
}