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
            if ((value as HashSet<Artist>) != null && (value as HashSet<Artist>).FirstOrDefault() != null)
                return (value as HashSet<Artist>).First().Artistname + " - ";

            else if ((value as List<Artist>) != null && (value as List<Artist>).FirstOrDefault() != null)
                return (value as List<Artist>).First().Artistname + " - ";


            return string.Empty;
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

    public class CounterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return String.Format((String)parameter, (Int32)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            return String.Format((String)parameter, (Int32)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public static class ToListEx
    {
        public static YAM_Player.Playlist TO_YAM_Player(Title toConvert)
        {
            return new YAM_Player.Playlist()
            {
                Bitrate = toConvert.Bitrate,
                Filepath = toConvert.Filepath,
                Genre = toConvert.Genre,
                Id = toConvert.Id,
                Lang = toConvert.Lang,
                Lastplayed = toConvert.Lastplayed,
                Playcounter = toConvert.Playcounter,
                Playtime = toConvert.Playtime,
                Releaseyear = toConvert.Releaseyear,
                Titlename = toConvert.Titlename
            };
        }

        public static List<YAM_Player.Playlist> TO_YAM_Player_List(IEnumerable<Title> listToConvert)
        {
            var result = new List<YAM_Player.Playlist>();

            foreach (var item in listToConvert)
                result.Add(TO_YAM_Player(item));

            return result;
        }
    }
}
