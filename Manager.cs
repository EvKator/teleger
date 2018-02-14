using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLSharp.Core;
using Teleger;
using TeleSharp;
using TeleSharp.TL.Messages;
using TeleSharp.TL;
using Teleger.Properties;
using TLSharp.Core.MTProto;
using TLSharp.Core.Requests;
using System.Windows.Forms;
using Microsoft.VisualBasic;

using TeleSharp.TL.Account;
using TeleSharp.TL.Auth;
using TeleSharp.TL.Contacts;
using TeleSharp.TL.Help;
using TeleSharp.TL.Upload;
using TLSharp.Core.Auth;
using TLSharp.Core.MTProto.Crypto;
using TLSharp.Core.Network;
using TLSharp.Core.Utils;
using Teleger;
using TeleSharp.TL.Bots;
using TeleSharp.TL.Updates;
using Teleger.Properties;
using TLAuthorization = TeleSharp.TL.Auth.TLAuthorization;
namespace Teleger
{
    public class Manager
    {
        const int apiId = 191412;
        TelegramClient client = null;
        const string apiHash = "68ed96b9aa9842eb2ded4023c3e32e6e";
        public bool Authorized { get; set; }
        public string CurrentChatName { get; set; }
        public string Number { get; set; }
        FileSessionStore store;
        private Manager()
        {
        }
        public static async Task<Manager> Create(string number)
        {
            Manager mngr = new Manager();
            mngr.CurrentChatName = "";
            mngr.Authorized = false;

            mngr.store = (new FileSessionStore());// "session.dat"
            try
            {
                mngr.client = new TelegramClient(apiId, apiHash, mngr.store, number.ToString());
                await mngr.client.ConnectAsync();
                if (!mngr.client.IsUserAuthorized() || !mngr.client.IsConnected)
                    throw new Exception("Need sms code");
                mngr.Authorized = true;
                MessageBox.Show("Authorization success\n" + number);
            }
            catch
            {
                string hashNumber = await mngr.client.SendCodeRequestAsync(number);
                for(int attemp = 0; attemp < 3 && !mngr.Authorized; attemp++)
                {
                    FormTeleCode ftc = new FormTeleCode();
                    ftc.Question = "Enter the sms code" ;
                    ftc.Text = number;
                    if (ftc.ShowDialog() == DialogResult.OK)
                    {
                        string code = ftc.Code;
                        try
                        {
                            mngr.Confirm(number, hashNumber, code);
                            mngr.Authorized = true;
                            
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else break;
                }
            }

            return mngr;
        }

        private async void Confirm(string number, string hashNumber, string code)
        {
            var user = await client.MakeAuthAsync(number, hashNumber, code);
            System.Windows.Forms.MessageBox.Show("Authorisation succes\n" + user.FirstName + " " + user.LastName);
        }

        public async Task SendMsg(string text)
        {
            var a = await client.SearchUserAsync(CurrentChatName);
            try
            {
                long hash = ((TeleSharp.TL.TLUser)a.Users[0]).AccessHash.Value;
                int id = ((TeleSharp.TL.TLUser)a.Users[0]).Id;
                TeleSharp.TL.TLInputPeerUser peer = new TeleSharp.TL.TLInputPeerUser() { UserId = id, AccessHash = hash };
                TeleSharp.TL.TLAbsUpdates up = await this.client.SendMessageAsync(peer, text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task<List<string>> GetAllChatCntacts()
        {
            List<string> list = new List<string>();


            var dialogs = await client.GetUserDialogsAsync(); // .GetContactsAsync(); //.ImportByUserName("userName");
            try
            {
                foreach(var user in ((TLDialogs)dialogs).Users)
                {
                    string uname = ((TLUser)user).Username;
                    if (!string.IsNullOrWhiteSpace(uname) && uname != "null")
                        list.Add(uname);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            return list;
        }

        private async Task<TLChannel> FindChannel(string channelname)
        {
            var found = await client.SearchUserAsync(channelname);
            TLChannel tLChannel = (TLChannel)found.Chats[0];
            return tLChannel;
        }

        public async void JoinChannel(string channelname)
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

        public async void MessageBtnClick(TLMessage message, TLUser user, int Row, int Btn = 0)
        {
            int ID = ((TeleSharp.TL.TLUser)user).Id;
            long hash = ((TeleSharp.TL.TLUser)user).AccessHash.Value;

            if (((((TLReplyInlineMarkup)message.ReplyMarkup).Rows[1]).Buttons[0]).GetType() == typeof(TeleSharp.TL.TLKeyboardButtonUrl)) {
                var req = new TLRequestGetBotCallbackAnswer()
                {
                    MsgId = message.Id,
                    Peer = new TLInputPeerUser { UserId = ID, AccessHash = hash },
                    Data = ((TeleSharp.TL.TLKeyboardButtonCallback)(((TeleSharp.TL.TLReplyInlineMarkup)message.ReplyMarkup).Rows[Row]).Buttons[Btn]).Data
                };
                await client.SendRequestAsync<Boolean>(req);
            }
            else
            {
                string Url = ((TeleSharp.TL.TLKeyboardButtonUrl)(((TeleSharp.TL.TLReplyInlineMarkup)message.ReplyMarkup).Rows[Row]).Buttons[Btn]).Url;
            }
            

        }

        public async void GetLastMessages(int num)
        {
            var a = await client.SearchUserAsync(this.CurrentChatName);
            int ID = ((TeleSharp.TL.TLUser)a.Users[0]).Id;
            long hash = ((TeleSharp.TL.TLUser)a.Users[0]).AccessHash.Value;

            var messages = (await client.GetHistoryAsync(new TLInputPeerUser { UserId = ID, AccessHash = hash }, 0, 0, num));

            string str = "";
            var qwe = ((TLMessagesSlice)messages).Messages;

            hash = ((TeleSharp.TL.TLUser)a.Users[0]).AccessHash.Value;
            foreach (TLMessage message in qwe)
            {
                str += message.Message + "\n\n------------\n";
            }
        }

        public async Task<MyMessage> GetLastMessage()
        {
            return await GetMessage(0);
        }

        private async Task<TLUser> FindBot(string botname)
        {
            return (TLUser)(await client.SearchUserAsync(botname)).Users[0];
        }

        public async Task<MyMessage> GetMessage(int num = 0)
        {
            TLUser bot = await FindBot(this.CurrentChatName);
            int ID = ((TeleSharp.TL.TLUser)bot).Id;
            long hash = ((TeleSharp.TL.TLUser)bot).AccessHash.Value;

            var messages = (await client.GetHistoryAsync(new TLInputPeerUser { UserId = ID, AccessHash = hash }, num, 0, 1));
            var message = (TLMessage)((TLMessagesSlice)messages).Messages[0];
            MyMessage mymessage = new MyMessage(message,client ,bot );
            return mymessage;
        }
    }


}
