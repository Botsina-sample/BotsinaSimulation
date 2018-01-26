using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotsinaAgent.Spy
{
    class SpyResult : LogWriterInfo
    {
        private int _index;
        private string _autoID;
        private string _classname;
        private string _name;


        public int index { get { return _index; } set { _index = value; } }
        public string autoDI { get { return _autoID; } set { _autoID = value; } }
        public string classname { get { return _classname; } set { _classname = value; } }
        public string name { get { return _name; } set { _name = value; } }
    }
}
