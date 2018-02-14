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
        public Script(Manager mngr, JToken token, ref Log log)
        {
            this.log = log;
            this.mngr = mngr;
            Commands = new List<Command>();
            this.BotName = token["username"].ToString();
            log.Wrt("Working wis " + BotName);
            var cmdarr = token["script"].Children().ToList();

            for (int i = 0; i < cmdarr.Count;i++)
            {
                JProperty p = (JProperty)cmdarr[i].First();
                Command cmd = null;
                switch (p.Name)
                {
                    case "sendmsg":
                        try
                        {
                            cmd = new SendMsg(mngr, p.Value.ToString());
                        }
                        catch(Exception ex)
                        {
                            log.Wrt(ex.Message);
                        }
                        break;
                    case "callbackbtn":
                        try
                        {
                            var Row = p.Value["Row"].ToString();
                            var Btn = p.Value["Btn"].ToString();
                            cmd = new CallbackBtn(mngr, Convert.ToInt16(Row), Convert.ToInt16(Btn));
                        }
                        catch (Exception ex)
                        {
                            log.Wrt(ex.Message);
                        }
                        break;
                    default: break;
                }
                this.Commands.Add(cmd);
            }
        }
        
        public async Task Run()
        {
            bool res = true;
            while(res)
            for(int i = 0; i < Commands.Count && res; i++)
            {

                mngr.CurrentChatName = BotName;
                res = await Commands[i].Run();
                await Task.Delay(2000);
                log.Wrt(Commands[i].ToString() + " : " + res.ToString());
            }
            //await Task.Delay(10000);
        }

        public abstract partial class Command
        {
            protected Manager mngr;
            public Command(Manager mngr)
            {
                this.mngr = mngr;
            }
            public async virtual Task<bool> Run()
            {
                return true;
                //mngr.SendMsg(chatname, message);
            }
        }
        public class CallbackBtn : Command
        {
            int BtnNum, Row;
            public CallbackBtn(Manager mngr, int Row, int BtnNum) : base(mngr)
            {
                this.Row = Row;
                this.BtnNum = BtnNum;
            }
            public override async Task<bool> Run()
            {
                try
                {
                    MyMessage msg = await mngr.GetLastMessage();
                    MyMessage.Button button = msg.Buttons.Find((MyMessage.Button btn) => { return (btn.Position.Row == Row && btn.Position.Btn == BtnNum); });
                    if (button != null)
                    {
                        button.Click(null, null);
                    }
                    else
                        return false;
                }
                catch { return false; }
                return true;
            }
            public override string ToString()
            {
                return "[" + this.mngr.Number + " ] CallbackBtn {" + Row + ", " + BtnNum + "}";
            }
        }
        public class SendMsg : Command
        {
            string msg;
            public SendMsg(Manager mngr, string msg):base(mngr)
            {
                this.msg = msg;
            }
            public override async Task<bool> Run()
            {
                try
                {
                    await mngr.SendMsg(msg);
                }
                catch(Exception ex)
                {
                    if (ex.Message == "invalid checksum! skip")
                        try { await Task.Delay(5000); await mngr.SendMsg(msg); }catch { return false; }
                    else return false;
                }
                return true;
            }

            public override string ToString()
            {
                return "[" + this.mngr.Number + " ] SendMsg {" + this.msg + "}";
            }
        }
        
    }
}
