using parser.DataTypes;
using parser.ViewModels;
using System.Windows;

namespace parser
{
    public partial class MovieWindow : Window
    {
        public MovieWindow(Movie movie)
        {
            InitializeComponent();
            DataContext = new MovieWindowViewModel(movie);
        }
    }
}
