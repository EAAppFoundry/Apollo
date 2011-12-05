using System;
using System.Collections.Generic;
using System.IO;
using ApolloTFSCrawler.TFSMapping;
using Microsoft.Practices.ServiceLocation;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.TeamFoundation.Client;
using SolrNet;

namespace ApolloTFSCrawler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Startup.Init<RequirementsDocument>("http://localhost:8983/solr");

            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<RequirementsDocument>>();

            //var doc = new RequirementsDocument()
            //              {
            //                  ID = "requirement1",
            //                  Title = "This is the title. Tile",
            //                  Status = "Status1",
            //                  Project = "Project 1",
            //                  Department = "Department 1",
            //                  SystemSource = "System 1",
            //                  LastIndexed = DateTime.Now,
            //                  Description =
            //                      "This is the description of the requirements. Here are some things to search for. FBD CID Orion"
            //              };

            ////do a commit just in case
            solr.Commit();

            Uri collectionUri2010 = new Uri("http://eavtfs2010:8080/tfs");
            Uri collectionUri2008 = new Uri("http://eavvsts01:8080/");

            var workItemStore2008 = GetWorkItemStore(collectionUri2008);
            var workItemStore2010 = GetWorkItemStore(collectionUri2010);

            var mappers = new List<ITFSMapper>();
            mappers.Add(new ScrumMapper("Commercials (MTS)", workItemStore2008));
            mappers.Add(new ScrumMapper("Millennium", workItemStore2008));
            mappers.Add(new ScrumMapper("NetOps", workItemStore2008));
            mappers.Add(new UrbanTurtleMapper("Turner Digital Library", workItemStore2010));
            

            foreach (var tfsMapper in mappers)
            {
                RequirementsDocument[] docs = tfsMapper.FindAllWorkItemsForProject();
                solr.AddRange(docs, new AddParameters { CommitWithin = 10000 });
            }
            
            solr.Commit();
            solr.Optimize();
            

            //Console.ReadLine();

        }

        private static WorkItemStore GetWorkItemStore(Uri uri)
        {
            // Connect to the server and the store. 
            TfsTeamProjectCollection teamProjectCollection =
                new TfsTeamProjectCollection(uri);

            WorkItemStore workItemStore = teamProjectCollection.GetService<WorkItemStore>();
            return workItemStore;
        }
    }
}