using System;
using Microsoft.Practices.ServiceLocation;
using SolrNet;

namespace ApolloTFSCrawler
{
    class Program
    {
        static void Main(string[] args)
        {

            Startup.Init<RequirementsDocument>("http://localhost:8983/solr");

            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<RequirementsDocument>>();

            var doc = new RequirementsDocument()
                          {
                              ID = "requirement1",
                              Title = "This is the title. Tile",
                              Status = "Status1",
                              Project = "Project 1",
                              Department = "Department 1",
                              SystemSource = "System 1",
                              LastIndexed = DateTime.Now,
                              Description =
                                  "This is the description of the requirements. Here are some things to search for. FBD CID Orion"
                          };

            //do a commit just in case
            solr.Commit();

            solr.Add(doc);

            solr.Commit();
        }
    }
}
