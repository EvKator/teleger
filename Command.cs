using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teleger
{
    public abstract class Command
    {
        protected Manager mngr;
        public Command(Manager mngr)
        {
            this.mngr = mngr;
        }
        public async virtual Task<bool> Run(int attemps = 2)
        {
            int attemp = 0;
            bool done = false;
            for (; attemp < attemps && !done; attemp++)
            {
                await mngr.Reconnect();////////////////////////////////////////May be error
                done = await this.Perform();
            }
            return done;
        }

        protected async virtual Task<bool> Perform()
        {
            return true;
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

        protected async override Task<bool> Perform()
        {
            MyMessage msg = await mngr.GetLastMessage();
            MyMessage.Button button = msg.Buttons.Find((MyMessage.Button btn) => { return (btn.Position.Row == Row && btn.Position.Btn == BtnNum); });
            if (button != null)
            {
                return await button.Click(null, null);
            }
            else
                return false; ///////////////////////////////BUTTON NOT FOUNT ERROR
        }

        public override string ToString()
        {
            return "[" + this.mngr.Number + " ] CallbackBtn {" + Row + ", " + BtnNum + "}";
        }
    }


    public class SendMsg : Command
    {
        string msg;
        public SendMsg(Manager mngr, string msg) : base(mngr)
        {
            this.msg = msg;
        }

        protected async override Task<bool> Perform()
        {
            return await mngr.SendMsg(msg);
        }

        public override string ToString()
        {
            return "[" + this.mngr.Number + " ] SendMsg {" + this.msg + "}";
        }
    }
}
