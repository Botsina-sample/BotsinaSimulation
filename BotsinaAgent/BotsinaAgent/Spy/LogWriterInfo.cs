using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotsinaAgent.Spy
{
    class LogWriterInfo
    {
        private string _username;
        private string _direcory;
        private string _currenttime;

        public string username { get { return _username; } set { _username = value; } }
        public string directory { get { return _direcory; } set { _direcory = value; } }
        public string currenttime { get { return _currenttime; } set { _currenttime = value; } }
    }
}
