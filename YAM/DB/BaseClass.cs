using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAM
{
    public class BaseClass : INotifyPropertyChanged
    {
        private YAM_StorageEntities _db;

        public YAM_StorageEntities db { get { return _db ?? (_db = new YAM_StorageEntities()); } }

        #region INotifyPropertyChanged

        public void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
	
}
