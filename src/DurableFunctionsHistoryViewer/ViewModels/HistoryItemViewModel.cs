using System;
using System.Collections.Generic;
using System.Text;

namespace DurableFunctionsHistoryViewer.ViewModels
{
    public class HistoryItemViewModel
    {
        public string Row { get; set; }

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
