using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Nightingale.Core.Models
{
    public class WorkspaceResponse : ObservableBase, IWorkspaceResponse
    {
        private bool? _testsAllPass;

        public bool Successful { get; set; }

        public int StatusCode { get; set; }

        public string StatusDescription { get; set; }

        public long TimeElapsed { get; set; }

        public byte[] RawBytes { get; set; }

        public bool? TestsAllPass
        {
            get => _testsAllPass;
            set
            {
                if (_testsAllPass == value)
                {
                    return;
                }

                _testsAllPass = value;
                RaisePropertyChanged("TestsAllPass");
            }
        }

        public string Body { get; set; }

        public string ContentType { get; set; }

        public IList<KeyValuePair<string, string>> Cookies { get; } = new List<KeyValuePair<string, string>>();

        public IList<KeyValuePair<string, string>> Headers { get; } = new List<KeyValuePair<string, string>>();

        public IList<ApiTestResult> TestResults { get; } = new ObservableCollection<ApiTestResult>();

        public string Log { get; set; }
    }
}
