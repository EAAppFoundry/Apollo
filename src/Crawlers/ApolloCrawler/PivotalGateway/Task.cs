using System;

namespace Apollo.PivotalGateway
{
    public class Task
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Position { get; set; }
        public bool Complete { get; set; }
        public string Created_At { get; set; }
        //public DateTime CreatedAt { get; set; }
        public string CreatedAt { get; set; }
    }
}