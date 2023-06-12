using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsApp.Views
{
    public sealed class TestView
    {
        private static readonly TestView instance = new TestView();

        private TestView() { }

        public static TestView Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
