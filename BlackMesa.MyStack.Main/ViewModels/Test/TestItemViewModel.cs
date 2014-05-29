using System;
using BlackMesa.MyStack.Main.Models;

namespace BlackMesa.MyStack.Main.ViewModels.Test
{
    public class TestItemViewModel
    {
        public string FolderId { get; set; }
        public string TestId { get; set; }
        public string CardId { get; set; }
        public string FrontSide { get; set; }
        public string BackSide { get; set; }
        public DateTime StartTime { get; set; }
        public TestResult Result { get; set; }
        public int CardsLeft { get; set; }
        public bool ReturnToDetailsView { get; set; }
    }
}