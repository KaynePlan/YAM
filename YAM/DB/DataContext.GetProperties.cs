using System;
using System.Collections.Generic;
using System.Linq;

namespace YAM
{
    public partial class DataContext : BaseClass
    {
        private ObservableCollectionEx<Title> _GlobalMusic;

        #region read only properties
        
        public ObservableCollectionEx<Title> GlobalMusic { get { return _GlobalMusic ?? (_GlobalMusic = new ObservableCollectionEx<Title>(db.Titles)); } }

        public String GlobalMusicCount { get { return String.Format("Vorhandene Musik : ({0})", this.GlobalMusic.Count); } }

        public String DeletePlayListButton { get { return "Images/" + ((false) ? "HP-Recycle-Empty-Dock" : "HP-Recycle-Full-Dock") + ".png"; } }

        public String SelectedSongCover { get { return @"D:\Eigene Dokumente\DesktopDevel\YAM\YAM\Images\rammstein-made-in-germany-album-cover.jpg"; } }

        public IEnumerable<Title> SelectedGlobalMusic { get { return GlobalMusic.Where(o => o.IsSelected); } }

        #endregion
    }
}
