using Microsoft.Win32;
using parser.DataTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace parser.ViewModels
{
    public class MovieWindowViewModel: INotifyPropertyChanged
    {
        private Movie currentMovie;
        private BitmapImage posterImage;
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
        public BitmapImage PosterImage
        {
            get
            {
                return posterImage;
            }
            set
            {
                posterImage = value;
                OnPropertyChanged(nameof(PosterImage));
            }
        }

        public ICommand CopyPosterUrlCommand { get; private set; }
        public ICommand SavePosterAsCommand { get; private set; }

        public MovieWindowViewModel(Movie _currentMovie)
        {
            CurrentMovie = _currentMovie;
            if(_currentMovie.Poster!= "немає")
            {
                PosterImage = new BitmapImage(new Uri(_currentMovie.Poster));
            }
            CopyPosterUrlCommand = new RelayCommand(CopyPosterUrl);
            SavePosterAsCommand = new RelayCommand(SavePosterAs);
        }

        public void CopyPosterUrl(object obj)
        {
            Clipboard.Clear();
            Clipboard.SetText(CurrentMovie.Poster);
        }

        public async void SavePosterAs(object obj)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = CurrentMovie.PosterFileName;
            saveFileDialog.Filter = "Картинки (*.jpg, *.png, *.gif, *.jpeg, *.bmp)|*.jpg;*.png;*.gif;*.jpeg;*.bmp|Усі файли (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                string fileName = saveFileDialog.FileName;
                await Task.Run(() =>
                {
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile(CurrentMovie.Poster, fileName);
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
