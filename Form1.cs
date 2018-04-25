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
        public string NumsFileName { get; private set; }
        public string ScriptsFileName { get; private set; }
        

        private async void buttonLoadScript_Click(object sender, EventArgs e)
        {
            Log lg = new Log(ref richTextBoxLog);
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "JSON files|*.json";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Spammer spammer = await Spammer.LoadFromFile(ofd.FileName, this.NumsFileName, lg);
                //Panel panel1 = new Panel();
                //panel1.Name = "p1";

                for(int i = 0; i < spammer.pertasks.Count; i++)
                {
                    ProgressBar pbar = new ProgressBar();
                    pbar.Name = "pbar" + i.ToString();

                    GroupBox gbox = new GroupBox();
                    gbox.Text = spammer.pertasks[i].Number;
                    gbox.Name = "gbox" + i.ToString();
                    gbox.Dock = DockStyle.Top;
                    gbox.Controls.Add(pbar);

                    panel1.Controls.Add(gbox);

                    gbox.Top = 50 * i;
                    gbox.Height = 50;
                    gbox.Left = 0;
                    pbar.Dock = System.Windows.Forms.DockStyle.Fill;
                    spammer.pertasks[i].log = new Log(ref richTextBoxLog, ref pbar) { ScriptsCount = spammer.pertasks[i].Scripts.Count };
                }
                await spammer.Start();
            }
        }

        private void buttonLoadNums_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "TXT files|*.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                NumsFileName = ofd.FileName;
                buttonLoadScript.Enabled = true;
            }
        }
    }
}
