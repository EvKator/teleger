using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft;
using Newtonsoft.Json.Linq;

namespace Teleger
{
    class Spammer
    {
        public List<PersonTask> pertasks { get; private set; }
        Log log;
        public static async Task<Spammer> LoadFromFile(string filename, string numsfilename, Log log)
        {
            try
            {
                string[] numbers = System.IO.File.ReadAllLines(numsfilename);
                string str = System.IO.File.ReadAllText(filename);
                List<PersonTask> ptasks = new List<PersonTask>();
                for (int i = 0; i < numbers.Count(); i++)
                {
                    log.Wrt("Creating task for " + numbers[i]);
                    PersonTask ptask = await PersonTask.Create(numbers[i], str, log);
                    ptasks.Add(ptask);
                }
                return new Spammer() { pertasks = ptasks, log = log };
            }catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Spammer LoadFromFile 0 error" + ex.Message);
                return null;
            }
        }

        public async Task Start(int delay = 10000)
        {
            log.Wrt("-------------------");
            log.Wrt("Tasks performing: \n");
            foreach (var pertask in pertasks)
            {
                pertask.Start();
                await Task.Delay(delay);
            }
        }
        
    }
    
}
