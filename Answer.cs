using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labatron
{
    struct Answer
    {
        public string answer;
        public string work;

        public Answer(string answer, string work)
        {
            this.work = work;
            this.answer = answer;
        }
    }
}
