using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft;
using Newtonsoft.Json.Linq;

namespace Teleger
{
    class Telescript
    {
        public static Telescript LoadFromFile()
        {
            string str =
            "{" +
                "'arr':" +
                "[" +
                    "{" +
                        "'username':'EasyyMoneyyBot'," +
                        "'script':{" +
                            "'sendmsg': '/start'," +
                            "'sendmsg': '💲 Подписаться на канал'," +
                        "'callbackbtn': " +
                            "{" +
                                "'Row':'0'," +
                                "'Btn':'0'" +
                            "}," +
                        "'callbackbtn': " +
                            "{" +
                                "'Row':'1'," +
                                "'Btn':'0'" +
                            "}" +
                        "}" +
                    "}," +
                    "{" +
                    "'username':'EasyyMoneyyBot'," +
                    "'script':{" +
                        "'sendmsg': '/start'," +
                        "'sendmsg': '➕ Подписаться на канал'," +
                        "'callbackbtn': " +
                            "{" +
                                "'Row':'0'," +
                                "'Btn':'0'" +
                            "}," +
                        "'callbackbtn': " +
                            "{" +
                                "'Row':'1'," +
                                "'Btn':'0'" +
                            "}" +
                        "}" +
                    "}" +
                "]" +
            "}";



            JObject googleSearch = JObject.Parse(str);
            IList<JToken> results = googleSearch["arr"].Children().ToList();
            List<Telescript.PerScript> scripts = new List<Telescript.PerScript>();
            foreach(JToken result in results)
            {
                Telescript.PerScript searchResult = result.ToObject<Telescript.PerScript>();
                var dict = JsonConvert.DeserializeObject <Dictionary <string, string>>(result["script"].ToString());
                searchResult.Comands = dict;
                scripts.Add(searchResult);
            }


            return new Telescript();
        }
        class PerScript
        {
            public string Username { get; set; }
            public string CurrentComand { get; set; }
            public Dictionary<string, string> Comands { get; set; }

        }
    }
    
}
