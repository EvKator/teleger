using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Teleger
{
    public class Script
    {
        Log log;
        List<Command> Commands { get; set; }
        string BotName { get; set; }
        Manager mngr {get;set; }

        public static async Task<Script> Create(string num, JToken token, Log log)
        {
            Script script = new Script();
            Manager mngr = await Manager.Create(num);
            log.Wrt("Authorization success : " + num);

            script.log = log;
            script.mngr = mngr;
            script.Commands = new List<Command>();

            try
            {
                mngr.CurrentChatName = script.BotName = token["username"].ToString();
            }
            catch
            {
                throw new Exception("username parsing error");
            }

            log.Wrt("Script processing for " + script.BotName);

            List<Newtonsoft.Json.Linq.JToken> cmdarr;

            try
            {
                cmdarr = token["script"].Children().ToList();
            }
            catch
            {
                throw new Exception("script parsing error");
            }

            for (int i = 0; i < cmdarr.Count; i++)
            {
                JProperty p = (JProperty)cmdarr[i].First();
                Command cmd = null;
                try
                {
                    switch (p.Name)
                    {
                        case "sendmsg":
                            cmd = new SendMsg(mngr, p.Value.ToString());
                            break;
                        case "callbackbtn":
                            var Row = p.Value["Row"].ToString();
                            var Btn = p.Value["Btn"].ToString();
                            cmd = new CallbackBtn(mngr, Convert.ToInt16(Row), Convert.ToInt16(Btn));
                            break;
                        default: break;
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(p.Name + " command creation error");
                }
                script.Commands.Add(cmd);
            }
            log.Wrt("OK");
            return script;
        }
        private Script()
        {

            
        }
        
        public async Task<bool> Run(int Delay = 15000)
        {
            try
            {
                bool res = true;
                for (int i = 0; i < Commands.Count && res; i++)
                {
                    res = await Commands[i].Run();
                    await Task.Delay(Delay);
                    log.Wrt(Commands[i].ToString() + " : " + res.ToString());
                }
                return res;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return false;
            }
        }

        
        
    }
}
