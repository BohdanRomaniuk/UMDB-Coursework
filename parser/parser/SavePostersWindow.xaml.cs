using System.Windows;
using parser.ViewModels;

namespace parser
{
    public partial class SavePostersWindow : Window
    {

        public SavePostersWindow(MovieViewModel movieViewModel)
        {
            InitializeComponent();
            DataContext = movieViewModel;
        }
    }
}
