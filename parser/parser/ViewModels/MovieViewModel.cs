using HtmlAgilityPack;
using parser.DataTypes;
using parser.WebLogginer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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

        public ObservableCollection<Movie> Movies { get; private set; }
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
        public ICommand GetAllInfoCommand { get; private set; }
        public ICommand ShowMovieCommand { get; private set; }
        #endregion

        public MovieViewModel()
        {
            Session = new BrowserSession();
            UserName = "bohdan2307";
            UserPassword = "BOHDAN09731353740973135374";
            Movies = new ObservableCollection<Movie>();

            LoginCommand = new RelayCommand(Login);
            GetAllInfoCommand = new RelayCommand(GetAllInfo);
            ShowMovieCommand = new RelayCommand(ShowMovie);
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
                HtmlDocument doc = new HtmlDocument();
                await Task.Run(() =>
                {
                    doc = session.Load(Url + "-" + start);
                });
                HtmlNodeCollection urls = doc.DocumentNode.SelectNodes("//a[@class='topictitle']");
                for(int i=urls.Count-1; i>=0; --i)
                {
                    name = urls[i].InnerText.Substring(0, urls[i].InnerHtml.LastIndexOf(')') + 1);
                    Movies.Insert(0,new Movie(++id, name, urls[i].GetAttributeValue("href", null), Convert.ToInt32(name.Substring(name.Length-5, 4))));
                }
                ++Progress;
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
