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

                JObject json = JObject.Parse(strJson);
                IList<JToken> results = json["arr"].Children().ToList();
                foreach (JToken result in results)
                {
                    bool res = true;
                    while (res == true)
                    {
                        log.Wrt("Signing in " + num);
                        Manager mngr = await Manager.Create(num);
                        log.Wrt("Authorization success : " + num);
                        ptask.number = num;
                        ptask.mngr = mngr;
                        try
                        {
                            Script script = new Script(mngr, result, ref log);
                            res = await script.Run();
                        }
                        catch(Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(ex.Message);
                            res = false;
                        }
                    }
                }
                System.Windows.Forms.MessageBox.Show(num + " task done");
                return ptask;
            }
            catch (Exception ex)
            {
                log.Wrt(" Error Persontask Create 0 " + ex.Message);
                return null;
            }
        }
    }
}
