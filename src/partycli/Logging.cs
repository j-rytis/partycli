using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using partycli.Domain;
using partycli.Domain.Models;

namespace partycli
{
    public interface ILogging
    {
        void Log(string messageToLog);
    }

    public class Logging : ILogging
    {
        private readonly IValueStorage _valueStorage;

        public Logging(IValueStorage valueStorage)
        {
            _valueStorage = valueStorage;
        }

        public void Log(string messageToLog)
        {
            var newLog = new LogModel
            {
                Action = messageToLog,
                Time = DateTime.Now
            };

            List<LogModel> currentLog;
            if (!string.IsNullOrEmpty(Properties.Settings.Default.log))
            {
                currentLog = JsonConvert.DeserializeObject<List<LogModel>>(Properties.Settings.Default.log);
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
