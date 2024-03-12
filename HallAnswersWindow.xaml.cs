using System.Windows;
using WhoWantsToBeAMillionaire.Models;
using WhoWantsToBeAMillionaire.ViewModels;

namespace WhoWantsToBeAMillionaire
{
    /// <summary>
    /// Логика взаимодействия для HallAnswersWindow.xaml
    /// </summary>
    public partial class HallAnswersWindow : Window
    {
        public HallAnswersWindow(Question question)
        {
            DataContext = new HallAnswersViewModel(question);

            InitializeComponent();
        }
    }
}
