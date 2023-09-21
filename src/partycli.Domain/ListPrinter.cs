using System;
using System.Collections.Generic;
using partycli.Domain.Models;

namespace partycli.Domain
{
    public interface IListPrinter
    {
        void Display(List<ServerModel> serverlist);
    }

    public class ListPrinter : IListPrinter
    {
        private readonly IConsoleWriter _consoleWriter;

        public ListPrinter(IConsoleWriter consoleWriter)
        {
            _consoleWriter = consoleWriter;
        }

        public void Display(List<ServerModel> serverlist)
        {
            _consoleWriter.Output("Server list: ");
            
            for (var index = 0; index < serverlist.Count; index++)
            {
                _consoleWriter.Output("Name: " + serverlist[index].Name);
            }

            _consoleWriter.Output("Total servers: " + serverlist.Count);
        }
    }
}
