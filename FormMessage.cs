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
        public MyMessage.Button[] buttons
        {
            set
            {
                flowLayoutPanelButtons.Controls.Clear();
                for(int i = 0; i < value.Length; i++)
                {
                    Button but = new Button() { Height = 20, Text = value[i].Caption };
                    EventHandler eh = new EventHandler(value[i].Click);
                    but.Click += eh;
                    flowLayoutPanelButtons.Controls.Add(but);
                }
            }
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
