using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BlackMesa.MyStack.Main.Resources;
using BlackMesa.MyStack.Main.ViewModels.Query;

namespace BlackMesa.MyStack.Main.Models
{
    [Serializable]
    public class Test
    {
        public Guid Id { get; set; }

        public bool TestOnlyDueCards { get; set; }
        public bool ReverseSides { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Order")]
        public OrderType OrderType { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "TestType")]
        public TestType TestType { get; set; }

        public List<Card> CardsToTest { get; set; }

        public TestStatus TestStatus { get; set; }

        public DateTime StartTime { get; set; }
    }

    public enum TestStatus
    {
        InProgress,
        Paused,
        Completed,
        Aborted,
    }

}
