using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace parser.DataTypes
{
    [Serializable]
    public sealed class Movie: INotifyPropertyChanged
    {
        private int id;
        private string name;
        private int year;
        private string imdbLink;
        private string director;
        private string actors;
        private string companies;
        private string story;
        private string poster;
        private string posterFileName;

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public int Year
        {
            get
            {
                return year;
            }
            set
            {
                year = value;
                OnPropertyChanged(nameof(Year));
            }
        }
        public string ImdbLink
        {
            get
            {
                return imdbLink;
            }
            set
            {
                imdbLink = value;
                OnPropertyChanged(nameof(ImdbLink));
            }
        }
        public string Director
        {
            get
            {
                return director;
            }
            set
            {
                director = value;
                OnPropertyChanged(nameof(Director));
            }
        }
        public string Actors
        {
            get
            {
                return actors;
            }
            set
            {
                actors = value;
                OnPropertyChanged(nameof(Actors));
            }
        }
        public string Companies
        {
            get
            {
                return companies;
            }
            set
            {
                companies = value;
                OnPropertyChanged(nameof(Companies));
            }
        }
        public string Story
        {
            get
            {
                return story;
            }
            set
            {
                story = value;
                OnPropertyChanged(nameof(Story));
            }
        }
        public string Poster
        {
            get
            {
                return poster;
            }
            set
            {
                poster = value;
                OnPropertyChanged(nameof(Poster));
            }
        }
        public string PosterFileName
        {
            get
            {
                return posterFileName;
            }
            set
            {
                posterFileName = value;
                OnPropertyChanged(nameof(PosterFileName));
            }
        }

        public Movie() { }
        public Movie(int _id, string _name, int _year=0, string _imdb="http://", string _director="немає", string _actors="немає", string _companies="відсутні", string _story="немає", string _poster="немає", string _posterFileName="немає")
        {
            Id = _id;
            Name = _name;
            Year = _year;
            ImdbLink = _imdb;
            Director = _director;
            Actors = _actors;
            Companies = _companies;
            Story = _story;
            Poster = _poster;
            PosterFileName = _posterFileName;
        }

        public override string ToString()
        {
            return String.Format("{0,-10}{1,-10}{2,-10}{3,-10}{4,-10}{5,-10}{6,-10}{7,-10}{8,-10}{9,-10}", Id, Name, Year, ImdbLink, Director, Actors, Companies, Story, Poster, PosterFileName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
