using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleSharp.TL;
using TLSharp.Core;
using Teleger;
using TeleSharp;
using TeleSharp.TL.Messages;

namespace Teleger
{
    public class MyMessage
    {
        public string Text { get; private set; }
        public List<Button> Buttons { get; private set; }
        public TLMessage message { get; private set; }
        public MyMessage(Manager mngr, TLMessage message)
        {
            this.message = message;
            this.Text = message.Message;
            Buttons = new List<Button>();
            TLReplyInlineMarkup markup = null;
            try
            {
                markup = ((TLReplyInlineMarkup)message.ReplyMarkup);
            }
            catch { }
            if (markup != null)
            {
                for (int row = 0; row < markup.Rows.Count; row++)
                {
                    for (int but = 0; but < markup.Rows[row].Buttons.Count; but++)
                    {
                        Button button = Button.Create(mngr, message, markup.Rows[row].Buttons[but], new Button.BtnPosition { Row = row, Btn = but });
                        Buttons.Add(button);
                    }
                }
            }
        }

        public void Show()
        {
            FormMessage fmsg = new FormMessage();
            fmsg.Message = this.Text;
            fmsg.buttons = this.Buttons.ToArray();
            fmsg.ShowDialog();
        }
        public abstract class Button
        {
            public enum ButtonType { Data, Url}
            public string Caption { get; protected set; }
            public BtnPosition Position { get; protected set; }
            protected ButtonType type { get; set; }
            protected Manager mngr;
            protected TLMessage message { get; private set; }
            public struct BtnPosition
            {
                public int Row { get; set; }
                public int Btn { get; set; }
            }
            public Button(Manager mngr, TLMessage message, BtnPosition pos)
            {
                this.mngr = mngr;
                this.message = message;
                this.Position = pos;
            }
            public static Button Create(Manager mngr, TLMessage message, TLAbsKeyboardButton msgbtn, BtnPosition pos)
            {
                try
                {
                    return new DataButton(mngr, message, (TLKeyboardButtonCallback)msgbtn, pos);
                }
                catch
                {
                    return new UrlButton(mngr, message, (TLKeyboardButtonUrl)msgbtn, pos);
                }
            }
            public async virtual void Click(object s, object a) { }
        }

        public class DataButton:Button
        {
            public byte[] Data { get; private set; }
            
            public DataButton(Manager mngr, TLMessage message, TLKeyboardButtonCallback msgbtn, BtnPosition pos) :base(mngr, message, pos)
            {
                type = ButtonType.Data;
                this.Caption = msgbtn.Text;
            }

            public async override void Click(object s, object a)
            {
                await mngr.MessageBtnClick(message, Position.Row, Position.Btn);
            }
        }

        public class UrlButton:Button
        {
            public string Url { get; private set; }
            public UrlButton(Manager mngr, TLMessage message, TLKeyboardButtonUrl msgbtn, BtnPosition pos) : base(mngr, message, pos)
            {
                type = ButtonType.Data;
                this.Url = msgbtn.Url;
                this.Caption = this.Url;
            }

            public async override void Click(object s, object a)
            {
                try
                {
                    await mngr.JoinChannel(Url.Remove(0, 13));
                }
                catch (Exception ex) {  }
            }
            
        }

    }
}
