//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace YAM
{
    using System;
    using System.Collections.Generic;
    
    public partial class Playlist
    {
        public Playlist()
        {
            this.Playlistitems = new HashSet<Playlistitem>();
        }
    
        public int Id { get; set; }
        public string Playlistname { get; set; }
        public Nullable<System.DateTime> Lastplayed { get; set; }
        public Nullable<int> Playcounter { get; set; }
    
        public virtual ICollection<Playlistitem> Playlistitems { get; set; }
    }
}
