using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Dictionary_POL_ENG
{
    /// <summary>
    /// Logika interakcji dla klasy AddWordWindow.xaml
    /// </summary>
     partial class AddWordWindow : Window
    {

        private Dictionary<string, Dictionary<string, string>> dictionaryTable = new Dictionary<string, Dictionary<string, string>>();
        private List<string> dictionaries_list = new List<string>();
        private Dictionary<string, List<string>> dictionary_keys_list = new Dictionary<string, List<string>>();
        Dictionary<string, string> dictionary_eng_word= new Dictionary<string, string>();
        List<string> eng_words = new List<string>();

        static string filename = "Dictionary.json";
        static string filename_2 = "Vocabulary_ENG.json";
        static string filename_3 = "BasicVocabulary_ENG.json";
        static string filename_4 = "Settings.json";
        static string filename_5 = "BasicVocabulary_PL.json";
        static string filename_6 = "Vocabulary_PL.json";

        public static string Address = Path.Combine(Directory.GetCurrentDirectory(), filename);
        public static string Address_2 = Path.Combine(Directory.GetCurrentDirectory(), filename_2);
        public static string Address_3 = Path.Combine(Directory.GetCurrentDirectory(), filename_3);
        public static string Address_4 = Path.Combine(Directory.GetCurrentDirectory(), filename_4);
        public static string Address_5 = Path.Combine(Directory.GetCurrentDirectory(), filename_5);
        public static string Address_6 = Path.Combine(Directory.GetCurrentDirectory(), filename_6);



        bool _switch;

        private string json = null;

        public AddWordWindow(bool _Switch, Dictionary<string, Dictionary<string, string>> DictionaryTable, List<string> Dictionaries_list,
            Dictionary<string, List<string>> Dictionary_keys_list, Dictionary<string, string> Dictionary_eng_word,
            List<string> Eng_words)
        {
            InitializeComponent();
            _switch= _Switch;
            dictionaryTable=DictionaryTable;
            dictionaries_list=Dictionaries_list;
            dictionary_keys_list=Dictionary_keys_list;
            dictionary_eng_word= Dictionary_eng_word;
            eng_words= Eng_words;

            //if(_switch)
            //{
            //    AddEngWord();
            //}
            //else
            //{
            //    AddPlWord();
            //}

            //json = ManagementClass.ReadFile().Result;
        }


        private async void AddEngWord()
        {
            if ((polish_word.Text=="")||(englsh_word.Text==""))
            {
                ShowMessage(1000, 2);
            }
            else
            {
                if (!dictionary_eng_word.ContainsKey(englsh_word.Text))
                {
                    dictionary_eng_word.Add(englsh_word.Text, polish_word.Text);
                    eng_words.Add(englsh_word.Text);
                }
                else
                {
                    ShowMessage(3000, 1);
                }

            }
        }

        private void AddPlWord()
        {
            int count = 1;
            List <bool> bools= new List<bool>();//Table to check which mean 
            if ((polish_word.Text == "") || (englsh_word.Text == ""))
            {
                ShowMessage(1000, 2);
            }
            else
            {

                foreach (var x in dictionaries_list)
                {
                    if (!dictionaryTable[x].ContainsKey(polish_word.Text.ToLower()))
                    {
                        dictionaryTable[x].Add(polish_word.Text.ToLower(), englsh_word.Text);
                        break;
                    }
                    count++;
                }

                if(count>dictionaries_list.Count) 
                {
                    string x = "mean" + count.ToString();
                    var dic= new Dictionary<string, string>() { { polish_word.Text.ToLower(), englsh_word.Text } };
                    dictionaryTable.Add(x, dic);
                    dictionaries_list.Add(x);
                }
            }
        }

        private async Task ShowMessage(int miliseconds, int option)
        {

            WordLabel.Visibility = Visibility.Visible;
            switch (option)
            {
                case 1: 
                    WordLabel.Content = "The word aleready exist !!!";
                    break;
                case 2:
                    WordLabel.Content = "Too enough data !!!";
                    break;
            }

            await Task.Delay(miliseconds);
            WordLabel.Visibility = Visibility.Hidden;
        }

        public void addWord(string value_pl, string value_eng)
        {
            if (_switch)
            {
                foreach (var x in dictionaries_list)
                {
                    if (!dictionaryTable[x].ContainsKey(value_pl))
                    {
                        using (StreamWriter writer = new StreamWriter(Address_6))
                        {
                            dictionaryTable[x].Add(value_pl, value_eng);
                            string js = JsonConvert.SerializeObject(dictionaryTable);
                            writer.WriteLine(js);
                            writer.Close();
                        }
                        break;
                    }
                }
            }

            else
            {
                foreach (var x in dictionaries_list)
                {
                    if (!eng_words.Contains(x))
                    {
                        using (StreamWriter writer = new StreamWriter(Address_2))
                        {
                            dictionary_eng_word.Add(value_pl, value_eng);
                            string js = JsonConvert.SerializeObject(dictionaryTable);
                            writer.WriteLine(js);
                            writer.Close();
                        }
                        break;
                    }
                }
            }
        }



        private void Add_word_Click(object sender, RoutedEventArgs e)
        {
            if(_switch) 
            {
                AddPlWord();
            }
            else
            {
                AddEngWord();
            }

            //var eng_word=englsh_word.Text;
            //var pol_word=polish_word.Text;
            //addWord(pol_word, eng_word);
            //englsh_word.Text = null;
            //polish_word.Text = null;
        }


        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //MainWindow.Visibility= Visibility.Hidden;
            using (StreamWriter writer = new StreamWriter(Address_6))
            {
                string data = JsonConvert.SerializeObject(dictionaryTable);
                await writer.WriteLineAsync(data);
            }

            using (StreamWriter writer = new StreamWriter(Address_2))
            {
                string data = JsonConvert.SerializeObject(dictionary_eng_word);
                await writer.WriteLineAsync(data);
            }

            this.Close();
        }
    }
}
