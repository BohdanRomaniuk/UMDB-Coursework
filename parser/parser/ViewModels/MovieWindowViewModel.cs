using Microsoft.Win32;
using parser.DataTypes;
using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
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
            try
            {
                CurrentMovie = _currentMovie;
                if (_currentMovie.Poster != "немає")
                {
                    PosterImage = new BitmapImage(new Uri(_currentMovie.Poster));
                }
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message + "\n" + CurrentMovie.Name + "\n" + CurrentMovie.Link, "Виникла помилка", MessageBoxButton.OK, MessageBoxImage.Error);)
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
                try
                {
                    string fileName = saveFileDialog.FileName;
                    await Task.Run(() =>
                    {
                        WebClient webClient = new WebClient();
                        webClient.DownloadFile(CurrentMovie.Poster, fileName);
                    });
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message + "\n" + CurrentMovie.Name + "\n" + CurrentMovie.Link, "Виникла помилка", MessageBoxButton.OK, MessageBoxImage.Error);)
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
