using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace DurableFunctionsHistoryViewer.Models
{
    public class HistoryTableEntity : TableEntity
    {
        public int EventId { get; set; }

        public string EventType { get; set; }

        public string ExecutionId { get; set; }

        public bool IsPlayed { get; set; }

        public string Result { get; set; }

        public int TaskScheduledId { get; set; }

        public string Name { get; set; }

        public string OrchestrationInstance { get; set; }
        
        public string Version { get; set; }

        public string OrchestrationStatus { get; set; }

        public string Detail { get; set; }

        public string Reason { get; set; }

        public string Input { get; set; }


    }
}
