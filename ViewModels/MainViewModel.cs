using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WhoWantsToBeAMillionaire.Models;
using WhoWantsToBeAMillionaire.Models.Enums;
using WhoWantsToBeAMillionaire.Seeder;

namespace WhoWantsToBeAMillionaire.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            InitGame();
        }

        private void InitGame()
        {
            lastFixedMoney = 0f;
            CurrentMoney = 0f;
            CurrentLevel = 0;

            CanPlay = true;
            canUseBonus = true;
            canAskHall = true;
            canHalf = true;

            var questions = QuestionSeeder.GetQuestions();
            Questions = new List<Question>(questions);

            Levels = Questions.Select(q => q.Level).ToList();

            totalLevels = Questions.Count - 1;

            CurrentQuestion = Questions[CurrentLevel];
        }

        [ObservableProperty]
        private int currentLevel;

        [ObservableProperty]
        public List<Question> questions;

        [ObservableProperty]
        private Question currentQuestion;

        [ObservableProperty]
        private IReadOnlyCollection<QuestionLevel> levels;

        [ObservableProperty]
        private float currentMoney;

        private int totalLevels;

        private float lastFixedMoney;

        private void NextLevel()
        {
            if (CurrentLevel == totalLevels)
            {
                FinishGame(GameOverReasons.Victory);
                return;
            }

            CurrentLevel++;

            var level = CurrentQuestion.Level;

            Levels.Single(l => l == level).IsCompleted = true;

            if (level.IsFixed)
                lastFixedMoney = level.Sum;

            CurrentMoney = level.Sum;
            CurrentQuestion = Questions[CurrentLevel];

            canUseBonus = true;
        }

        private bool CanPlay { get; set; } = true;
        [RelayCommand(CanExecute = nameof(CanPlay))]
        public void CheckAnswer(Answer answer)
        {
            if (!answer.IsCorrect)
            {
                CurrentMoney = lastFixedMoney;
                FinishGame(GameOverReasons.Fail);
                return;
            }

            NextLevel();
            return;
        }

        [RelayCommand]
        public void FinishGame(GameOverReasons reason)
        {
            CanPlay = false;

            var status = GetGameStatus(reason);

            var description = $"{status.Description}\n\n";

            description += CurrentMoney == 0 ? "Вы не смогли ничего заработать" : $"Вы заработали {CurrentMoney:C}";

            if (MessageBox.Show(description, status.Title, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                InitGame();
            }
        }

        private GameOverDTO GetGameStatus(GameOverReasons reason)
        {
            return reason switch
            {
                GameOverReasons.Fail => new GameOverDTO()
                {
                    Description = "К сожалению вы проиграли!",
                    Title = "Поражение"
                },
                GameOverReasons.Exit => new GameOverDTO()
                {
                    Description = "Вы решили покинуть игру",
                    Title = "Уход"
                },
                GameOverReasons.Victory => new GameOverDTO()
                {
                    Description = "Вы победили!",
                    Title = "Победа"
                },
                _ => throw new System.Exception()
            };
        }

        #region Bonuses
        private bool canUseBonus = true;

        [ObservableProperty]
        private string askHallImagePath = "/Assets/people.png";

        private bool canAskHall = true;
        private bool CanExecuteAsk => canAskHall && canUseBonus && CanPlay;
        [RelayCommand(CanExecute = nameof(CanExecuteAsk))]
        public void AskHall()
        {
            var stats = new HallAnswersWindow(CurrentQuestion);

            stats.Show();

            canUseBonus = false;
            canAskHall = false;
            AskHallImagePath = null;
        }

        [ObservableProperty]
        private string halfAnswersImagePath = "/Assets/half.png";
        private bool canHalf = true;
        private bool CanExecuteHalf => canHalf && canUseBonus && CanPlay;
        [RelayCommand(CanExecute = nameof(CanExecuteHalf))]
        public void HalfAnswers()
        {
            var incorrectAnswers = CurrentQuestion.Answers.Where(a => !a.IsCorrect)
                .Take(2).ToArray();

            foreach (var answer in incorrectAnswers)
            {
                CurrentQuestion.Answers.Remove(answer);
            }

            canHalf = false;
            canUseBonus = false;
            HalfAnswersImagePath = null;
        }
        #endregion
    }
}
