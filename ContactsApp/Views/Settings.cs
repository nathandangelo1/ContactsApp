using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ContactsApp
{
    public class Settings
    {
        
        public static int BirthdayRange { get; set; } = 15;
        private static bool? _sortByFirstName = true;

        public static bool? SortByFirstName
        {
            get
            {
                if (_sortByFirstName is null)
                {
                    return true;
                }
                return _sortByFirstName;
            }
            set
            {
                if(value != null && value != _sortByFirstName)
                {
                    _sortByFirstName = value;
                    OnPropertyChanged(nameof(SortByFirstName));

                }
            }
        }

        public static event EventHandler OnSettingsChange;
        
        private static void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnSettingsChange?.Invoke(typeof(Settings), new PropertyChangedEventArgs(propertyName));
        }


    }
}
