using System;
using System.Collections.Generic;
using System.Text;

namespace AmigueInvisible.Model
{
    public class Assignment
    {
        public string Text { get; set; }
        public Participant Assignee { get; set; }
        public Participant AssignedTo { get; set; }
    }
}
