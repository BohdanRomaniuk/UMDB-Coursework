﻿using HtmlAgilityPack;
using parser.DataTypes;
using parser.WebLogginer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace parser.ViewModels
{
    public class MovieMainViewModel : INotifyPropertyChanged
    {
        string connectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;initial catalog=UMDB;integrated security=True;MultipleActiveResultSets=True;";
        //Logining
        #region logining
        Session session;
        public Session Session
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
        private string postersLocation;
        private bool offlineMode;
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
        private int errorsCount = 0;

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
        public bool OfflineMode
        {
            get
            {
                return offlineMode;
            }
            set
            {
                offlineMode = value;
                OnPropertyChanged(nameof(OfflineMode));
                if (value)
                {
                    using (var fbd = new FolderBrowserDialog())
                    {
                        DialogResult result = fbd.ShowDialog();
                        if (result == DialogResult.OK && !String.IsNullOrEmpty(fbd.SelectedPath))
                        {
                            postersLocation = fbd.SelectedPath;
                        }
                    }
                }
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
        public int ErrorsCount
        {
            get
            {
                return errorsCount;
            }
            set
            {
                errorsCount = value;
                OnPropertyChanged(nameof(ErrorsCount));
            }
        }
        
        public ObservableCollection<Movie> Movies
        {
            get
            {
                switch (SearchType)
                {
                    case 0:
                        return (SearchText != "") ? new ObservableCollection<Movie>(movies.Where(s => (new CultureInfo("UA")).CompareInfo.IndexOf(s.Name, SearchText, CompareOptions.IgnoreCase) >= 0)) : movies;
                    case 1:
                        return (SearchText != "") ? new ObservableCollection<Movie>(movies.Where(s => s.Year.ToString()==SearchText)) : movies;
                    case 2:
                        return (SearchText != "") ? new ObservableCollection<Movie>(movies.Where(s => (new CultureInfo("UA")).CompareInfo.IndexOf(s.Genre, SearchText, CompareOptions.IgnoreCase) >= 0)) : movies;
                    case 3:
                        return (SearchText != "") ? new ObservableCollection<Movie>(movies.Where(s => (new CultureInfo("UA")).CompareInfo.IndexOf(s.Countries, SearchText, CompareOptions.IgnoreCase) >= 0)) : movies;
                    case 4:
                        return (SearchText != "") ? new ObservableCollection<Movie>(movies.Where(s => (new CultureInfo("UA")).CompareInfo.IndexOf(s.ImdbLink, SearchText, CompareOptions.IgnoreCase) >= 0)) : movies;
                    case 5:
                        return (SearchText != "") ? new ObservableCollection<Movie>(movies.Where(s => (new CultureInfo("UA")).CompareInfo.IndexOf(s.Director, SearchText, CompareOptions.IgnoreCase) >= 0)) : movies;
                    case 6:
                        return (SearchText != "") ? new ObservableCollection<Movie>(movies.Where(s => (new CultureInfo("UA")).CompareInfo.IndexOf(s.Actors, SearchText, CompareOptions.IgnoreCase) >= 0)) : movies;
                    case 7:
                        return (SearchText != "") ? new ObservableCollection<Movie>(movies.Where(s => (new CultureInfo("UA")).CompareInfo.IndexOf(s.Story, SearchText, CompareOptions.IgnoreCase) >= 0)) : movies;
                    case 8:
                        return (SearchText != "") ? new ObservableCollection<Movie>(movies.Where(s=> (new CultureInfo("UA")).CompareInfo.IndexOf(s.PosterFileName, SearchText, CompareOptions.IgnoreCase) >= 0)) : movies;
                    default:
                        return movies;
                }
            }
            set
            {
                movies = value;
                OnPropertyChanged(nameof(Movies));
                OnPropertyChanged(nameof(MoviesCount));
            }
        }
        public ObservableCollection<Genre> Genres { get; set; }
        public ObservableCollection<Country> Countries { get; set; }
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

        public int MoviesCount
        {
            get
            {
                return movies.Count;
            }
            private set { }
        }

        //Commands
        #region commands
        public ICommand LoginCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }
        public ICommand GetAllInfoCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand ShowMovieCommand { get; private set; }
        public ICommand OpenFromBinaryCommand { get; private set; }
        public ICommand SaveToBinaryCommand { get; private set; }
        public ICommand OpenParseWindowCommand { get; private set; }
        public ICommand ParseMoviesCommand { get; private set; }
        public ICommand ClearDirAndActorsCommand { get; private set; }
        public ICommand OpenSavePostersWindowCommand { get; private set; }
        public ICommand SavePostersCommand { get; private set; }
        public ICommand UpdateImdbLinkCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand GenerateGenresCommand { get; private set; }
        public ICommand SaveGenresToDBCommand { get; private set; }
        public ICommand GenereteCountriesCommand { get; private set; }
        public ICommand SaveCountriesToDBCommand { get; private set; }
        public ICommand SaveMoviesToDBCommand { get; private set; }
        #endregion

        public MovieMainViewModel()
        {
            Session = new Session();
            UserName = "bohdan2307";
            Movies = new ObservableCollection<Movie>();
            Genres = new ObservableCollection<Genre>();
            Countries = new ObservableCollection<Country>();

            LoginCommand = new RelayCommand(Login);
            ClearCommand = new RelayCommand(Clear);
            GetAllInfoCommand = new RelayCommand(GetAllInfo);
            UpdateCommand = new RelayCommand(Update);
            ShowMovieCommand = new RelayCommand(ShowMovie);
            OpenFromBinaryCommand = new RelayCommand(OpenFromBinary);
            SaveToBinaryCommand = new RelayCommand(SaveToBinary);
            OpenParseWindowCommand = new RelayCommand(OpenParseWindow);
            ParseMoviesCommand = new RelayCommand(ParseMovie);
            ClearDirAndActorsCommand = new RelayCommand(ClearDirAndActors);
            OpenSavePostersWindowCommand = new RelayCommand(OpenSavePostersWindow);
            SavePostersCommand = new RelayCommand(SavePosters);
            UpdateImdbLinkCommand = new RelayCommand(UpdateImdbLink);
            DeleteCommand = new RelayCommand(Delete);
            GenerateGenresCommand = new RelayCommand(GenerateGenres);
            SaveGenresToDBCommand = new RelayCommand(SaveGenresToDB);
            GenereteCountriesCommand = new RelayCommand(GenereteCountries);
            SaveCountriesToDBCommand = new RelayCommand(SaveCountriesToDB);
            SaveMoviesToDBCommand = new RelayCommand(SaveMoviesToDB);
        }

        private async void Login(object obj)
        {
            if (obj != null)
            {
                await Task.Run(() =>
                {
                    Session.Get("https://toloka.to/login.php");
                    Session.FormElements["username"] = UserName;
                    Session.FormElements["password"] = (obj as PasswordBox).Password;
                    Session.Post("https://toloka.to/login.php");
                    if (Session.Cookies.Count != 0)
                    {
                        Status = "[Онлайн]";
                        StatusColor = Brushes.Green;
                    }
                });
            }
        }

        private void Clear(object obj)
        {
            Movies.Clear();
        }

        private async void GetAllInfo(object obj)
        {
            ErrorsCount = 0;
            Movies.Clear();
            Progress = 0;
            Maximum = ToPage - FromPage + 1;
            string name = "";
            for (int page = FromPage; page <= ToPage; ++page)
            {
                int start = page * 45 - 45;
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                await Task.Run(() =>
                {
                    doc = session.Load(Url + "-" + start);
                });
                System.Windows.MessageBox.Show(doc.DocumentNode.SelectSingleNode("//body").InnerHtml, "EDFFF");
                HtmlNodeCollection urls = doc.DocumentNode.SelectNodes("//a[@class='topictitle']");
                for(int i=0; i<urls.Count; ++i)
                {
                    try
                    {
                        name = urls[i].InnerText;
                        if (name.Contains("Дилогія") || name.Contains("Трилогія") || name.Contains("Квадрологія") || name.Contains("Пенталогія") || name.Contains("Антологія") || name.Contains("Колекція") || name.Contains("Повне зібрання") || name.Contains("Dilogy") || name.Contains("Trilogy") || name.Contains("Quadrilogy") || name.Contains("колекція") || name.Contains("Collection"))
                        {
                            continue;
                        }
                        Match yearMatch = Regex.Match(name, @"(\([0-9]{4}\))");
                        if(yearMatch.Success)
                        {
                            name = name.Substring(0, yearMatch.Index);
                            if (Regex.Match(name, @"\[(.*?)\]").Success)
                            {
                                name = Regex.Replace(name, @"\[(.*?)\]", String.Empty);
                            }
                            else if(Regex.Match(name, @"\((.*?)\)").Success)
                            {
                                name = Regex.Replace(name, @"\((.*?)\)", String.Empty);
                            }
                            if(name.Contains("&quot;"))
                            {
                                name = name.Replace("&quot;", "\"");
                            }
                            if(name[name.Length-1]==' ')
                            {
                                name = name.Remove(name.Length - 1, 1);
                            }
                            name = name.Replace("  ", " ");
                            Movie toAdd = new Movie(name, urls[i].GetAttributeValue("href", null), Convert.ToInt32(yearMatch.Value.Substring(1,4)));
                            if (Movies.Contains(toAdd))
                            {
                                continue;
                            }
                            Movies.Add(toAdd);
                        }
                    }
                    catch(Exception exc)
                    {
                        ++ErrorsCount;
                        System.Windows.MessageBox.Show(exc.Message + "\n" + (Movies.Count-i) + "\n" + Movies[i].Name + "\n" + Movies[i].Link, "Виникла помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                ++Progress;
            }
        }

        private async void Update(object obj)
        {
            ErrorsCount = 0;
            Progress = 0;
            Maximum = ToPage - FromPage + 1;
            string name = "";
            LinkedList<Movie> toUpdate = new LinkedList<Movie>();
            for (int page = FromPage; page <= ToPage; ++page)
            {
                int start = page * 45 - 45;
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                await Task.Run(() =>
                {
                    doc = session.Load(Url + "-" + start);
                });
                HtmlNodeCollection urls = doc.DocumentNode.SelectNodes("//a[@class='topictitle']");
                for (int i = 0; i < urls.Count; ++i)
                {
                    try
                    {
                        name = urls[i].InnerText;
                        if (name.Contains("Дилогія") || name.Contains("Трилогія") || name.Contains("Квадрологія") || name.Contains("Пенталогія") || name.Contains("Антологія") || name.Contains("Колекція") || name.Contains("Повне зібрання") || name.Contains("Dilogy") || name.Contains("Trilogy") || name.Contains("Quadrilogy") || name.Contains("колекція") || name.Contains("Collection"))
                        {
                            continue;
                        }
                        Match yearMatch = Regex.Match(name, @"(\([0-9]{4}\))");
                        if (yearMatch.Success)
                        {
                            name = name.Substring(0, yearMatch.Index);
                            if (Regex.Match(name, @"\[(.*?)\]").Success)
                            {
                                name = Regex.Replace(name, @"\[(.*?)\]", String.Empty);
                            }
                            else if (Regex.Match(name, @"\((.*?)\)").Success)
                            {
                                name = Regex.Replace(name, @"\((.*?)\)", String.Empty);
                            }
                            if (name.Contains("&quot;"))
                            {
                                name = name.Replace("&quot;", "\"");
                            }
                            if (name[name.Length - 1] == ' ')
                            {
                                name = name.Remove(name.Length - 1, 1);
                            }
                            name = name.Replace("  ", " ");
                            Movie toAdd = new Movie(name, urls[i].GetAttributeValue("href", null), Convert.ToInt32(yearMatch.Value.Substring(1, 4)));
                            if (Movies.Contains(toAdd) || toUpdate.Contains(toAdd))
                            {
                                continue;
                            }
                            toUpdate.AddFirst(toAdd); ;
                        }
                    }
                    catch (Exception exc)
                    {
                        ++ErrorsCount;
                        System.Windows.MessageBox.Show(exc.Message + "\n" + (Movies.Count - i) + "\n" + Movies[i].Name + "\n" + Movies[i].Link, "Виникла помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                ++Progress;
            }
            if (toUpdate.Count!=0)
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(String.Format("Знайдено {0} нових фільмів!\nОновити список?", toUpdate.Count), "Важлива інформація", System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    foreach(Movie movie in toUpdate)
                    {
                        Movies.Insert(0, movie);
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Нових фільмів не знайдено", "Важлива інформація", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
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
            ErrorsCount = 0;
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
                    Match storyMatch = Regex.Match(firstPost.InnerHtml, "<div style=\"text-align: justify;\"><span style=\"font-style: italic\">(.|\n)*?</span></div>");
                    string text = "";
                    if (storyMatch.Success)
                    {
                        Movies[i].Story = Movie.StripHTML(storyMatch.Value);
                        Movies[i].Story = Regex.Replace(Movies[i].Story, @"\t|\n|\r", String.Empty);
                        text = Movie.StripHTML(firstPost.InnerHtml);
                    }
                    else
                    {
                        text = Movie.StripHTML(firstPost.InnerHtml);
                        Movies[i].Story = Movie.ParseElementByNameFromText(text, "Сюжет:", "Сюжет фільму:", "Опис:");
                        Movies[i].Story = Regex.Replace(Movies[i].Story, @"\t|\n|\r", String.Empty);
                    }
                    Movies[i].Genre = Movie.ParseElementByNameFromText(text, "Жанр:");
                    Movies[i].Countries = Movie.ParseElementByNameFromText(text, "Країна:");
                    Movies[i].Companies = Movie.ParseElementByNameFromText(text, "Кінокомпанія:", "Кіностудія:", "Кіностудія / кінокомпанія:", "Кінокомпанія / телеканал:", "Телеканал / кіностудія:");
                    Movies[i].Director = Movie.ParseElementByNameFromText(text, "Режисер:", "Режисери:");
                    Movies[i].Actors = Movie.ParseElementByNameFromText(text, "Актори:", "У ролях:", "Оповідач:");
                    Movies[i].Length = Movie.ParseElementByNameFromText(text, "Тривалість:");
                    ++Progress;
                }
                catch(Exception exc)
                {
                    ++ErrorsCount;
                    System.Windows.MessageBox.Show(exc.Message+"\n"+Movies[i].Name+"\n"+Movies[i].Link, "Виникла помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void ClearDirAndActors(object obj)
        {
            for(int i=0; i<Movies.Count; ++i)
            {
                await Task.Run(() =>
                {
                    Movies[i].Director = Regex.Replace(Movies[i].Director, "[A-Za-z/()]", "");
                    Movies[i].Actors = Regex.Replace(Movies[i].Actors, "[A-Za-z/()]", "");
                    Movies[i].Actors = Regex.Replace(Movies[i].Actors, "[ ]{2,}", " ");
                    Movies[i].Actors = Regex.Replace(Movies[i].Actors, "[ ],", ",");
                });
            }
        }

        private void ShowMovie(object obj)
        {
            if (obj != null)
            {
                MovieWindow tw = new MovieWindow(obj as Movie, OfflineMode, postersLocation);
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
                    Progress = 0;
                    Maximum = ToPoster - FromPoster + 1;
                    int from = Movies.Count - ToPoster;
                    int to = Movies.Count - FromPoster;
                    WebClient webClient = new WebClient();
                    for (int i = from; i <= to; ++i)
                    {

                        await Task.Run(() =>
                        {
                            try
                            {
                                int count = 1;
                                string fullPath = directory + "\\" + Movies[i].PosterFileName;
                                string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
                                string extension = Path.GetExtension(fullPath);
                                string path = Path.GetDirectoryName(fullPath);
                                string newFullPath = fullPath;

                                while (File.Exists(newFullPath))
                                {
                                    string newFileName = string.Format("{0}_{1}", fileNameOnly, count++);
                                    newFullPath = Path.Combine(path, newFileName + extension);
                                    Movies[i].PosterFileName = newFileName + extension;
                                }
                                webClient.DownloadFile(Movies[i].Poster, newFullPath);
                            }
                            catch (WebException wexc)
                            {
                                ++ErrorsCount;
                                Movies[i].PosterFileName = "no_poster.jpg";
                                //System.Windows.MessageBox.Show(wexc.Message + "\n" + Movies[i].Name + "\n" + Movies[i].Link, "Помилка завантаження", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            catch (Exception exc)
                            {
                                ++ErrorsCount;
                                System.Windows.MessageBox.Show(exc.Message + "\n" + Movies[i].Name + "\n" + Movies[i].Link, "Виникла помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        });
                        ++Progress;
                    }
                }
            }
        }

        public async void UpdateImdbLink(object obj)
        {
            foreach(var elem in Movies)
            {
                elem.Length = Regex.Match(elem.Length, @"([0-9]{2}|[0-9]):[0-9]{2}:[0-9]{2}").Value;
            }
        }

        public void Delete(object obj)
        {
            Movie toDelete = obj as Movie;
            if(System.Windows.MessageBox.Show(String.Format("Ви впевнені, що хочете видалити фільм {0}?",toDelete.Name),"Видалення фільму", MessageBoxButton.YesNo,MessageBoxImage.Question)== MessageBoxResult.Yes)
            {
                Movies.Remove(toDelete);
            }
        }

        public void GenerateGenres(object obj)
        {
            int index = 0;
            foreach (Movie movie in Movies)
            {
                string genreString = movie.Genre.Replace('.', ',');
                genreString = Regex.Replace(genreString, @"[А-Я]", с => с.ToString().ToLower());
                string[] genres = genreString.Split(',');
                movie.MovieGenres = new List<Genre>();
                foreach (string genre in genres)
                {
                    string temp = genre;
                    temp = Regex.Replace(temp, @"^\s+", "");
                    temp = Regex.Replace(temp, @"\s+$", "");
                    if (!Genres.Contains(new Genre() { Name = temp }) && temp != "")
                    {
                        Genres.Add(new Genre(++index, temp));
                    }
                    movie.MovieGenres.Add(Genres.Where(s => s.Name == temp).FirstOrDefault());
                }
            }
            System.Windows.MessageBox.Show("Жанри згенеровано!", "Статус генерації", MessageBoxButton.OK);
            //Debuging
            using (StreamWriter writer = new StreamWriter("../../Genres.txt"))
            {
                foreach (var elem in Genres)
                {
                    writer.WriteLine("{0,-10}{1}", elem.Id, elem.Name);
                }
            }
        }

        public void SaveGenresToDB(object obj)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            foreach(Genre genre in Genres)
            {
                command.CommandText = String.Format("INSERT INTO Genres(Name) VALUES(N'{0}');", genre.Name.Replace("'","''"));
                command.ExecuteNonQuery();
            }
            System.Windows.MessageBox.Show("Усі жанри було успішно збережено в БД!", "Збереження в бд", MessageBoxButton.OK);
        }

        public void GenereteCountries(object obj)
        {
            int index = 0;
            foreach (Movie movie in Movies)
            {
                string countriesString = movie.Countries.Replace('.', ',');
                string[] countries = countriesString.Split(',');
                movie.MovieCountries = new List<Country>();
                foreach (string country in countries)
                {
                    string temp = country;
                    temp = Regex.Replace(temp, @"^\s+", "");
                    temp = Regex.Replace(temp, @"\s+$", "");
                    if (!Countries.Contains(new Country() { Name = temp }) && temp != "")
                    {
                        Countries.Add(new Country(++index, temp));
                    }
                    movie.MovieCountries.Add(Countries.Where(s => s.Name == temp).FirstOrDefault());
                }
            }
            System.Windows.MessageBox.Show("Країни згенеровано!", "Статус генерації", MessageBoxButton.OK);
            //Debuging
            using (StreamWriter writer = new StreamWriter("../../Countries.txt"))
            {
                foreach (var elem in Countries)
                {
                    writer.WriteLine("{0,-10}{1}", elem.Id, elem.Name);
                }
            }
        }

        public void SaveCountriesToDB(object obj)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            foreach (Country country in Countries)
            {
                command.CommandText = String.Format("INSERT INTO Countries(Name) VALUES(N'{0}');", country.Name.Replace("'", "''"));
                command.ExecuteNonQuery();
            }
            System.Windows.MessageBox.Show("Усі країни було успішно збережено в БД!", "Збереження в бд", MessageBoxButton.OK);
        }

        public void SaveMoviesToDB(object obj)
        {
            Progress = 0;
            Maximum = Movies.Count;
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            int movieId = 1;
            foreach (Movie movie in Movies)
            {
                command.CommandText = String.Format("INSERT INTO Movies(Name, Year, Length, ImdbLink, Companies, Director, Actors, Story, Poster) VALUES(N'{0}',{1},N'{2}',N'{3}',N'{4}',N'{5}',N'{6}',N'{7}',N'{8}');", movie.Name.Replace("'", "''"), movie.Year, movie.Length, movie.ImdbLink, movie.Companies.Replace("'", "''"), movie.Director.Replace("'", "''"), movie.Actors.Replace("'", "''"), movie.Story.Replace("'", "''"), movie.PosterFileName.Replace("'", "''"));
                command.ExecuteNonQuery();
                foreach(Genre genre in movie.MovieGenres)
                {
                    if(genre!=null)
                    {
                        command.CommandText = String.Format("INSERT INTO MovieGenres(GenreId, MovieId) VALUES ({0},{1});", genre.Id, movieId);
                        command.ExecuteNonQuery();
                    }
                }
                foreach(Country country in movie.MovieCountries)
                {
                    if(country!=null)
                    {
                        command.CommandText = String.Format("INSERT INTO MovieCountries(CountryId, MovieId) VALUES({0},{1});", country.Id, movieId);
                        command.ExecuteNonQuery();
                    }
                }
                ++movieId;
                ++Progress;
            }
            System.Windows.MessageBox.Show("Усі ФІЛЬМИ було успішно збережено в БД!", "Збереження в бд", MessageBoxButton.OK);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
