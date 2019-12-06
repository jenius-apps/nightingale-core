using System.ComponentModel;

namespace Nightingale.Core.Models
{
    public abstract class ObservableBase : ModifiableBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            ObjectModified();
        }
    }
}
