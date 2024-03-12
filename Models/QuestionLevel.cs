using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace WhoWantsToBeAMillionaire.Models
{
    public partial class QuestionLevel : ObservableObject
    {
        public int Level { get; set; }
        public float Sum { get; set; }
        public bool IsFixed { get; set; }

        [ObservableProperty]
        private bool isCompleted;

        public Tuple<bool, bool> LevelStatus => new(IsFixed, IsCompleted);
    }
}