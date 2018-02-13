using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Teleger
{
    public partial class PersonTask
    {
        public string number { get; set; }
        public List<Script> Scripts { get; set; }
        List<string> chats { get; set; }
        Manager mngr;
        
        private PersonTask()
        {
            
        }


        public static async Task<PersonTask> Create(string num, string strJson)
        {
            PersonTask ptask = new PersonTask();
            Manager mngr = await Manager.Create(num);
            var chats = mngr.GetAllChatCntacts();
            ptask.number = num;
            ptask.mngr = mngr;
            
            var Scripts = new List<Script>();
            JObject json = JObject.Parse(strJson);
            IList<JToken> results = json["arr"].Children().ToList();
            foreach (JToken result in results)
            {
                Script script = new Script(mngr, result);
                Scripts.Add(script);
            }
            
            ptask.Scripts = Scripts;
            ptask.chats = await chats;
            
            return ptask;
        }

        public async Task Run()
        {
            for(int i = 0; i < Scripts.Count; i++)
            {
                await Scripts[i].Run();
            }
        }
    }
}
