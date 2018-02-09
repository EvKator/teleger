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
            mngr = new Manager(textBoxNumber.Text);
            bool res = await mngr.Connect(textBoxNumber.Text);
            List<string> chats = await mngr.GetAllChatCntacts();
            FillContactList(chats);
            groupBoxAuthorize.Text = "Authorized";
            groupBoxContacts.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mngr.SendMsg(comboBoxContacts.SelectedItem.ToString() , textBoxMsgToSend.Text);
        }

        private async void buttonGetMsg_Click(object sender, EventArgs e)
        {
            /*string messages = await */mngr.GetLastMessages(comboBoxContacts.SelectedItem.ToString(), Convert.ToInt16(textBoxGetMsgsCount.Text));
            /*richTextBoxLog.Text = messages + "\r\n=================================r\n=================================\r\n" + richTextBoxLog.Text;
            richTextBoxLog.AppendText(messages);*/
        }

        private void FillContactList(List<string> contacts)
        {
            //for(int i = 0; i < contacts.Count; i++)
            //{
                comboBoxContacts.Items.AddRange(contacts.ToArray());
            //}
        }

        private void comboBoxContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(comboBoxContacts.SelectedItem.ToString()))
            {
                groupBoxMsgs.Enabled = true;
            }
            else
            {
                groupBoxMsgs.Enabled = false;
            }
        }
    }
}
