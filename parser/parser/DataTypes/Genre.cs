using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parser.DataTypes
{
    [Serializable]
    public class Genre: IEquatable<Genre>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Genre()
        {

        }

        public Genre(int _id, string _name)
        {
            Id = _id;
            Name = _name;
        }

        public bool Equals(Genre other)
        {
            return Name.Equals(other.Name);
        }
    }
}
