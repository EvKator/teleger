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
            try
            {
                Commands = new List<Command>();
                mngr.CurrentChatName = this.BotName = token["username"].ToString();
                log.Wrt("Working wis " + BotName);
                var cmdarr = token["script"].Children().ToList();

                for (int i = 0; i < cmdarr.Count; i++)
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
                            catch (Exception ex)
                            {
                                log.Wrt(mngr.Number + "Script Constructor 0 error" + ex.Message);
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
                                log.Wrt(mngr.Number + "Script Constructor 1 error" + ex.Message);
                            }
                            break;
                        default: break;
                    }
                    Commands.Add(cmd);
                }
            }catch(Exception ex)
            {
                log.Wrt(mngr.Number + "Script Constructor 2 error" + ex.Message);
            }
        }
        
        public async Task<bool> Run()
        {
            try
            {
                bool res = true;
                for (int i = 0; i < Commands.Count && res; i++)
                {
                    res = await Commands[i].Run();
                    await Task.Delay(15000);
                    log.Wrt(Commands[i].ToString() + " : " + res.ToString());
                }
                return res;
            }
            catch (Exception ex)
            {
                //log.Wrt(mngr.Number + "Script Run 0 error" + ex.Message);
                return false;
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
                bool done = false;
                int attemp = 0;
                for (; attemp < 3 && !done; attemp++)
                {
                    try
                    {
                        MyMessage msg = await mngr.GetLastMessage();
                        MyMessage.Button button = msg.Buttons.Find((MyMessage.Button btn) => { return (btn.Position.Row == Row && btn.Position.Btn == BtnNum); });
                        if (button != null)
                        {
                            button.Click(null, null);
                            done = true;
                        }
                        else
                            return false;
                    }
                    catch (Exception ex)
                    {
                            await mngr.Reconnect();
                    }
                }
                //if (attemp == 3) System.Windows.Forms.MessageBox.Show(mngr.Number + " AUTH_KEY_UNREGISTERED");
                return done;
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
                bool done = false;
                int attemp = 0;
                for(; attemp < 3 && !done; attemp++)
                {
                    try
                    {
                        await mngr.SendMsg(msg);
                        done = true;
                    }
                    catch (Exception ex)
                    {
                        await mngr.Reconnect();
                    }
                }
                //if (attemp == 3) System.Windows.Forms.MessageBox.Show(mngr.Number + " AUTH_KEY_UNREGISTERED");
                return done;
            }

            public override string ToString()
            {
                return "[" + this.mngr.Number + " ] SendMsg {" + this.msg + "}";
            }
        }
        
    }
}
