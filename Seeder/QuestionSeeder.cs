using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Windows;
using WhoWantsToBeAMillionaire.Models;

namespace WhoWantsToBeAMillionaire.Seeder
{
    public static class QuestionSeeder
    {
        private static readonly Random random = new();
        public static IReadOnlyCollection<Question> GetQuestions()
        {
            var questions = new List<Question>();

            var easyQuestions = GetEasyQuestions();

            questions.AddRange(easyQuestions);

            questions.AddRange(GetMediumQuestions());

            questions.AddRange(GetHardQuestions());

            var levels = GetQuestionLevels();

            for (int i = 0; i < questions.Count; i++)
                questions[i].Level = levels.ElementAt(i);

            return questions;
        }

        public static Question[] GetEasyQuestions()
        {
            return GetQuestionsByLevel("easy");
        }
        public static Question[] GetMediumQuestions()
        {
            return GetQuestionsByLevel("medium");
        }
        public static Question[] GetHardQuestions()
        {
            return GetQuestionsByLevel("hard", 4);
        }
        public static Question[] GetQuestionsByLevel(string level)
        {
            return GetQuestionsByLevel(level, 3);
        }
        public static Question[] GetQuestionsByLevel(string level, int amount)
        {
            var res = Application.GetResourceStream(new Uri($"Seeder/Questions/questions_{level}.json", UriKind.Relative));

            var questions = JsonSerializer.Deserialize<IEnumerable<Question>>(res.Stream)
                .ToArray();

            random.Shuffle(questions);

            var array = questions.Take(amount)
                .ToArray();

            foreach (var question in array)
            {
                var answers = question.Answers.ToArray();

                random.Shuffle(answers);

                question.Answers = new(answers);
            }

            return array;
        }

        public static IReadOnlyCollection<QuestionLevel> GetQuestionLevels()
        {
            var levels = new[]
            {
                new { Sum = 500, Fixed = false },
                new { Sum = 1000, Fixed = false },
                new { Sum = 2000, Fixed = false },
                new { Sum = 3000, Fixed = true },
                new { Sum = 5000, Fixed = false },
                new { Sum = 10000, Fixed = false },
                new { Sum = 15000, Fixed = false},
                new { Sum = 25000, Fixed = true },
                new { Sum = 50000, Fixed = false },
                new { Sum = 100000, Fixed = false },
            };

            return levels.Select((l, i) => new QuestionLevel
            {
                Level = i + 1,
                Sum = l.Sum,
                IsFixed = l.Fixed,
            }).ToArray();
        }

        public static void Shuffle<T>(this Random rng, T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                (array[k], array[n]) = (array[n], array[k]);
            }
        }
    }
}
