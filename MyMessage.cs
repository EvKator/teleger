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
        public TelegramClient client { get; set; }
        public TLMessage message { get; private set; }
        public TLUser user { get; set; }
        public MyMessage(TLMessage message, TelegramClient client, TLUser user)
        {
            this.client = client;
            this.user = user;
            this.message = message;
            this.Text = message.Message;
            Buttons = new List<Button>();
            var markup = ((TLReplyInlineMarkup)message.ReplyMarkup);
            if(markup!=null)
            for (int row = 0; row < markup.Rows.Count; row++)
            {
                for(int but = 0; but < markup.Rows[row].Buttons.Count; but++)
                {
                    Button button = Button.Create(this, markup.Rows[row].Buttons[but], new Button.BtnPosition { Row = row, Btn = but });
                    Buttons.Add(button);
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
            protected TLMessage message { get; private set; }
            //protected TLUser user;
            protected int ID { get; private set; }
            protected long hash { get; private set; }
            protected TelegramClient client { get; private set; }
            public struct BtnPosition
            {
                public int Row { get; set; }
                public int Btn { get; set; }
            }
            public Button(MyMessage mymessage, BtnPosition pos)
            {
                this.message = mymessage.message;
                this.client = mymessage.client;
                this.Position = pos;
                ID = ((TeleSharp.TL.TLUser)mymessage.user).Id;
                hash = ((TeleSharp.TL.TLUser)mymessage.user).AccessHash.Value;
            }
            public static Button Create(MyMessage mymessage,TLAbsKeyboardButton msgbtn, BtnPosition pos)
            {
                try
                {
                    return new DataButton(mymessage, (TLKeyboardButtonCallback)msgbtn, pos);
                }
                catch
                {
                    return new UrlButton(mymessage, (TLKeyboardButtonUrl)msgbtn, pos);
                }
            }
            public async virtual void Click(object s, object a) { }
        }

        public class DataButton:Button
        {
            public byte[] Data { get; private set; }
            
            public DataButton(MyMessage mymessage, TLKeyboardButtonCallback msgbtn, BtnPosition pos) :base(mymessage, pos)
            {
                type = ButtonType.Data;
                this.Data = msgbtn.Data;
                this.Caption = msgbtn.Text;
            }

            public async override void Click(object s, object a)
            {
                var req = new TLRequestGetBotCallbackAnswer()
                {
                    MsgId = message.Id,
                    Peer = new TLInputPeerUser { UserId = ID , AccessHash = hash },
                    Data = this.Data
                };
                try
                {
                    await client.SendRequestAsync<Boolean>(req);
                }
                catch
                {

                }
            }
        }

        public class UrlButton:Button
        {
            public string Url { get; private set; }
            public UrlButton(MyMessage mymessage, TLKeyboardButtonUrl msgbtn, BtnPosition pos) : base(mymessage, pos)
            {
                type = ButtonType.Data;
                this.Url = msgbtn.Url;
                this.Caption = this.Url;
            }

            public async override void Click(object s, object a)
            {
                try
                {
                    await JoinChannel(Url.Remove(0, 13));
                }
                catch { }
            }
            private async Task<TLChannel> FindChannel(string channelname)
            {
                var found = await client.SearchUserAsync(channelname);
                TLChannel tLChannel = (TLChannel)found.Chats[0];
                return tLChannel;
            }

            public async Task JoinChannel(string channelname)
            {
                TLChannel channel = await FindChannel(channelname);
                var req = new TeleSharp.TL.Channels.TLRequestJoinChannel()
                {
                    Channel = new TLInputChannel
                    {
                        ChannelId = channel.Id,
                        AccessHash = (long)channel.AccessHash
                    }
                };

                TLUpdates resJoinChannel = await client.SendRequestAsync<TLUpdates>(req);
            }
        }

    }
}
