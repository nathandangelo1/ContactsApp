﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ContactsApp
{
    public class FL : ObservableCollection<Contact>
    {
        public ObservableCollection<Contact> favorites;
  
        public ObservableCollection<Contact> Favorites
        {
            get { return favorites; }
            set
            {
                favorites = value;
            }
        }
    }
}
