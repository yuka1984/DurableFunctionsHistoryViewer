using System;
using System.Text;

namespace DurableFunctionsHistoryViewer.ViewModels
{
    public class IndexItemViewModel
    {
        public string InstanceId { get; set; }

        public DateTime CreatedTime { get; set; }

        public string ExecutionId { get; set; }

        public string Input { get; set; }

        public DateTime LastUpdatedTime { get; set; }

        public string Name { get; set; }

        public string RuntimeStatus { get; set; }

        public string Version { get; set; }

        public string DetailUrl { get; set; }
    }
}
