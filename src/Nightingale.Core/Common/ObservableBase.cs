using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Nightingale.Core.Common
{
    /// <summary>
    /// Base class that implements
    /// INotifyPropertyChanged.
    /// </summary>
    public abstract class ObservableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Triggers property changed event.
        /// </summary>
        /// <param name="propertyName">Optional. 
        /// Name of property that changed.
        /// Uses caller member name by default.</param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
