using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dictionary_POL_ENG
{
     public static partial class ManagementClass
    {



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





        public static async Task<ReturnWordStruct> DownloadData()
        {
            Dictionary<string, Dictionary<string, string>> dictionaryTable = new Dictionary<string, Dictionary<string, string>>();
            List<string> dictionaries_list = new List<string>();
            Dictionary<string, List<string>> dictionary_keys_list = new Dictionary<string, List<string>>();
            Dictionary<string, string> eng_words = new Dictionary<string, string>();

            using (StreamReader read = new StreamReader(Address_6))
            {
                string json = await read.ReadToEndAsync();
                read.Close();
                var dicitonary = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
                foreach (var x in dicitonary)
                {
                    dictionaryTable.Add(x.Key, x.Value);
                    if (dictionaryTable[x.Key].Count == 0)
                    {
                        continue;
                    }
                    else
                    {
                        dictionaries_list.Add(x.Key);
                        dictionary_keys_list.Add(x.Key, x.Value.Keys.ToList());
                    }
                }
            }


            using (StreamReader read = new StreamReader(Address_2))
            {
                string json = await read.ReadToEndAsync();
                read.Close();
                eng_words=JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }

            var ReturnValue = new ReturnWordStruct
            {
                Dictionaries_list = dictionaries_list,
                DictionaryTable = dictionaryTable,
                Dictionary_keys_list = dictionary_keys_list,
                Eng_words = eng_words
            };
            return ReturnValue;  
        }

        public static async Task<Dictionary<string, string>> DownloadSettings()
        {
            using (StreamReader read = new StreamReader(Address_4))
            {
                string json = await read.ReadToEndAsync();
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                read.Close();
            }
           
        }


        public static async Task<ReturnWordStruct> Reset_PL()
        {

            Dictionary<string, Dictionary<string, string>> dictionaryTable = new Dictionary<string, Dictionary<string, string>>();
            List<string> dictionaries_list = new List<string>();
            Dictionary<string, List<string>> dictionary_keys_list = new Dictionary<string, List<string>>();



                using (StreamReader reader = new StreamReader(Address_5))
                {
                    string json = await reader.ReadToEndAsync();
                    reader.Close();
                    var dicitonary = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);

                    foreach (var x in dicitonary)
                    {
                        dictionaryTable.Add(x.Key, x.Value);
                        if (dictionaryTable[x.Key].Count == 0)
                        {
                            continue;
                        }
                        else
                        {
                            dictionaries_list.Add(x.Key);
                            dictionary_keys_list.Add(x.Key, x.Value.Keys.ToList());
                        }
                    }


                    return new ReturnWordStruct
                    {
                        Dictionaries_list = dictionaries_list,
                        DictionaryTable = dictionaryTable,
                        Dictionary_keys_list = dictionary_keys_list
                    };
                }
        }



        public static async Task<Dictionary<string, string>> Reset_ENG()
        {
            await Task.Delay(10000);
            using(StreamReader reader=new StreamReader(Address_3))
            {
                string json = await reader.ReadToEndAsync();
                reader.Close();
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
        }


        public async static void SaveSettings(int progress_1,int progress_2, ReturnWordStruct wordStruct)
        {

            using(StreamWriter writer= new StreamWriter(Address_4))
            {
                string json = JsonConvert.SerializeObject(wordStruct.Settings);
                writer.Write(json);
                writer.Close();
            }

            using (StreamWriter writer = new StreamWriter(Address_6))
            {
                string json = JsonConvert.SerializeObject(wordStruct.DictionaryTable);
                writer.Write(json);
                writer.Close();
            }

            using(StreamWriter writer= new StreamWriter(Address_2))
            {
                string json = JsonConvert.SerializeObject(wordStruct.Dictionary_eng_words);
                writer.Write(json);
                writer.Close();
            }
        }


        public async static void Return_POL_ENG()
        {
            int count;
            List<string> list = new List<string>();
            StringBuilder builder = new StringBuilder();
            Dictionary<string, Dictionary<string, string>> dic_pl_table = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> dic_pl_words = new Dictionary<string, string>();

            using (StreamReader reader = new StreamReader(Address_5))
            {
                string json = await reader.ReadToEndAsync();
                reader.Close();
                if (json == "")
                {
                    dic_pl_table.Add("mean1", new Dictionary<string, string>());
                }
                else
                {
                    dic_pl_table = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
                }
                list.AddRange(dic_pl_table.Keys.ToList());
            }


            //Load json settings 
            using (StreamReader reader = new StreamReader(Address_4))
            {
                string json = await reader.ReadToEndAsync();
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                count = Convert.ToInt32(dictionary["count"]);
                reader.Close();

                /*Code to convert word places, words which are keys will become as value, the problem was that, one polish
                word can be descibe using many english word, so I created dictionary which contain other dictionaries,
                so when one dictionary contain word, other word is adding to next dictionary which is creating*/

                Dictionary<string, string> dic = DownloadDictionaresListENG_Basic().Result;
                foreach (var x in dic)
                {
                    bool flag = false;
                    bool flag_2 = false;
                    int number = 0;

                    for (int i = 0; i < list.Count; i++)
                    {
                        if (dic_pl_table[list[i]].ContainsKey(x.Value))
                        {
                            flag = true;
                            number++;
                        }
                        else
                        {
                            flag_2 = true;
                            continue;
                        }


                    }

                    if (number == count)
                    {
                        count++;

                        builder.Append("mean");
                        builder.Append(Convert.ToString(count));
                        list.Add(builder.ToString());
                        var dic_2 = new Dictionary<string, string>();
                        dic_2.Add(x.Value, x.Key);
                        dic_pl_table.Add(builder.ToString(), dic_2);
                        builder.Clear();
                    }
                    else
                    {
                        dic_pl_table[list[number]].Add(x.Value, x.Key);
                    }

                }

                using (StreamWriter writer = new StreamWriter(Address_4))
                {
                    dictionary["count"] = Convert.ToString(count);
                    string json_2 = JsonConvert.SerializeObject(dictionary);
                    writer.WriteLine(json_2);
                    writer.Close();
                }

                using (StreamWriter writer = new StreamWriter(Address_5))
                {
                    string json_3 = JsonConvert.SerializeObject(dic_pl_table);
                    writer.WriteLine(json_3);
                    writer.Close();
                }
            }
        }

    }






}
