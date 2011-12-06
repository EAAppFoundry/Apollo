﻿using System;
using System.Collections.Generic;
using System.IO;
using ApolloCrawler.Mapping;
using Microsoft.Practices.ServiceLocation;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.TeamFoundation.Client;
using SolrNet;

namespace ApolloCrawler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Startup.Init<RequirementsDocument>("http://localhost:8983/solr");

            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<RequirementsDocument>>();
            solr.Commit();

            Uri collectionUri2010 = new Uri("http://eavtfs2010:8080/tfs");
            Uri collectionUri2008 = new Uri("http://eavvsts01:8080/");

            var workItemStore2008 = GetWorkItemStore(collectionUri2008);
            var workItemStore2010 = GetWorkItemStore(collectionUri2010);

            //TODO: Move this to a configuration file
            var mappers = new List<IMapper>();
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