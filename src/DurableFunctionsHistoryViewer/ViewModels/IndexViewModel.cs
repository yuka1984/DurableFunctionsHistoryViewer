using System.Collections.Generic;

namespace DurableFunctionsHistoryViewer.ViewModels
{
    public class IndexViewModel
    {
        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string OrchestratorName { get; set; }

        public string Code { get; set; }

        public List<IndexItemViewModel> List { get; set; }

    }
}