using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dictionary_POL_ENG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random random= new Random();
        private int learn = 0;
        private Dictionary<string, Dictionary<string, string>> dictionaryTable = new Dictionary<string, Dictionary<string, string>>(); //słownik słowników
        private List<string> dictionaries_list = new List<string>(); //lista słowników
        private Dictionary<string,List<string>> dictionary_keys_list= new Dictionary<string,List<string>>(); //lista słów ze słowników do losowania
        private Dictionary<string,string> dictionary_eng_word = new Dictionary<string,string>();
        private Dictionary<string,string> dic_pl_eng= new Dictionary<string, string>();
        private List<string> pl_words = new List<string>();
        private List<string> eng_words= new List<string>();
        private Dictionary<string, string> settings = new Dictionary<string, string>();
        private int progress_1;
        private int progress_2; 

        private bool _switch=true;

        private string question = null;
        private string answer = null;
        private Q_A_Struct data=new Q_A_Struct();


        public MainWindow()
        {
           // ManagementClass.Return_POL_ENG();
            InitializeComponent();
            DownloadData();     
        }

        protected void Rand_Click(object sender, RoutedEventArgs e)
        {
            if (_switch)
            {
                CheckWord.Text = "";
                Delete_btn.IsEnabled= true;
                CheckWord.IsReadOnly = false;
                ans_label.Content = "Answer";
                CheckButton.Content = "Check";
                data = ReturnWord();
                answer = data.Answer;
                question = data.Word;
                LabelRand.Content = question;
            }
            else
            {
                CheckWord.Text = "";
                Delete_btn.IsEnabled = true;
                CheckWord.IsReadOnly = false;
                ans_label.Content = "Answer";
                CheckButton.Content = "Check";
                data = ReturnWord_2();
                answer = data.Answer;
                question = data.Word;
                LabelRand.Content = question;
            }
        }


        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            var ans = CheckWord.Text.ToLower();
            if (_switch)
            {
                CheckWord.IsReadOnly = true;
                var mean = DownloadMean(data.Word, data.Dictionary_name);

                if (ans == mean)
                {
                    CheckButton.Content = "Good!!! Next Please";
                }
                else
                {
                    CheckButton.Content = "Bad try again!!!";
                }
            }
            else
            {
                var mean = dictionary_eng_word[question];

                if (ans == mean)
                {
                    CheckButton.Content = "Good!!! Next Please";
                    question = null;
                }
                else
                {
                    CheckButton.Content = "Bad try again!!!";
                }
            }
        }



        private void Show_ans_btn_Click(object sender, RoutedEventArgs e)
        {
            ans_label.Content = answer;
        }


        private void Add_Word_Click(object sender, RoutedEventArgs e)
        {
            AddWordWindow new_window = new AddWordWindow(_switch, dictionaryTable, dictionaries_list, dictionary_keys_list,
                dictionary_eng_word, eng_words);
            new_window.Show();
            this.UpdateLayout();
        }


        public async void DownloadData()
        {
            
            var zmienna = await ManagementClass.DownloadData();

            dictionaryTable = zmienna.DictionaryTable;
            dictionaries_list = zmienna.Dictionaries_list;
            dictionary_keys_list = zmienna.Dictionary_keys_list;
            dictionary_eng_word = zmienna.Eng_words;

            /*
            dictionaryTable = ManagementClass.DownloadData().Result.DictionaryTable;
            dictionaries_list = ManagementClass.DownloadData().Result.Dictionaries_list;
            dictionary_keys_list = ManagementClass.DownloadData().Result.Dictionary_keys_list;
            dictionary_eng_word = ManagementClass.DownloadData().Result.Eng_words;
            */
            eng_words =dictionary_eng_word.Keys.ToList();
            pl_words = dic_pl_eng.Keys.ToList();
            settings = await ManagementClass.DownloadSettings();
            progress_1 = Convert.ToInt32(settings["progress_1"]);
            progress_2 = Convert.ToInt32(settings["progress_2"]);

            if (_switch)
            {
                ProgressBar.Value = progress_1;
            }
            else
            {
                ProgressBar.Value = progress_2;
            }
        }

        public string DownloadMean(string word,string dic)
        {
            var dictionary = dictionaryTable[dic];
            return dictionary[word];
        }



        public Q_A_Struct ReturnWord()
        {
            int value=dictionaries_list.Count;
            string dictionary_name = dictionaries_list[random.Next(dictionaries_list.Count)];
            var dictionary = dictionary_keys_list[dictionary_name];
            var dictionary_1 = dictionaryTable[dictionary_name];
            var word = dictionary[random.Next(dictionary.Count)];
            var answer = dictionary_1[word];

            return new Q_A_Struct { Word = word, Answer = answer, Dictionary_name=dictionary_name };

            //var value = pl_words[random.Next(eng_words.Count)];
            //var ans = dic_pl_eng[value];
            //return new Q_A_Struct { Word = value, Answer = ans };
        }

        public Q_A_Struct ReturnWord_2()
        {
            var value = eng_words[random.Next(eng_words.Count)];
            var ans = dictionary_eng_word[value];
            return new Q_A_Struct {Word=value,Answer=ans};
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Q)&&(Keyboard.Modifiers&ModifierKeys.Control)==ModifierKeys.Control)
            {
                CheckWord.Text = "";
                Rand_Click(sender, e);
            }
            else if ((e.Key == Key.E) && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                Show_ans_btn_Click(sender, e);
            }
            else if ((e.Key == Key.D) && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                Delete_btn_Click(sender, e);
            }
            else if ((e.Key == Key.A) && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                CheckButton_Click(sender, e);
            }
            else if ((e.Key == Key.W) && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                Change_mode_Click(sender, e);
            }
        }



        
        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            if(_switch)
            {
                /*
                var value=DeleteWord_PL();
                dictionaryTable = value.DictionaryTable;
                dictionary_keys_list = value.Dictionary_keys_list;
                dictionaries_list=value.Dictionaries_list;
                */
                DeleteWord_PL();
                progress_1++;
                ProgressBar.Value = progress_1;
            }
            else
            {
                DeleteWordENG();
                progress_2++;
                ProgressBar.Value = progress_2;
            }
        }

        public void DeleteWord_PL()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(question, answer);
            for(int i=0;i<dictionaries_list.Count;i++)
            {
                if ((dictionaryTable[dictionaries_list[i]].ContainsKey(question)) && (dictionaryTable[dictionaries_list[i]][question]==answer))
                {
                    dictionaryTable[dictionaries_list[i]].Remove(question);
                    dictionary_keys_list[dictionaries_list[i]].Remove(question);
                    if(dictionaryTable[dictionaries_list[i]].Count==0)
                    {
                        dictionaryTable.Remove(dictionaries_list[i]);
                        dictionaries_list.Remove(dictionaries_list[i]);
                    }

                }
                else
                {
                    continue;
                }
            }
            /*
            return new ReturnWordStruct
            {
                Dictionaries_list = dictionaries_list,
                DictionaryTable = dictionaryTable,
                Dictionary_keys_list = dictionary_keys_list
            };
            */
        }

        public void DeleteWordENG()
        {
            dictionary_eng_word.Remove(question);
        }

        private void Show_list_btn_Click(object sender, RoutedEventArgs e)
        {
            WordList word_list_window= new WordList(_switch,dictionaryTable,dictionaries_list,dictionary_keys_list,
                dictionary_eng_word,eng_words);
            word_list_window.Show();

        }

        private void Change_mode_Click(object sender, RoutedEventArgs e)
        {
            LabelRand.Content = "Rand";
            CheckWord.Text = "";
            ans_label.Content = "Answer";
            if(_switch==true)
            {
                _switch= false;
                Delete_btn.IsEnabled = false;
                ProgressBar.Value = progress_2;
            }
            else
            {
                _switch= true;
                Delete_btn.IsEnabled = false;
                ProgressBar.Value = progress_1;
            }
        }

        private async void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            if(_switch)
            {
                var value = await ManagementClass.Reset_PL();
                dictionaryTable = value.DictionaryTable;
                dictionary_keys_list = value.Dictionary_keys_list;
                dictionaries_list = value.Dictionaries_list;
                progress_1 = 0;
                ProgressBar.Value= progress_1;
            }
            else
            {
               dictionary_eng_word = await ManagementClass.Reset_ENG();
               progress_2= 0;
               ProgressBar.Value = progress_2;
            }
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            settings["progress_1"] = Convert.ToString(progress_1);
            settings["progress_2"] = Convert.ToString(progress_2);
            ManagementClass.SaveSettings(progress_1,progress_2, new ReturnWordStruct { DictionaryTable=dictionaryTable,
            Settings=settings,Dictionary_eng_words=dictionary_eng_word});
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            DownloadData();
        }
    }
}
