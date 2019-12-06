using Nightingale.Core.Models;
using System;

namespace Nightingale.Core.History
{
    public class HistoryRequest : WorkspaceRequest, IHistoryItem
    {
        public HistoryRequest()
        {
        }

        public HistoryRequest(WorkspaceRequest request, DateTime lastUsed)
        {
            this.LastUsedDate = lastUsed;
            this.Name = request.Name;
            this.MethodIndex = request.MethodIndex;
            this.BaseUrl = request.BaseUrl;
            this.Body = request.Body.DeepClone() as RequestBody;
            this.Authentication = request.Authentication.DeepClone() as Authentication;
            this.Status = ModifiedStatus.New;

            foreach (Parameter p in request.Queries)
            {
                this.Queries.Add(p.DeepClone() as Parameter);
            }

            foreach (Parameter h in request.Headers)
            {
                this.Headers.Add(h.DeepClone() as Parameter);
            }

            foreach (ApiTest t in request.ApiTests)
            {
                this.ApiTests.Add(t.DeepClone() as ApiTest);
            }

            foreach (Parameter chainingRule in request.ChainingRules)
            {
                this.ChainingRules.Add(chainingRule.DeepClone() as Parameter);
            }
        }

        public DateTime LastUsedDate { get; set; }

        public override object DeepClone()
        {
            var baseClone = base.DeepClone() as WorkspaceRequest;
            var historyRequest = new HistoryRequest(baseClone, this.LastUsedDate);
            return historyRequest;
        }
    }
}
