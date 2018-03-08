using System.Windows;
using parser.ViewModels;

namespace parser
{
    public partial class SavePostersWindow : Window
    {

        public SavePostersWindow(MovieMainViewModel movieViewModel)
        {
            InitializeComponent();
            DataContext = movieViewModel;
        }
    }
}
