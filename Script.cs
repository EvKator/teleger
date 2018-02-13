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
        List<Command> Commands { get; set; }
        string BotName { get; set; }
        Manager mngr {get;set; }
        public Script(Manager mngr, JToken token)
        {
            this.mngr = mngr;
            Commands = new List<Command>();
            this.BotName = token["username"].ToString();
            mngr.CurrentChatName = BotName;
            var cmdarr = token["script"].Children().ToList();
            for (int i = 0; i < cmdarr.Count;i++)
            {
                JProperty p = (JProperty)cmdarr[i].First();
                Command cmd = null;
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
                this.Commands.Add(cmd);
            }
        }
        
        public async Task Run()
        {
            bool res = true;
            for(int i = 0; i < Commands.Count && !res; i++)
            {
                res = await Commands[i].Run();
                await Task.Delay(2000);
            }
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
                System.Windows.Forms.MessageBox.Show("CallbackBtn");
                MyMessage msg = await mngr.GetLastMessage();
                MyMessage.Button button = msg.Buttons.Find((MyMessage.Button btn) => { return (btn.Position.Row == Row && btn.Position.Btn == BtnNum); });
                if (button != null)
                {
                    button.Click(null, null);
                }
                else
                    return false;
                return true;
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
                    System.Windows.Forms.MessageBox.Show("SendMsg");
                    await mngr.SendMsg(msg);
                }
                catch
                {
                    return false;
                }
                return true;
            }
        }
    }
}
