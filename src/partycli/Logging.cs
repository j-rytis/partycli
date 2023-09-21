using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using partycli.Domain;
using partycli.Domain.Models;

namespace partycli
{
    public class Logging : ILogging
    {
        private readonly IValueStorage _valueStorage;

        private readonly IConfigurationReader _configurationReader;


        public Logging(IValueStorage valueStorage, IConfigurationReader configurationReader)
        {
            _valueStorage = valueStorage;
            _configurationReader = configurationReader;
        }

        public void Log(string messageToLog)
        {
            var newLog = new LogModel
            {
                Action = messageToLog,
                Time = DateTime.Now
            };

            var currentLog = _configurationReader.GetLog();

            if (currentLog.Any())
            {
                currentLog.Add(newLog);
            }
            else
            {
                currentLog = new List<LogModel> { newLog };
            }

            _valueStorage.Store("log", JsonConvert.SerializeObject(currentLog), false);
        }
    }
}
