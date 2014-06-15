using System;
using System.Collections.Generic;
using System.Linq;

namespace YAM
{
    public partial class DataContext
    {
        #region Variables

        private String _CurrentPlaylistName;

        #endregion

        #region Properties

        public String CurrentPlaylistName
        {
            get { return _CurrentPlaylistName; }
            set
            {
                if (_CurrentPlaylistName == value)
                    return;

                _CurrentPlaylistName = value;

                OnPropertyChanged("CurrentPlaylistName");
            }
        }

        private String NotificationTemplate { get; set; }
        private Int32 NotificationCurrentSongCount { get; set; }
        private Int32 NotificationMaxSongCount { get; set; }

        #endregion
    }
}
