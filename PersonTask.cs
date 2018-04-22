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
        public List<Script> Scripts { get; set; }
        List<string> chats { get; set; }
        Manager mngr;
        Log log;

        
        private PersonTask()
        {
            
        }


        public static async Task<PersonTask> Create(string num, string strJson, Log log)
        {
            try
            {
                PersonTask ptask = new PersonTask();
                ptask.log = log;
                ptask.Scripts = new List<Script>();
                JObject json = JObject.Parse(strJson);
                IList<JToken> jsonScripts = json["arr"].Children().ToList();
                foreach (JToken jsonScript in jsonScripts)
                {
                    Script script = await Script.Create(num, jsonScript, log);
                    ptask.Scripts.Add(script);
                }
                System.Windows.Forms.MessageBox.Show(num + " task done");
                return ptask;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return null;
            }
        }

        public async Task Start()
        {
            foreach (var script in this.Scripts)
            {
                bool res = true;
                while (res == true)
                {
                    try
                    {
                        res = await script.Run(5000);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                        res = false;
                    }
                }
            }
        }
    }
}
