using System;
using System.Collections.Generic;

namespace Apollo.PivotalGateway
{
    public class Iteration
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public DateTime StartDate { get { return DateFormatter.Format(Start); } }
        public string Start { get; set; }
        public DateTime? FinishDate { get { return DateFormatter.FormatNullable(Finish); } }
        public string Finish { get; set; }
        public List<Story> Stories { get; set; }

        public TimeSpan Length
        {
            get { return FinishDate == null ? new TimeSpan(21, 0, 0, 0) : new TimeSpan(FinishDate.Value.Ticks - StartDate.Ticks); }
        }
    }
}