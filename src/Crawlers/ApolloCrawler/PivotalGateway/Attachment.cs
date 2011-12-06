using System;

namespace Apollo.PivotalGateway
{
    public class Attachment
    {
        public int Id { get; set; }
        public string Filename { get; set; }
        public string Description { get; set; }
        public string Uploaded_By { get; set; }
        public DateTime UploadedAt
        {
            get
            {
                return DateFormatter.Format(Uploaded_At);
            }
        }
        public string Uploaded_At { get; set; }
        public string Url { get; set; }
    }
}