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
    public partial class FormMessage : Form
    {
        public void addButton(MyMessage.Button messageBtn)
        {
            Button formBtn = new Button() { Height = 20, Text = messageBtn.Caption };
            EventHandler eh = new EventHandler(async (e,s) =>await messageBtn.Click(e,s));
            formBtn.Click += eh;
            flowLayoutPanelButtons.Controls.Add(formBtn);
        }
        public string Message
        {
            set
            {
                richTextBox1.Text = value;
            }
        }
        public FormMessage()
        {
            InitializeComponent();
        }
    }
}
