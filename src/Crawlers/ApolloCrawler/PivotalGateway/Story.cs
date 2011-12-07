using System;
using System.Collections.Generic;

namespace Apollo.PivotalGateway
{
    /*
     *     <id type="integer">STORY_ID</id>
    <project_id type="integer">PROJECT_ID</project_id>
    <story_type>feature</story_type>
    <url>http://www.pivotaltracker.com/story/show/STORY_ID</url>
    <estimate type="integer">1</estimate>
    <current_state>accepted</current_state>
    <lighthouse_id>43</lighthouse_id>
    <lighthouse_url>http://mylighthouseapp.com/projects/100/tickets/43</lighthouse_url>
    <description></description>
    <name>More power to shields</name>
    <requested_by>James Kirk</requested_by>
    <owned_by>Montgomery Scott</owned_by>
    <created_at type="datetime">2008/12/10 00:00:00 UTC</created_at>
    <accepted_at type="datetime">2008/12/10 00:00:00 UTC</accepted_at>
    <labels>label 1,label 2,label 3</labels>
    <attachments type="array">
      <attachment>
        <id type="integer">4</id>
        <filename>shield_improvements.pdf</filename>
        <description>How to improve the shields in 3 easy steps.</description>
        <uploaded_by>James Kirk</uploaded_by>
        <uploaded_at type="datetime">2008/12/10 00:00:00 UTC</uploaded_at>
      </attachment>
    </attachments>
     * */
    public class Story
    {
        public int Id { get; set; }
        public int Project_Id { get; set; }
        public string Story_Type { get; set; }
        public string Url { get; set; }
        public int Estimate { get; set; }
        public string Current_State { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Requested_By { get; set; }
        public string Owned_By { get; set; }

        public DateTime CreatedAt
        {
            get { return DateFormatter.Format(Created_At); }
        }

        public string Created_At { get; set; }

        public DateTime AcceptedAt
        {
            get { return DateFormatter.Format(Accepted_At); }
        }
        public string Accepted_At { get; set; }

        public DateTime UpdatedAt
        {
            get { return DateFormatter.Format(Updated_At); }
        }

        public string Updated_At { get; set; }

        public string Labels { get; set; }
        //public List<Attachment> Attachments { get; set; }
        public List<Note> Notes { get; set; }
        //public List<Task> Tasks { get; set; }
    }
}
