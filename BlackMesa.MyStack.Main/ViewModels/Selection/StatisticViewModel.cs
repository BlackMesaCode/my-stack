using System;
using System.Collections.Generic;

namespace BlackMesa.MyStack.Main.ViewModels.Selection
{
    public class StatisticViewModel
    {
        public string FolderId { get; set; }

        public int SelectedCount { get; set; }

        public double AverageAnwserTime { get; set; }
        public double AverageLevel { get; set; }
        public double AverageAge { get; set; }
        public double AverageRepititions { get; set; }

        public int TestItemsCount { get; set; }

        public double CorrectPercentage { get; set; }
        public double PartlyCorrectPercentage { get; set; }
        public double WrongPercentage { get; set; }
        public double DuePercentage { get; set; }

        public int CorrectCount { get; set; }
        public int PartlyCorrectCount { get; set; }
        public int WrongCount { get; set; }
        public int DueCount { get; set; }
        
        public Models.Card LatestCard { get; set; }
        public Models.Card OldestCard { get; set; }
        public IEnumerable<Models.Card> TopCards { get; set; }
        public IEnumerable<Models.Card> FlopCards { get; set; }
    }
}