using parser.ViewModels;
using System.Windows;

namespace parser
{
    public partial class ParsingPagesWindow : Window
    {
        public ParsingPagesWindow(MovieMainViewModel _model)
        {
            InitializeComponent();
            DataContext = _model;
        }
    }
}
