using RecordsViewer.Portable.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsViewer.Portable
{
    public class PortableStrings
    {
        private static Strings _strings = new Strings();

        public Strings Strings { get { return _strings; } }
    }
}
