using System;
using System.Collections.Generic;

namespace Apollo.PivotalGateway
{
    public class Project
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Iteration_Length { get; set; }
        public string Week_Start_Day { get; set; }
        public string Point_Scale { get; set; }
        public string Account { get; set; }
        public string Velocity_Scheme { get; set; }
        public int Current_Velocity { get; set; }
        public int Initial_Velocity { get; set; }
        public string Number_of_Done_Iterations_To_Show { get; set; }
        public string Labels { get; set; }
        public DateTime LastActivityAt
        {
            get { return DateFormatter.Format(Last_Activity_At); }
        }
        public string Last_Activity_At { get; set; }
        public bool Allow_Attachments { get; set; }
        public bool Public { get; set; }
        public bool Use_Https { get; set; }
        public bool Bugs_and_Chores_Are_Estimatable { get; set; }
        public bool Commit_Mode { get; set; }
        public List<Membership> Memberships { get; set; }
    }
}
