﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharePointPnP.Modernization.Framework.Telemetry.Observers
{
    public class ConsoleObserver : ILogObserver
    {
        private static readonly Lazy<List<Tuple<LogLevel, LogEntry>>> _lazyLogInstance = new Lazy<List<Tuple<LogLevel, LogEntry>>>(() => new List<Tuple<LogLevel, LogEntry>>());
        private bool _includeDebugEntries;

        /// <summary>
        /// Get the single List<LogEntry> instance, singleton pattern
        /// </summary>
        public static List<Tuple<LogLevel, LogEntry>> Logs
        {
            get
            {
                return _lazyLogInstance.Value;
            }
        }

        /// <summary>
        /// Console Observer constructor
        /// </summary>
        public ConsoleObserver(bool includeDebugEntries = false)
        {
            _includeDebugEntries = includeDebugEntries;
        }

        /// <summary>
        /// Output on any warnings generated by the transform process
        /// </summary>
        /// <param name="entry"></param>
        public void Debug(LogEntry entry)
        {
            if (_includeDebugEntries)
            {
                Write($"Debug: [{entry.Heading}] {entry.Message} \n\t Source: {entry.Source} ");
            }
        }

        /// <summary>
        /// Errors 
        /// </summary>
        /// <param name="entry"></param>
        public void Error(LogEntry entry)
        {
            var error = entry.Exception != null ? entry.Exception.Message : "No error logged";
            Write($"Error: [{entry.Heading}] {entry.Message} Error: { error }");
        }

        public void Flush()
        {
            //Output transform duration
            Console.WriteLine("-----------Transformation Summary -------------");

            Logs.ForEach(o =>
            {
                Write($"-  {o.Item2.Message}");
            });

            Console.WriteLine("-----------------------------------------------");
        }

        /// <summary>
        /// Output on operations throughout the transform process
        /// </summary>
        /// <param name="entry"></param>
        public void Info(LogEntry entry)
        {
            // Log summary data for final output
            if(entry.Heading == LogStrings.Heading_Summary)
            {
                Logs.Add(new Tuple<LogLevel, LogEntry>(LogLevel.Information, entry));
            }
            else
            {
                Write($"Info: [{entry.Heading}] {entry.Message}");
            }
        }

        /// <summary>
        /// Output on any warnings generated by the transform process
        /// </summary>
        /// <param name="entry"></param>
        public void Warning(LogEntry entry)
        {
            
            Write($"Warning: [{entry.Heading}] {entry.Message}");
        }

        /// <summary>
        /// Cental method to output to console
        /// </summary>
        /// <param name="message"></param>
        public void Write(string message)
        {
            message = message.Replace(LogStrings.KeyValueSeperatorToken, "=");
            Console.WriteLine(message);
        }
    }
}
