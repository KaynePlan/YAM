using System;
using System.Collections.Generic;
using System.Linq;
using TagLib;

namespace YAM
{
    public static class MP3TagReader
    {
        public static Boolean IsTitleExists(YAM_StorageEntities db, String filePath)
        {
            return db.Titles.Any(t => t.Filepath.CompareTo(filePath) == 0);
        }

        public static String GetFileName(String filename)
        {
            return filename.Substring(0, filename.LastIndexOf("."));
        }

        public static Title GetMP3Title(YAM_StorageEntities db, String filepath)
        {
            TagLib.File file = null;
            Title metaTag = null;
            System.IO.FileInfo file_info = null;

            try
            {
                file_info = new System.IO.FileInfo(filepath);
            }
            catch
            {
                return null;
            }

            try
            {
                file = TagLib.File.Create(filepath);
            }
            catch (TagLib.UnsupportedFormatException)
            {
                //TODO Exception Handler implementieren
                throw new UnsupportedFormatException("UNSUPPORTED FILE: " + filepath);
            }

            //Album auslesen bzw. anlegen
            Album album = null;
            if (!String.IsNullOrEmpty(file.Tag.Album))
            {
                album = db.Albums.FirstOrDefault(a => a.Albumname == file.Tag.Album);

                if (album == null)
                {
                    album = new Album()
                    {
                        Albumname = file.Tag.Album,
                        Releaseyear = (int)file.Tag.Year
                    };

                    db.Albums.Add(album);
                }
            }

            if (IsTitleExists(db, filepath))
                return null;

            //Titel anlegen
            metaTag = new Title();

            metaTag.Filepath = filepath;
            metaTag.Playtime = file.Properties.Duration.Ticks;
            metaTag.Releaseyear = (int)file.Tag.Year;
            metaTag.Titlename = String.IsNullOrEmpty(file.Tag.Title) ? GetFileName(file_info.Name) : file.Tag.Title;
            metaTag.Playcounter = 0;

            var acodec = file.Properties.Codecs as TagLib.IAudioCodec;

            if (acodec != null)
                metaTag.Bitrate = acodec.AudioBitrate;

            //Lied zum Album zuordnen
            if (!String.IsNullOrEmpty(file.Tag.Album))
            {
                db.Albumtitles.Add(new Albumtitle()
                {
                    Album = album,
                    Title = metaTag,
                    Titlenumber = (int)file.Tag.Track
                });
            }

            if (!String.IsNullOrEmpty(file.Tag.FirstGenre))
            {
                var genre = db.Genres.FirstOrDefault(g => g.Genrename.CompareTo(file.Tag.FirstGenre) == 0);

                if (genre == null)
                    metaTag.Genre1 = new Genre() { Genrename = file.Tag.FirstGenre };
                else
                    metaTag.Genre = genre.Id;
            }

            if (!String.IsNullOrEmpty(file.Tag.FirstPerformer))
            {
                var artist = db.Artists.Where(a => a.Artistname.CompareTo(file.Tag.FirstPerformer) == 0);

                if (!artist.Any())
                    metaTag.Artists.Add(new Artist() { Artistname = file.Tag.FirstPerformer });
                else
                    metaTag.Artists = artist.ToList();
            }
            else
                metaTag.Artists = null;

            //Write("Grouping", file.Tag.Grouping);
            //Write("Title", file.Tag.Title);
            //Write("TitleSort", file.Tag.TitleSort);
            //Write("Album Artists", file.Tag.AlbumArtists);
            //Write("Album Artists Sort", file.Tag.AlbumArtistsSort);
            //Write("Performers", file.Tag.Performers);
            //Write("Performers Sort", file.Tag.PerformersSort);
            //Write("Conductor", file.Tag.Conductor);
            //Write("Album", file.Tag.Album);
            //Write("Album Sort", file.Tag.AlbumSort);
            //Write("Genres", file.Tag.Genres);
            //Write("Year", file.Tag.Year);
            //Write("Track", file.Tag.Track);

            //foreach (TagLib.ICodec codec in file.Properties.Codecs)
            //{
            //    TagLib.IAudioCodec acodec = codec as TagLib.IAudioCodec;

            //    if (acodec != null && (acodec.MediaTypes & TagLib.MediaTypes.Audio) != TagLib.MediaTypes.None)
            //    {
            //        Console.WriteLine("Bitrate:    " + acodec.AudioBitrate);
            //        Console.WriteLine("SampleRate: " + acodec.AudioSampleRate);
            //        Console.WriteLine("Channels:   " + acodec.AudioChannels + "\n");
            //    }
            //}

            //if (file.Properties.MediaTypes != TagLib.MediaTypes.None)
            //    Console.WriteLine("Length:     " + file.Properties.Duration + "\n");

            //IPicture[] pictures = file.Tag.Pictures;

            //Console.WriteLine("Embedded Pictures: " + pictures.Length);

            //foreach (IPicture picture in pictures)
            //{
            //    Console.WriteLine(picture.Description);
            //    Console.WriteLine("   MimeType: " + picture.MimeType);
            //    Console.WriteLine("   Size:     " + picture.Data.Count);
            //    Console.WriteLine("   Type:     " + picture.Type);
            //}

            return metaTag;
        }
    }
}
