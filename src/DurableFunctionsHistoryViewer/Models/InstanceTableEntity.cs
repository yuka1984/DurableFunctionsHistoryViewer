using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace DurableFunctionsHistoryViewer.Models
{
    public class InstanceTableEntity : TableEntity
    {
        public DateTime CreatedTime { get; set; }

        public string ExecutionId { get; set; }

        public string Input { get; set; }

        public DateTime LastUpdatedTime { get; set; }

        public string Name { get; set; }

        public string RuntimeStatus { get; set; }

        public string Version { get; set; }
    }
}
