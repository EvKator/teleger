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
    class Manager
    {
        const int apiId = 191412;
        TelegramClient client = null;
        const string apiHash = "68ed96b9aa9842eb2ded4023c3e32e6e";
        public bool Authorized { get; private set; }
        FileSessionStore store;
        public Manager(string number)
        {
            //Connect(number);
        }
        public async Task<bool> Connect(string number)
        {
            store = (new FileSessionStore());// "session.dat"
            try
            {
                client = new TelegramClient(apiId, apiHash, store, "session");
                await client.ConnectAsync();
                if (!client.IsUserAuthorized() || !client.IsConnected)
                    throw new Exception("Need sms code");
                MessageBox.Show("Authorization success\n" + number);
            }
            catch
            {
                string hashNumber = await client.SendCodeRequestAsync(number);
                for(int attemp = 0; attemp < 3; attemp++)
                {
                    FormTeleCode ftc = new FormTeleCode();
                    ftc.Question = "Enter the sms code";
                    if (ftc.ShowDialog() == DialogResult.OK)
                    {
                        string code = ftc.Code;
                        try
                        {
                            Confirm(number, hashNumber, code);
                            Authorized = true;
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else break;
                    return false;
                }
            }

            return true;
        }

        private async void Confirm(string number, string hashNumber, string code)
        {
            var user = await client.MakeAuthAsync(number, hashNumber, code);
            System.Windows.Forms.MessageBox.Show("Authorisation succes\n" + user.FirstName + " " + user.LastName);
        }

        public async void SendMsg(string username, string text = "QWERTY0")
        {
            var a = await client.SearchUserAsync(username);  // .GetContactsAsync(); //.ImportByUserName("userName");
            try
            {
                long hash = ((TeleSharp.TL.TLUser)a.Users[0]).AccessHash.Value;
                int id = ((TeleSharp.TL.TLUser)a.Users[0]).Id;
                TeleSharp.TL.TLInputPeerUser peer = new TeleSharp.TL.TLInputPeerUser() { UserId = id, AccessHash = hash };

                TeleSharp.TL.TLAbsUpdates up = await this.client.SendMessageAsync(peer, "jkhjkhjkhktart");
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //var b = a.
            //(TLAbsPeer)a.Results[0]
            //var c = (await client.GetUserDialogsAsync());

            //var user = c.
            ////.Where(x => x.GetType() == typeof(TLUser))
            //.Cast<TLUser>()
            //.FirstOrDefault(x => x.Username == username);
            ////IEnumerable<string> contact = c.Users.Where(usr => usr)
            ////await client.SendMessageAsync(new TLInputPeerUser { UserId = ((TLUser)a.Users[0]).Id}, text);
            //await client.SendMessageAsync(/*new TLInputPeerUser { UserId = user.Id }*/(TLAbsInputPeer)a.Results[0], text);
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

        public async void GetLastMessages(string username, int num = 1)
        {
            var a = await client.SearchUserAsync(username);
            int ID = ((TeleSharp.TL.TLUser)a.Users[0]).Id;
            long hash = ((TeleSharp.TL.TLUser)a.Users[0]).AccessHash.Value;

            var messages = (await client.GetHistoryAsync(new TLInputPeerUser { UserId = ID, AccessHash = hash }, 0, 0, num));

            string str = "";
            var qwe = ((TLMessagesSlice)messages).Messages;

            hash = ((TeleSharp.TL.TLUser)a.Users[0]).AccessHash.Value;
            //TLSharp.Core.Requests.
            foreach (TLMessage message in qwe)
            {
                var tlMessage = message as TLMessage;
                str += tlMessage.Message + "\n\n------------\n";
                var req = new TLRequestGetBotCallbackAnswer()
                {
                    MsgId = message.Id,
                    Peer = new TLInputPeerUser { UserId = ID, AccessHash = hash },
                    //Response = new TLBotCallbackAnswer { Message = "Перейти к каналу"  }
                        Data = ((TeleSharp.TL.TLKeyboardButtonCallback)(((TeleSharp.TL.TLReplyInlineMarkup)message.ReplyMarkup).Rows[1]).Buttons[0]).Data
                    };

                var res = await client.SendRequestAsync<Boolean>(req);
                ;
                ;
                ;
                //return await TeleSharp.TL.

            }



            //run request, and deserialize response to Boolean
            //return await SendRequestAsync<Boolean>(req);


            //return str;
        }
    }
}
