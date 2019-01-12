using System;
using System.Collections.Generic;
using System.Text;

namespace DurableFunctionsHistoryViewer.ViewModels
{
    public class HistoryViewModel
    {
        public string Code { get; set; }
        public string InstanceId { get; set; }
        public List<HistoryItemViewModel> List { get; set; }
        public string ErrorMessage { get; set; }
    }
}
