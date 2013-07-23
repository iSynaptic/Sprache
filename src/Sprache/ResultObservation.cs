using System;
using System.Collections.Generic;
using System.Linq;

namespace Sprache
{
    /// <summary>
    /// Represents an observation of the results of a parse attempt.
    /// </summary>
    public class ResultObservation
    {
        private readonly string _message;
        private readonly string[] _expectations;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">The observation's message</param>
        /// <param name="expectations">The parse expectations</param>
        /// <exception cref="ArgumentException"></exception>
        public ResultObservation(string message, IEnumerable<string> expectations)
        {
            if(string.IsNullOrWhiteSpace(message)) throw new ArgumentException("You must provide a message.", "message");

            _message = message;
            _expectations = expectations != null
                ? expectations.Where(x => x != null).ToArray()
                : new string[0];
        }

        /// <summary>
        /// The observation's message.
        /// </summary>
        public string Message { get { return _message; } }

        /// <summary>
        /// The parse expectations.
        /// </summary>
        public IEnumerable<string> Expectations { get { return _expectations; } }
    }
}