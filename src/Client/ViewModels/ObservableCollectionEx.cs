using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Baustellen.App.Client.ViewModels;

internal class ObservableCollectionEx<T> : ObservableCollection<T>
{
    internal ObservableCollectionEx()
    {
    }

    internal ObservableCollectionEx(IEnumerable<T> collection) : base(collection)
    {
    }

    internal ObservableCollectionEx(List<T> list) : base(list)
    {
    }

    internal void ReloadData(IEnumerable<T> items)
    {
        ReloadData(
            innerList =>
            {
                foreach (var item in items)
                {
                    innerList.Add(item);
                }
            });
    }

    internal void ReloadData(Action<IList<T>> innerListAction)
    {
        Items.Clear();

        innerListAction(Items);

        OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
        OnPropertyChanged(new PropertyChangedEventArgs("Items[]"));
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    internal async Task ReloadDataAsync(Func<IList<T>, Task> innerListAction)
    {
        Items.Clear();

        await innerListAction(Items);

        OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
        OnPropertyChanged(new PropertyChangedEventArgs("Items[]"));
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
}
