using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace WhoWantsToBeAMillionaire.Models
{
    public partial class Question : ObservableObject
    {
        public Question()
        {
            Answers = new ObservableCollection<Answer>();
        }

        public string Content { get; set; }

        [ObservableProperty]
        private ObservableCollection<Answer> answers;
        public QuestionLevel Level { get; set; }
    }
}
