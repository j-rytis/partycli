using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using partycli.Domain.Models;

namespace partycli.Domain
{
    public interface IListPrinter
    {
        void Display(string serverListAsString);
    }

    public class ListPrinter : IListPrinter
    {
        public void Display(string serverListAsString)
        {
            var serverlist = JsonConvert.DeserializeObject<List<ServerModel>>(serverListAsString);

            Console.WriteLine("Server list: ");
            
            for (var index = 0; index < serverlist.Count; index++)
            {
                Console.WriteLine("Name: " + serverlist[index].Name);
            }

            Console.WriteLine("Total servers: " + serverlist.Count);
        }
    }
}
