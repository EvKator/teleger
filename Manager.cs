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
        private Manager(){}

        public static async Task<Manager> Create(string number)
        {
            Manager mngr = new Manager();
            mngr.CurrentChatName = "";
            mngr.Authorized = false;
            mngr.Number = number;
            mngr.store = (new FileSessionStore());// "session.dat"
            try
            {
                mngr.client = new TelegramClient(apiId, apiHash, mngr.store, number.ToString());
                await mngr.client.ConnectAsync(false);
                if (!mngr.client.IsUserAuthorized() || !mngr.client.IsConnected)
                {
                    string hashNumber = await mngr.client.SendCodeRequestAsync(number);
                    for (int attemp = 0; attemp < 3 && !mngr.Authorized; attemp++)
                    {
                        FormTeleCode ftc = new FormTeleCode();
                        ftc.Question = "Enter the sms code";
                        ftc.Text = number;
                        if (ftc.ShowDialog() == DialogResult.OK)
                        {
                            string code = ftc.Code;
                            try
                            {
                                var user = await mngr.client.MakeAuthAsync(mngr.Number, hashNumber, code);
                                mngr.Authorized = true;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(number + "Manager Create 0 error" + ex.Message);
                            }
                        }
                        else break;
                    }
                }
                mngr.Authorized = true;
                //MessageBox.Show("Authorization success\n" + number);
            }
            catch(Exception ex)
            {
                MessageBox.Show(number + "Manager Create 1 error " + ex.Message);
            }

            return mngr;
        }

        public bool RemoveSession()
        {
            try
            {
                System.IO.File.Delete(Number.ToString() + ".dat");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Reconnect()
        {
            Authorized = false;
            store = null;
            
            try
            {
                store = (new FileSessionStore());// "session.dat"
                this.client = new TelegramClient(apiId, apiHash, store, Number.ToString());
                await client.ConnectAsync(false);
                if (!client.IsUserAuthorized() || !client.IsConnected)
                {
                    string hashNumber = await client.SendCodeRequestAsync(Number);
                    for (int attemp = 0; attemp < 3 && !Authorized; attemp++)
                    {
                        FormTeleCode ftc = new FormTeleCode();
                        ftc.Question = "Enter the sms code";
                        ftc.Text = this.Number;
                        if (ftc.ShowDialog() == DialogResult.OK)
                        {
                            string code = ftc.Code;
                            try
                            {
                                var user = await client.MakeAuthAsync(Number, hashNumber, code);
                                Authorized = true;

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(Number + "Manager Reconnect 0 error" + ex.Message);
                            }
                        }
                        else break;
                    }
                }
                Authorized = true;
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(Number + "Manager Reconnect 1 error" + ex.Message);
                return false;
            }
        }

        public async Task<bool> SendMsg(string text)
        {
            try
            {
                var a = await FindBot(CurrentChatName);
                long hash = ((TeleSharp.TL.TLUser)a).AccessHash.Value;
                int id = ((TeleSharp.TL.TLUser)a).Id;
                TeleSharp.TL.TLInputPeerUser peer = new TeleSharp.TL.TLInputPeerUser() { UserId = id, AccessHash = hash };
                await this.client.SendMessageAsync(peer, text);
                return true;
            }
            catch(Exception ex)
            {
                if(ex.Message == "AUTH_KEY_UNREGISTERED")
                {
                    bool sessionRemoved = this.RemoveSession();
                    if (sessionRemoved)
                        await this.Reconnect();
                }
                return false;
            }
        }

        public async Task<List<string>> GetAllChatCntacts()
        {
            List<string> list = new List<string>();


            var dialogs = (await client.GetUserDialogsAsync());
            try
            {
                foreach (var user in ((TeleSharp.TL.Messages.TLDialogsSlice)dialogs).Users)
                {
                    string uname = ((TLUser)user).Username;
                    if (!string.IsNullOrWhiteSpace(uname) && uname != "null")
                        list.Add(uname);
                }
            }
            catch (Exception ex)
            {
                foreach (var user in ((TeleSharp.TL.Messages.TLDialogs)dialogs).Users)
                {
                    string uname = ((TLUser)user).Username;
                    if (!string.IsNullOrWhiteSpace(uname) && uname != "null")
                        list.Add(uname);
                }
            }


            return list;
        }

        private async Task<TLChannel> FindChannel(string channelname)
        {
            var found = await client.SearchUserAsync(channelname);
            TLChannel tLChannel = (TLChannel)found.Chats.Where(x => ((TLChannel)x).Username == channelname).FirstOrDefault();
            return tLChannel;
        }

        public async Task<bool> JoinChannel(string channelname)
        {
            try
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
                return true;
            }
            catch(Exception ex)
            {
                if (ex.Message == "AUTH_KEY_UNREGISTERED")
                {
                    bool sessionRemoved = this.RemoveSession();
                    if (sessionRemoved)
                        await this.Reconnect();
                }
                return false;
            }
        }

        public async Task<bool> MessageBtnClick(TLMessage message, int Row, int Btn)
        {
            try
            {
                var user = await FindBot(CurrentChatName);//////////////////////////////////////////ReadMessageErrorHandle
                await ReadMessage(user, message);
                int ID = ((TeleSharp.TL.TLUser)user).Id;
                long hash = ((TeleSharp.TL.TLUser)user).AccessHash.Value;

                var req = new TLRequestGetBotCallbackAnswer()
                {
                    MsgId = message.Id,
                    Peer = new TLInputPeerUser { UserId = ID, AccessHash = hash },
                    Data = ((TeleSharp.TL.TLKeyboardButtonCallback)(((TeleSharp.TL.TLReplyInlineMarkup)message.ReplyMarkup).Rows[Row]).Buttons[Btn]).Data,
                };
                try
                {
                    await client.SendRequestAsync<Boolean>(req);
                }
                catch (Exception ex)
                {
                    if (ex.Message == "AUTH_KEY_UNREGISTERED")
                    {
                        bool sessionRemoved = this.RemoveSession();
                        if (sessionRemoved)
                            await this.Reconnect();
                        return false;
                    }
                    //Ignore exception.. I did not want it, but library is too bad documented and written (for one night?)
                }
                return true;
            }
            catch(Exception ex)
            {
                if (ex.Message == "AUTH_KEY_UNREGISTERED")
                {
                    bool sessionRemoved = this.RemoveSession();
                    if (sessionRemoved)
                        await this.Reconnect();
                }
                return false;
            }
        }

        public async Task ReadMessage(TLUser user, TLMessage message)
        {
            try
            {
                int ID = ((TeleSharp.TL.TLUser)user).Id;
                long hash = ((TeleSharp.TL.TLUser)user).AccessHash.Value;
                var req = new TLRequestReadHistory()
                {
                    ConfirmReceived = true,
                    MessageId = message.Id,
                    Peer = new TLInputPeerUser { UserId = ID, AccessHash = hash },
                    Sequence = 2
                };
                await client.SendRequestAsync<Boolean>(req);
            }catch(Exception ex)
            {
                //////////////////////////////////////Causes stuped undocumented exceptions, but works norm
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
            MyMessage mymessage = null;

            var messages = (await client.GetHistoryAsync(new TLInputPeerUser { UserId = ID, AccessHash = hash }, num, 0, 1));
            try
            {
                var message = (TLMessage)((TLMessagesSlice)messages).Messages[0];
                mymessage = new MyMessage(this, message);
            }
            catch
            {
                var message = (TLMessage)(((TLMessages)messages).Messages[0]);
                mymessage = new MyMessage(this, message);
            }
            return mymessage;
        }
    }


}
