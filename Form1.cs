using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teleger
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Manager mngr;
        private async void buttonConnect_Click(object sender, EventArgs e)
        {
            mngr = await Manager.Create(textBoxNumber.Text);
            List<string> chats = await mngr.GetAllChatCntacts();
            FillContactList(chats);
            groupBoxAuthorize.Text = "Authorized";
            groupBoxContacts.Enabled = true;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await mngr.SendMsg( textBoxMsgToSend.Text);
        }

        private async void buttonGetMsg_Click(object sender, EventArgs e)
        {
            MyMessage msg = await mngr.GetMessage(Convert.ToInt16(textBoxGetMsgsCount.Text));
            msg.Show();
        }

        private void FillContactList(List<string> contacts)
        {
            comboBoxContacts.Items.AddRange(contacts.ToArray());
        }

        private void comboBoxContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(comboBoxContacts.SelectedItem.ToString()))
            {
                mngr.CurrentChatName = comboBoxContacts.SelectedItem.ToString();
                groupBoxMsgs.Enabled = true;
            }
            else
            {
                groupBoxMsgs.Enabled = false;
            }
        }

        private async void buttonLoadScript_Click(object sender, EventArgs e)
        {
            Log lg = new Log(ref richTextBoxLog);
            Spammer spammer = await Spammer.LoadFromFile("script.json", lg);
        }
    }
}
