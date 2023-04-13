using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary_POL_ENG
{
    public struct ReturnWordStruct
    {
        public Dictionary<string, Dictionary<string, string>> DictionaryTable { get; set; }
        public List<string> Dictionaries_list { get; set; }
        public Dictionary<string, List<string>> Dictionary_keys_list { get; set; }
        public Dictionary<string, string> Eng_words { get; set; }
        public Dictionary<string, string> Dictionary_eng_words { get; set; }
        public Dictionary<string, string> Settings { get; set; }
    }
}
