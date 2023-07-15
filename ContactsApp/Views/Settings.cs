using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ContactsApp
{
    public static class Settings
    {
        public static event EventHandler OnSettingsChange;

        private static bool _sortByFirstName;
        public static bool SortByFirstName
        {
            get
            {
                return _sortByFirstName;
            }
            set
            {
                if(value != null)
                {
                    _sortByFirstName = value;
                    //OnSettingsChange?.Invoke();
                    OnPropertyChanged(nameof(SortByFirstName));
                }
            }
        }

        //public static event PropertyChangedEventHandler PropertyChanged;
        private static void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnSettingsChange?.Invoke(typeof(Settings), new PropertyChangedEventArgs(propertyName));
        }

        public static int BirthdayRange { get; set; } = 15;

    }
}
