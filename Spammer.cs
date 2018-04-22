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
        List<PersonTask> pertasks { get; set; }
        Log log;
        public static async Task<Spammer> LoadFromFile(string filename, Log log)
        {
            try
            {
                string[] numbers = System.IO.File.ReadAllLines("nums.txt");
                string str = System.IO.File.ReadAllText(filename);
                List<PersonTask> ptasks = new List<PersonTask>();
                for (int i = 0; i < numbers.Count(); i++)
                {
                    PersonTask ptask = await PersonTask.Create(numbers[i], str, log);
                    ptasks.Add(ptask);
                }
                return new Spammer() { pertasks = ptasks, log = log };
            }catch(Exception ex)
            {
                log.Wrt("Spammer LoadFromFile 0 error" + ex.Message);
                return null;
            }
        }

        public async Task Start(int delay = 10000)
        {
            foreach(var pertask in pertasks)
            {
                pertask.Start();
                await Task.Delay(delay);
            }
        }
        
    }
    
}
