using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dictionary_POL_ENG
{
    /// <summary>
    /// Logika interakcji dla klasy WordList.xaml
    /// </summary>
    public partial class WordList : Window
    {
        private Dictionary<string, Dictionary<string, string>> dictionaryTable = new Dictionary<string, Dictionary<string, string>>();
        private List<string> dictionaries_list = new List<string>();
        private Dictionary<string, List<string>> dictionary_keys_list = new Dictionary<string, List<string>>();
        private Dictionary<string, string> dictionary_eng_word = new Dictionary<string, string>();
        private List<string> eng_words = new List<string>();

        Dictionary<string,string> Word_list= new Dictionary<string,string>();
        private bool _switch;


        public WordList
            (bool _Switch, Dictionary<string, Dictionary<string, string>> DictionaryTable, List<string> Dictionaries_list,
            Dictionary<string, List<string>> Dictionary_keys_list, Dictionary<string, string> Dictionary_eng_word,
            List<string> Eng_words)
        {


            dictionaryTable= DictionaryTable;
            dictionaries_list= Dictionaries_list;
            dictionary_keys_list = Dictionary_keys_list;
            dictionary_eng_word=Dictionary_eng_word;
            eng_words= Eng_words;
            _switch = _Switch;


            InitializeComponent();
            if (_switch)
            {
                Word_list = Return_Word_List(ReturnListDictionares());
                Data_grid.ItemsSource = Word_list;
            }
            else
            {
                Word_list = dictionary_eng_word;
                Data_grid.ItemsSource = Word_list;
            }
        }


        public List<Dictionary<string, string>> ReturnListDictionares()//Funkcja zwracająca listę słowników wszystkich
        {
            List<Dictionary<string, string>> new_dictionares = new List<Dictionary<string, string>>();

            foreach (var x in dictionaries_list)
            {
                new_dictionares.Add(dictionaryTable[x]);
            }

            return new_dictionares;
        }



        public Dictionary<string, string> Return_Word_List(List<Dictionary<string, string>> dic)
        {
            Dictionary<string, string> Word_list = new Dictionary<string, string>();
            foreach (var x in dic)
            {
                foreach (var y in x)
                {
                    if (Word_list.Keys.Contains(y.Key))
                    {
                        continue;
                    }
                    else
                    {
                        Word_list.Add(y.Key, y.Value);
                    }
                }
            }

            return Word_list;
        }
    }
}
