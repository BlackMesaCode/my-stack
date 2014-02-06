using System;
using System.Collections.Generic;

namespace BlackMesa.MyStack.Main.Models
{
    [Serializable]
    public class BrowseList
    {
        public List<Card> Cards { get; set; }
        public int CardsCount { get; set; }
        public int Position { get; set; }
    }
}