using System;
using System.Collections.Generic;
using System.Linq;

namespace Sprache
{
    /// <summary>
    /// Represents the details of an error after a parse attempt.
    /// </summary>
    public class ResultError
    {
        private readonly Position _position;
        private readonly string _message;
        private readonly string[] _expectations;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position">The position of the error</param>
        /// <param name="message">The error message</param>
        /// <param name="expectations">The parse expectations</param>
        public ResultError(Position position, string message, IEnumerable<string> expectations)
        {
            if(position == null) throw new ArgumentNullException("position");

            _position = position;
            _message = message ?? "";
            _expectations = expectations != null
                ? expectations.Where(x => x != null).ToArray()
                : new string[0];
        }

        /// <summary>
        /// The position of the error
        /// </summary>
        public Position Position { get { return _position; } }

        /// <summary>
        /// The error message
        /// </summary>
        public string Message { get { return _message; } }

        /// <summary>
        /// The parse expectations
        /// </summary>
        public IEnumerable<string> Expectations { get { return _expectations; } }

    }
}