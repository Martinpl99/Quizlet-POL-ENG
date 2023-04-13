using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary_POL_ENG
{
    public static partial class ManagementClass
    {
        public static async Task<Dictionary<string, string>> DownloadDictionaresListENG_Basic()
        {
            Dictionary<string, string> eng_words = new Dictionary<string, string>();
            using (StreamReader read = new StreamReader(Address_2))
            {
                string json = await read.ReadToEndAsync().ConfigureAwait(false);
                read.Close();
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            }
        }
    }
}
