using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Winsomnia.Utility
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises OnPropertyChanged when property changes.
        /// </summary>
        /// <param name="name">string representation of the property name</param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}