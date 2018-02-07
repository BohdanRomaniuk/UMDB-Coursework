using HtmlAgilityPack;
using parser.DataTypes;
using parser.WebLogginer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace parser.ViewModels
{
    public class MovieViewModel: INotifyPropertyChanged
    {
        //Logining
        #region logining
        BrowserSession session;
        public BrowserSession Session
        {
            get
            {
                return session;
            }
            set
            {
                session = value;
                OnPropertyChanged(nameof(Session));
            }
        }
        private string userName;
        private string userPassword;
        private string status = "[Офлайн]";
        private Brush statusColor = Brushes.Red;
        #endregion logining

        //Parsing
        #region parsing
        private string url;
        private int fromPage=1;
        private int toPage=1;
        private ObservableCollection<Movie> movies;

        private string searchText = "";
        private int searchType = 0;
        private int fromMovie;
        private int toMovie;
        private int fromPoster;
        private int toPoster;

        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }
        public string UserPassword
        {
            get
            {
                return userPassword;
            }
            set
            {
                userPassword = value;
                OnPropertyChanged(nameof(UserPassword));
            }
        }
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
        public Brush StatusColor
        {
            get
            {
                return statusColor;
            }
            set
            {
                statusColor = value;
                OnPropertyChanged(nameof(StatusColor));
            }
        }

        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
                OnPropertyChanged(nameof(Url));
            }
        }
        public int FromPage
        {
            get
            {
                return fromPage;
            }
            set
            {
                fromPage = value;
                OnPropertyChanged(nameof(FromPage));
            }
        }
        public int ToPage
        {
            get
            {
                return toPage;
            }
            set
            {
                toPage = value;
                OnPropertyChanged(nameof(ToPage));
            }
        }


        public string SearchText
        {
            get
            {
                return searchText;
            }
            set
            {
                searchText = value;
                OnPropertyChanged(nameof(SearchText));
                OnPropertyChanged(nameof(Movies));
            }
        }
        public int SearchType
        {
            get
            {
                return searchType;
            }
            set
            {
                searchType = value;
                OnPropertyChanged(nameof(SearchType));
            }
        }
        public int FromMovie
        {
            get
            {
                return fromMovie;
            }
            set
            {
                fromMovie = value;
                OnPropertyChanged(nameof(FromMovie));
            }
        }
        public int ToMovie
        {
            get
            {
                return toMovie;
            }
            set
            {
                toMovie = value;
                OnPropertyChanged(nameof(ToMovie));
            }
        }
        public int FromPoster
        {
            get
            {
                return fromPoster;
            }
            set
            {
                fromPoster = value;
                OnPropertyChanged(nameof(FromPoster));
            }
        }
        public int ToPoster
        {
            get
            {
                return toPoster;
            }
            set
            {
                toPoster = value;
                OnPropertyChanged(nameof(ToPoster));
            }
        }

        private ObservableCollection<Movie> search(string what)
        {
            return (SearchText != "") ? new ObservableCollection<Movie>(movies.Where(s => (new CultureInfo("UA")).CompareInfo.IndexOf(s.Name, SearchText, CompareOptions.IgnoreCase) >= 0)) : movies;
        }
        public ObservableCollection<Movie> Movies
        {
            get
            {
                //return movies;
                //switch(SearchType)
                //{
                //    case 0:
                //        return (SearchText != "") ? new ObservableCollection<Movie>(movies.Where(s => (new CultureInfo("UA")).CompareInfo.IndexOf(s.Name, SearchText, CompareOptions.IgnoreCase) >= 0)) : movies;
                //    case 1:
                //        return (SearchText != "") ? new ObservableCollection<Movie>(movies.Where(s => (new CultureInfo("UA")).CompareInfo.IndexOf(s.Year.ToString(), SearchText, CompareOptions.IgnoreCase) >= 0)) : movies;
                //    case 2:
                //        return (SearchText != "") ? new ObservableCollection<Movie>(movies.Where(s => (new CultureInfo("UA")).CompareInfo.IndexOf(s.Genre, SearchText, CompareOptions.IgnoreCase) >= 0)) : movies;
                //    case 3:
                //        return (SearchText != "") ? new ObservableCollection<Movie>(movies.Where(s => (new CultureInfo("UA")).CompareInfo.IndexOf(s.Countries, SearchText, CompareOptions.IgnoreCase) >= 0)) : movies;
                //    case 4:
                //        return (SearchText != "") ? new ObservableCollection<Movie>(movies.Where(s => (new CultureInfo("UA")).CompareInfo.IndexOf(s.ImdbLink, SearchText, CompareOptions.IgnoreCase) >= 0)) : movies;
                //    case 5:
                //        return (SearchText != "") ? new ObservableCollection<Movie>(movies.Where(s => (new CultureInfo("UA")).CompareInfo.IndexOf(s.Director, SearchText, CompareOptions.IgnoreCase) >= 0)) : movies;
                //    case 6:
                //        return (SearchText != "") ? new ObservableCollection<Movie>(movies.Where(s => (new CultureInfo("UA")).CompareInfo.IndexOf(s.Actors, SearchText, CompareOptions.IgnoreCase) >= 0)) : movies;
                //    case 7:
                //        return (SearchText != "") ? new ObservableCollection<Movie>(movies.Where(s => (new CultureInfo("UA")).CompareInfo.IndexOf(s.Story, SearchText, CompareOptions.IgnoreCase) >= 0)) : movies;
                //    default:
                //        return movies;
                //}
                return (SearchText != "") ? new ObservableCollection<Movie>(movies.Where(s => (new CultureInfo("UA")).CompareInfo.IndexOf(s.Name, SearchText, CompareOptions.IgnoreCase) >= 0)) : movies;
            }
            set
            {
                movies = value;
                OnPropertyChanged(nameof(Movies));
            }
        }
        #endregion parsing

        //ProgressBar
        #region progress bar
        private int progress;
        private int maximum;
        public int Progress
        {
            get
            {
                return progress;
            }
            set
            {
                progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }
        public int Maximum
        {
            get
            {
                return maximum;
            }
            set
            {
                maximum = value;
                OnPropertyChanged(nameof(Maximum));
            }
        }
        #endregion progress bar

        //Commands
        #region commands
        public ICommand LoginCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }
        public ICommand GetAllInfoCommand { get; private set; }
        public ICommand ShowMovieCommand { get; private set; }
        public ICommand OpenFromBinaryCommand { get; private set; }
        public ICommand SaveToBinaryCommand { get; private set; }
        public ICommand OpenParseWindowCommand { get; private set; }
        public ICommand ParseMoviesCommand { get; private set; }
        public ICommand OpenSavePostersWindowCommand { get; private set; }
        public ICommand SavePostersCommand { get; private set; }
        #endregion

        public MovieViewModel()
        {
            Session = new BrowserSession();
            UserName = "bohdan2307";
            UserPassword = "BOHDAN09731353740973135374";
            Movies = new ObservableCollection<Movie>();

            LoginCommand = new RelayCommand(Login);
            ClearCommand = new RelayCommand(Clear);
            GetAllInfoCommand = new RelayCommand(GetAllInfo);
            ShowMovieCommand = new RelayCommand(ShowMovie);
            OpenFromBinaryCommand = new RelayCommand(OpenFromBinary);
            SaveToBinaryCommand = new RelayCommand(SaveToBinary);
            OpenParseWindowCommand = new RelayCommand(OpenParseWindow);
            ParseMoviesCommand = new RelayCommand(ParseMovie);
            OpenSavePostersWindowCommand = new RelayCommand(OpenSavePostersWindow);
            SavePostersCommand = new RelayCommand(SavePosters);
        }

        private async void Login(object obj)
        {
            await Task.Run(() =>
            {
                Session.Get("https://toloka.to/login.php");
                Session.FormElements["username"] = UserName;
                Session.FormElements["password"] = UserPassword;
                Session.Post("https://toloka.to/login.php");
                if (Session.Cookies.Count != 0)
                {
                    Status = "[Онлайн]";
                    StatusColor = Brushes.Green;
                }
            });
        }

        private void Clear(object obj)
        {
            Movies.Clear();
        }

        private async void GetAllInfo(object obj)
        {
            Movies.Clear();
            Progress = 0;
            Maximum = ToPage - FromPage + 1;
            int id = 0;
            string name = "";
            for (int page = ToPage; page >= FromPage; --page)
            {
                int start = page * 45 - 45;
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                await Task.Run(() =>
                {
                    doc = session.Load(Url + "-" + start);
                });
                HtmlNodeCollection urls = doc.DocumentNode.SelectNodes("//a[@class='topictitle']");
                for (int i=urls.Count-1; i>=0; --i)
                {
                    try
                    {
                        name = urls[i].InnerText;
                        if (name.Contains("Дилогія") || name.Contains("Трилогія") || name.Contains("Квадрологія") || name.Contains("Пенталогія") || name.Contains("Антологія") || name.Contains("Колекція") || name.Contains("Повне зібрання") || name.Contains("Dilogy") || name.Contains("Trilogy") || name.Contains("Quadrilogy") || name.Contains("колекція") || name.Contains("Розширена"))
                        {
                            continue;
                        }
                        Match yearMatch = Regex.Match(name, @"(\([0-9]{4}\))");
                        if(yearMatch.Success)
                        {
                            name = name.Substring(0, yearMatch.Index);
                            Movie toAdd = new Movie(0, name, urls[i].GetAttributeValue("href", null), Convert.ToInt32(yearMatch.Value.Substring(1,4)));
                            if (Movies.Contains(toAdd))
                            {
                                continue;
                            }
                            toAdd.Id = ++id;
                            Movies.Insert(0, toAdd);
                        }
                    }
                    catch(Exception exc)
                    {
                        System.Windows.MessageBox.Show(exc.Message + "\n" + Movies[i].Id + "\n" + Movies[i].Name + "\n" + Movies[i].Link, "Виникла помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                ++Progress;
            }
        }

        private void OpenParseWindow(object obj)
        {
            if (Movies.Count != 0)
            {
                ParsingPagesWindow ppw = new ParsingPagesWindow(this);
                ppw.Show();
                ppw.Owner = ((MainWindow)System.Windows.Application.Current.MainWindow); ;
            }
        }

        private async void ParseMovie(object obj)
        {
            Progress = 0;
            Maximum = ToMovie-FromMovie+1;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            int fromIndex = Movies.Count - FromMovie;
            int toIndex = Movies.Count - ToMovie;
            for (int i = fromIndex; i >= toIndex; --i)
            {
                try
                {
                    await Task.Run(() =>
                    {
                        doc = session.Load("https://toloka.to/" + Movies[i].Link);
                    });
                    //<span class="postbody">
                    var firstPost = doc.DocumentNode.SelectSingleNode("//span[@class='postbody']");
                    Movies[i].ImdbLink = Movie.ParseImdbLinkFromHtml(firstPost.InnerHtml);
                    Movies[i].Poster = Movie.ParsePosterLinkFromHtml(firstPost.InnerHtml);
                    string text = Movie.StripHTML(firstPost.InnerHtml);
                    Movies[i].Genre = Movie.ParseElementByNameFromText(text, "Жанр:");
                    Movies[i].Countries = Movie.ParseElementByNameFromText(text, "Країна:");
                    Movies[i].Companies = Movie.ParseElementByNameFromText(text, "Кінокомпанія:", "Кіностудія:", "Кіностудія / кінокомпанія:", "Кінокомпанія / телеканал:");
                    Movies[i].Director = Movie.ParseElementByNameFromText(text, "Режисер:");
                    Movies[i].Actors = Movie.ParseElementByNameFromText(text, "Актори:");
                    Movies[i].Story = Movie.ParseElementByNameFromText(text, "Сюжет:", "Сюжет фільму:");
                    Movies[i].Length = Movie.ParseElementByNameFromText(text, "Тривалість:");
                    ++Progress;
                }
                catch(Exception exc)
                {
                    System.Windows.MessageBox.Show(exc.Message+"\n"+Movies[i].Name+"\n"+Movies[i].Link, "Виникла помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ShowMovie(object obj)
        {
            if (obj != null)
            {
                MovieWindow tw = new MovieWindow(obj as Movie);
                tw.Show();
                tw.Owner = ((MainWindow)System.Windows.Application.Current.MainWindow);
            }
        }

        private void OpenFromBinary(object obj)
        {
            Movies.Clear();
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "BIN(*.BIN)|*.bin";
            if (ofd.ShowDialog() ?? true)
            {
                using (Stream reader = File.Open(ofd.FileName, FileMode.Open))
                {
                    BinaryFormatter ser = new BinaryFormatter();
                    Movies = (ObservableCollection<Movie>)ser.Deserialize(reader);
                }
            }
        }

        private void SaveToBinary(object obj)
        {
            Microsoft.Win32.SaveFileDialog svd = new Microsoft.Win32.SaveFileDialog();
            svd.Filter = "BIN(*.BIN)|*.bin";
            if (svd.ShowDialog() ?? true)
            {
                using (FileStream fileStr = new FileStream(svd.FileName, FileMode.Create))
                {
                    BinaryFormatter binFormater = new BinaryFormatter();
                    binFormater.Serialize(fileStr, Movies);
                }
            }
        }

        private void OpenSavePostersWindow(object obj)
        {
            SavePostersWindow spw = new SavePostersWindow(this);
            spw.Show();
        }

        private async void SavePosters(object obj)
        {
            string directory = "";
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !String.IsNullOrEmpty(fbd.SelectedPath))
                {
                    directory = fbd.SelectedPath;
                }
            }
            IEnumerable<Movie> selectedMovies =
                from movie in Movies
                where movie.Id >= FromPoster && movie.Id <= ToPoster
                select movie;
            Progress = 0;
            Maximum = selectedMovies.Count();
            WebClient webClient = new WebClient();
            foreach (Movie movie in selectedMovies)
            {
                await Task.Run(() =>
                {
                    int count = 1;

                    string fullPath = directory + "\\" + movie.PosterFileName;
                    string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
                    string extension = Path.GetExtension(fullPath);
                    string path = Path.GetDirectoryName(fullPath);
                    string newFullPath = fullPath;

                    while (File.Exists(newFullPath))
                    {
                        string newFileName = string.Format("{0}_{1}", fileNameOnly, count++);
                        newFullPath = Path.Combine(path, newFileName + extension);
                        movie.PosterFileName = newFileName + extension;
                    }
                    webClient.DownloadFile(movie.Poster, newFullPath);
                });
                ++Progress;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
