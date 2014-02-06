﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BlackMesa.MyStack.Main.Resources;

namespace BlackMesa.MyStack.Main.Models
{
    [Table("MyStack_Cards")]
    public class Card
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }

        [Required]
        [Display(ResourceType = typeof(Strings), Name = "DateCreated")]
        public DateTime DateCreated { get; set; }

        [Required]
        [Display(ResourceType = typeof(Strings), Name = "DateEdited")]
        public DateTime DateEdited { get; set; }

        public bool IsSelected { get; set; }

        public int Position { get; set; }

        public Guid FolderId { get; set; }  // Having both the ForeignKey and the navigation property in place, will make FolderId a not nullable ForeignKey in the database

        public virtual Folder Folder { get; set; }

        public virtual List<TestItem> TestItems { get; set; }

        //[StringLength(255)] // todo reactivate after population
        public string FrontSide { get; set; }

        //[StringLength(10000)] // todo reactivate after population
        [DataType(DataType.MultilineText)]
        public string BackSide { get; set; }

        [NotMapped]
        public int Level
        {
            get
            {
                var testItems = TestItems.OrderBy(i => i.EndTime).ToList();

                int level = 0;

                if (testItems.Count == 1)
                {
                    if (testItems.Single().Result == TestResult.Correct)
                        level++;
                    else
                        level--;
                }
                else if (testItems.Count > 1)
                {
                    for (int i = 1; i < testItems.Count(); i++)
                    {
                        if (testItems[i].EndTime - testItems[i - 1].EndTime >= GetTimespanByLevel(level))
                        {
                            if (testItems[i].Result == TestResult.Correct)
                                level++;
                            else
                                level--;
                        }
                    }
                }

                return level;
            }
        }

        [NotMapped]
        public bool IsDue {
            get
            {
                if (TestItems.Count == 0)
                    return true;
                else
                {
                    var lastTestedItem = TestItems.OrderBy(i => i.EndTime).Last();
                    return DateTime.Now - lastTestedItem.EndTime >= GetTimespanByLevel(Level);
                }
            }
        }

        public static TimeSpan GetTimespanByLevel(int level)
        {
            switch (level)
            {
                case 0:
                    return TimeSpan.Zero;
                case 1:
                    return TimeSpan.FromHours(16);
                case 2:
                    return TimeSpan.FromDays(7);
                case 3:
                    return TimeSpan.FromDays(30);
                default:
                    return TimeSpan.FromDays(180);
            }
        }

    }
}
