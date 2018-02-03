using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace parser.DataTypes
{
    [Serializable]
    public sealed class Movie: INotifyPropertyChanged
    {
        private int id;
        private string link;
        private string name;
        private int year;       
        private string genre;
        private string countries;
        private string imdbLink;
        private string companies;
        private string director;
        private string actors;
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
        public string Link
        {
            get
            {
                return link;
            }
            set
            {
                link = value;
                OnPropertyChanged(nameof(Link));
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
        public string Genre
        {
            get
            {
                return genre;
            }
            set
            {
                genre = value;
                OnPropertyChanged(nameof(Genre));
            }
        }
        public string Countries
        {
            get
            {
                return countries;
            }
            set
            {
                countries = value;
                OnPropertyChanged(nameof(Countries));
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
        public Movie(int _id, string _name, string _link="http://", int _year=0, string _genre="немає", string _countries="відсутні", string _imdb="http://", string _companies = "відсутні", string _director="немає", string _actors="немає", string _story="немає", string _poster="немає", string _posterFileName="немає")
        {
            Id = _id;
            Name = _name;
            Link = _link;
            Year = _year;
            Genre = _genre;
            Countries = _countries;
            ImdbLink = _imdb;
            Companies = _companies;
            Director = _director;
            Actors = _actors;
            Story = _story;
            Poster = _poster;
            PosterFileName = _posterFileName;
        }

        public override string ToString()
        {
            return String.Format("{0,-10}{1,-10}{2,-10}{3,-10}{4,-10}{5,-10}{6,-10}{7,-10}{8,-10}{9,-10}{10,-10}", Id, Name, Link, Year, Genre, Countries, ImdbLink, Companies, Director, Actors, Story, Poster, PosterFileName);
        }

        public static string ParseImdbLinkFromHtml(string html)
        {
            int startPos = html.IndexOf("imdb.com");
            int endPos = html.IndexOf('"', startPos);
            return (startPos!=-1 && endPos!=-1)?"https://"+html.Substring(startPos,  endPos -startPos):"Помилка";
        }

        public static string ParsePosterLinkFromHtml(string html)
        {
            string url = "";

            int ind1 = html.IndexOf("posters.hurtom.com");
            int ind2 = (ind1 != -1) ? html.IndexOf('"', ind1) : -1;

            int ind3 = html.IndexOf("toloka.to/photos/");
            int ind4 = (ind3 != -1) ? html.IndexOf('"', ind3) : -1;

            int ind5 = html.IndexOf("img.hurtom.com/");
            int ind6 = (ind5 != -1) ? html.IndexOf('"', ind5) : -1;

            int ind7 = html.IndexOf("img src=\"");
            int ind8 = (ind7 != -1) ? html.IndexOf('"', ind7+9) : 0;

            if (ind1 != -1 && ind2 != -1)
            {
                url = "https://" + html.Substring(ind1, ind2 - ind1);
            }
            else if (ind3 != -1 && ind4 != -1)
            {
                url = "https://" + html.Substring(ind3, ind4 - ind3);
            }
            else if (ind5 != -1 && ind6 != -1)
            {
                url = "https://" + html.Substring(ind5, ind6 - ind5);
            }
            else if (ind7 != -1 && ind8 != -1)
            {
                url = "https:"+html.Substring(ind7 + 5, ind8 - ind7);
            }
            else
            {
                url = "Помилка!";
            }
            return url;
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
