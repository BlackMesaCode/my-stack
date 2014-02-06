using System;

namespace BlackMesa.MyStack.Main.ViewModels.Query
{
    public class TestCompletedViewModel
    {
        public string FolderId { get; set; }
        public int TotalCount { get; set; }
        public int CorrectCount { get; set; }
        public int PartlyCorrectCount { get; set; }
        public int WrongCount { get; set; }
        public TimeSpan Duration { get; set; }

    }
}