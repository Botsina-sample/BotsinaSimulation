using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotsinaAgent.Spy
{
    class SpyIMG
    {
        private int _imgID;
        private string _imgClassName;

        public int imgID { get { return _imgID; } set { _imgID = value; } }
        public string imgClassName { get { return _imgClassName; } set { _imgClassName = value; } }
    }
}
