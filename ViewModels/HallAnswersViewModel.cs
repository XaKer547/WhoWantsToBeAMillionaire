using LiveCharts;
using LiveCharts.Wpf;
using System;
using WhoWantsToBeAMillionaire.Models;

namespace WhoWantsToBeAMillionaire.ViewModels
{
    public class HallAnswersViewModel
    {
        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> Formatter { get; set; }
        public HallAnswersViewModel(Question question)
        {
            SeriesCollection = new SeriesCollection();

            var votes = GetVotes();

            int i = 0;
            foreach (var answer in question.Answers)
            {
                SeriesCollection.Add(new ColumnSeries
                {
                    Title = answer.Content,
                    Values = new ChartValues<int> { votes[i] },
                    ColumnPadding = 10,
                    MinWidth = 100,
                    Width = 100,
                });
                i++;
            }

            Formatter = value => value.ToString("N");
        }

        private int[] GetVotes()
        {
            const int k = 4;
            const int sum = 100;

            var rnd = new Random();
            var x = new int[k + 1];

            x[0] = 0;
            x[k] = sum;

            for (int i = 1; i < k; i++)
            {
                x[i] = rnd.Next(0, sum + 1);
            }

            Array.Sort(x);

            var n = new int[k];
            for (int i = 0; i < k; i++)
            {
                n[i] = x[i + 1] - x[i];
            }

            return n;
        }
    }
}
