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

        public static async Task<Spammer> LoadFromFile(string filename = "script.json")
        {
            string[] numbers = System.IO.File.ReadAllLines("nums.txt");
            string str = System.IO.File.ReadAllText(filename);
            List<PersonTask> ptasks = new List<PersonTask>();
            for(int i = 0; i < numbers.Count(); i++)
            {
                PersonTask ptask = await PersonTask.Create(numbers[i], str);
                ptasks.Add(ptask);
            }
            return new Spammer() { pertasks = ptasks };
        }
  
    
        public async Task Run()
        {
            for(int i = 0; i < pertasks.Count; i++)
            {
                await pertasks[i].Run();
            }
        }
        
    }
    
}
