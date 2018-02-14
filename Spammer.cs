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
            string[] numbers = System.IO.File.ReadAllLines("nums.txt");
            string str = System.IO.File.ReadAllText(filename);
            List<PersonTask> ptasks = new List<PersonTask>();
            for(int i = 0; i < numbers.Count(); i++)
            {
                PersonTask ptask = await PersonTask.Create(numbers[i], str, log);
                ptasks.Add(ptask);
            }
            
            return new Spammer() { pertasks = ptasks, log = log };
        }
  
    
        public async Task Run()
        {
            for(int i = 0; i < pertasks.Count; i++)
            {
                log.Wrt("Task" + pertasks[i].number + "started");
                await pertasks[i].Run();
                log.Wrt("Task" + pertasks[i].number + "done");
            }
        }
        
    }
    
}
