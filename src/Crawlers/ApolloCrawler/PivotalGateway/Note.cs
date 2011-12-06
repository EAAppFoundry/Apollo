using System;

namespace Apollo.PivotalGateway
{
    public class Note
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public string Noted_At { get; set; }
        public DateTime NotedAt
        {
            get { return DateFormatter.Format(Noted_At); }
        }
    }
}
