using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace YAM
{
    public class ArtistJoinConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((value as HashSet<Artist>) != null)
                return (value as HashSet<Artist>).First().Artistname + " - ";
            
            else if ((value as List<Artist>) != null)
                return (value as List<Artist>).First().Artistname + " - ";
            
            else
                return string.Empty;

            //try
            //{
            //    return (value as HashSet<Artist>).First().Artistname + " - ";
            //}
            //catch (ArgumentNullException)
            //{
            //    return (value as List<Artist>).First().Artistname + " - ";
            //}
            //catch
            //{
            //    return string.Empty;
            //}
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class PlaytimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new TimeSpan((long)value).ToString(@"mm\:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var item = (ListViewItem)value;
            var listView = ItemsControl.ItemsControlFromItemContainer(item) as ListView;
            int index = listView.ItemContainerGenerator.IndexFromContainer(item) + 1;
            return String.Format("{0}.  ", index);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
