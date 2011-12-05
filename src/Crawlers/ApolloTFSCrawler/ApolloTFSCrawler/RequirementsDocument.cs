using System;
using SolrNet.Attributes;

namespace ApolloTFSCrawler
{
    public class RequirementsDocument
    {
        [SolrUniqueKey("id")] 
        public string ID { get; set; }

        [SolrField("title")]
        public string Title { get; set; }

        [SolrField("area")]
        public string Area { get; set; }

        [SolrField("status")]
        public string Status { get; set; }

        [SolrField("description")]
        public string Description { get; set; }

        [SolrField("relatedstories")]
        public string RelatedStories { get; set; }

        [SolrField("team")]
        public string Team { get; set; }

        [SolrField("releaseiteration")]
        public string ReleaseIteration { get; set; }

        [SolrField("storypoints")]
        public string StoryPoints { get; set; }

        [SolrField("acceptancecriteria")]
        public string AcceptanceCriteria { get; set; }

        [SolrField("iterationstart")]
        public DateTime IterationStart { get; set; }

        [SolrField("iterationend")]
        public DateTime IterationEnd { get; set; }

        [SolrField("discipline")]
        public string Discipline { get; set; }

        [SolrField("storytype")]
        public string StoryType { get; set; }

        [SolrField("storytags")]
        public string StoryTags { get; set; }

        [SolrField("storyuri")]
        public string StoryURI { get; set; }

        [SolrField("referenceID")]
        public string ReferenceID { get; set; }

        [SolrField("project")]
        public string Project { get; set; }

        [SolrField("department")]
        public string Department { get; set; }

        [SolrField("systemsource")]
        public string SystemSource { get; set; }

        [SolrField("lastindexed")]
        public DateTime LastIndexed { get; set; }

        [SolrField("lastupdated")]
        public DateTime LastUpdated { get; set; }
    }
}
