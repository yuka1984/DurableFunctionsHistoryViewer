using System;
using System.Text;

namespace DurableFunctionsHistoryViewer.ViewModels
{
    public class IndexItemViewModel
    {
        public string InstanceId { get; set; }

        public DateTimeOffset CreatedTime { get; set; }

        public string ExecutionId { get; set; }

        public string Input { get; set; }

        public string Output { get; set; }

        public DateTimeOffset LastUpdatedTime { get; set; }

        public string Name { get; set; }

        public string RuntimeStatus { get; set; }

        public string Version { get; set; }

        public string DetailUrl { get; set; }
    }
}
