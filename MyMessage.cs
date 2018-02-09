using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleSharp.TL;

namespace Teleger
{
    class MyMessage
    {
        public string Text { get; private set; }
        public List<Button> Buttons { get; private set; }
        public MyMessage(TLMessage message)
        {
            this.Text = message.Message;
            var markup = ((TLReplyInlineMarkup)message.ReplyMarkup);
            for (int row = 0; row < markup.Rows.Count; row++)
            {
                for(int but = 0; but < markup.Rows[row].Buttons.Count; but++)
                {
                    Button button = Button.Create( markup.Rows[row].Buttons[but] , new Button.BtnPosition { Row = row, Btn = but });
                    Buttons.Add(button);
                }
            }
        }
        public abstract class Button
        {
            public enum ButtonType { Data, Url}
            public string Caption { get; protected set; }
            public BtnPosition Position { get; protected set; }
            public struct BtnPosition
            {
                public int Row { get; set; }
                public int Btn { get; set; }
            }
            protected ButtonType type { get; set; }
            public Button(BtnPosition pos)
            {
                this.Position = pos;
            }
            public static Button Create(TLAbsKeyboardButton msgbtn, BtnPosition pos)
            {
                try
                {
                    return new DataButton((TLKeyboardButtonCallback)msgbtn, pos);
                }
                catch
                {
                    return new UrlButton((TLKeyboardButtonUrl)msgbtn, pos);
                }
            }
        }

        public class DataButton:Button
        {
            public byte[] Data { get; private set; }
            
            public DataButton(TLKeyboardButtonCallback msgbtn, BtnPosition pos) :base(pos)
            {
                type = ButtonType.Data;
                this.Data = msgbtn.Data;
                this.Caption = msgbtn.Text;
            }
        }

        public class UrlButton:Button
        {
            public string Url { get; private set; }
            public UrlButton(TLKeyboardButtonUrl msgbtn, BtnPosition pos) : base(pos)
            {

                type = ButtonType.Data;
                this.Url = msgbtn.Url;
                this.Caption = this.Url;
            }
        }

    }
}
