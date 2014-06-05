using System;
using System.Collections.Generic;
using System.Linq;

namespace YAM
{
    public partial class DataContext
    {
        private String _CurrentPlaylistName;

        #region properties

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

        #endregion
    }
}
