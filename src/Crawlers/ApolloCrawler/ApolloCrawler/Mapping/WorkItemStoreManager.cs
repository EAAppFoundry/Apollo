using System;
using System.Collections.Generic;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace ApolloCrawler.Mapping
{
    public class WorkItemStoreManager
    {
        private static Dictionary<string, WorkItemStore> _workItemStores=new Dictionary<string, WorkItemStore>();

        public static WorkItemStore GetWorkItemStoreForUrl(string url)
        {
            if (_workItemStores.ContainsKey(url))
                return _workItemStores[url];
            else
            {
                var uri = new Uri(url);
                // Connect to the server and the store. 
                TfsTeamProjectCollection teamProjectCollection =
                    new TfsTeamProjectCollection(uri);

                

                WorkItemStore workItemStore = teamProjectCollection.GetService<WorkItemStore>();

                _workItemStores.Add(url,workItemStore);
                return workItemStore;
            }
        }
}
}
