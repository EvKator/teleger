using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teleger
{
    public class Log
    {
        System.Windows.Forms.RichTextBox rtb;
        public System.Windows.Forms.ProgressBar pbar { get; set; }
        public delegate void pbarinc(int i);
        public int ScriptsCount
        {
            get
            {
                if (pbar != null) return pbar.Maximum;
                else return 0;
            }
            set
            {
                if (pbar != null)
                {
                    if (pbar.InvokeRequired)
                    {
                        var mDel = new pbarinc((q) =>
                        {
                            pbar.Maximum = value;
                            pbar.Step = 1;
                        });
                        pbar.Invoke(mDel);
                    }
                    else
                    {
                        pbar.Maximum = value;
                        pbar.Step = 1;
                    }
                }
            }
        }
        public Log(ref System.Windows.Forms.RichTextBox rtb)
        {
            this.rtb = rtb;
        }

        public Log(ref System.Windows.Forms.RichTextBox rtb, ref System.Windows.Forms.ProgressBar pbar)
        {
            this.rtb = rtb;
            this.pbar = pbar;
        }
        public void Wrt(string logStr)
        {
            logStr = "\r\n" + DateTime.Now.ToLongTimeString() + " | " + logStr + "\r\n";
            rtb.AppendText(logStr);
        }

        public void ScriptPerformed()
        {
            if (pbar != null)
            {
                if (pbar.InvokeRequired)
                {

                    var mDel = new pbarinc((q) => pbar.Value++);
                    pbar.Invoke(mDel);
                }
                else
                {
                    pbar.Value++;
                }
            }
        }
    }
}
