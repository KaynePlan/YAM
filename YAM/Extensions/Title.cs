using System;

namespace YAM
{
    public partial class Title : ICloneable
    {
        public Boolean IsSelected { get; set; }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public Title Clone()
        {
            return (Title)this.MemberwiseClone();
        }
    }
}
