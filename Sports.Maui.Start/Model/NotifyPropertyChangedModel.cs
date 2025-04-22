namespace Sports.Maui.Start.Model;

using System.ComponentModel;
using System.Runtime.CompilerServices;

public abstract class NotifyPropertyChangedModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
