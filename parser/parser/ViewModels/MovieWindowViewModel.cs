using parser.DataTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace parser.ViewModels
{
    public class MovieWindowViewModel: INotifyPropertyChanged
    {
        private Movie currentMovie;
        public Movie CurrentMovie
        {
            get
            {
                return currentMovie;
            }
            set
            {
                currentMovie = value;
                OnPropertyChanged(nameof(CurrentMovie));
            }
        }
        public MovieWindowViewModel(Movie _currentMovie)
        {
            CurrentMovie = _currentMovie;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
