using parser.ViewModels;
using System.Windows;

namespace parser
{
    public partial class ParsingPagesWindow : Window
    {
        public ParsingPagesWindow(MovieViewModel _model)
        {
            InitializeComponent();
            DataContext = _model;
        }
    }
}
