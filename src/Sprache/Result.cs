using System;
using System.Collections.Generic;
using System.Linq;

namespace Sprache
{
    /// <summary>
    /// Contains helper functions to create <see cref="IResult&lt;T&gt;"/> instances.
    /// </summary>
    public static class Result
    {
        /// <summary>
        /// Creates a result with a value.
        /// </summary>
        /// <typeparam name="T">The type of the result (value).</typeparam>
        /// <param name="value">The sucessfully parsed value.</param>
        /// <param name="remainder">The remainder of the input.</param>
        /// <param name="errors">The resulting errors.</param>
        /// <returns>The new <see cref="IResult&lt;T&gt;"/>.</returns>
        public static IResult<T> Value<T>(T value, Input remainder, params ResultError[] errors)
        {
            return Value(value, remainder, (IEnumerable<ResultError>) errors);
        }

        /// <summary>
        /// Creates a result with a value.
        /// </summary>
        /// <typeparam name="T">The type of the result (value).</typeparam>
        /// <param name="value">The sucessfully parsed value.</param>
        /// <param name="remainder">The remainder of the input.</param>
        /// <param name="observations">The resulting errors.</param>
        /// <returns>The new <see cref="IResult&lt;T&gt;"/>.</returns>
        public static IResult<T> Value<T>(T value, Input remainder, IEnumerable<ResultError> observations)
        {
            return new Result<T>(value, remainder, observations);
        }

        /// <summary>
        /// Creates a result without a value.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="remainder">The remainder of the input.</param>
        /// <param name="errors">The resulting errors.</param>
        /// <returns>The new <see cref="IResult&lt;T&gt;"/>.</returns>
        public static IResult<T> NoValue<T>(Input remainder, params ResultError[] errors)
        {
            return NoValue<T>(remainder, (IEnumerable<ResultError>)errors);
        }

        /// <summary>
        /// Creates a result without a value.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="remainder">The remainder of the input.</param>
        /// <param name="observations">The resulting errors.</param>
        /// <returns>The new <see cref="IResult&lt;T&gt;"/>.</returns>
        public static IResult<T> NoValue<T>(Input remainder,  IEnumerable<ResultError> observations)
        {
            return new Result<T>(remainder, observations);
        }

        /// <summary>
        /// Creates a ResultError
        /// </summary>
        /// <param name="message">The observation's message</param>
        /// <param name="position"></param>
        /// <param name="expectations">The parse expectations</param>
        /// <returns>The observation</returns>
        public static ResultError Error(string message, Position position, params string[] expectations) { return Error(message, position, (IEnumerable<string>)expectations); }

        /// <summary>
        /// Creates a ResultError
        /// </summary>
        /// <param name="message">The observation's message</param>
        /// <param name="position"></param>
        /// <param name="expectations">The parse expectations</param>
        /// <returns>The observation</returns>
        public static ResultError Error(string message, Position position, IEnumerable<string> expectations) { return new ResultError(position, message, expectations); }

    }

    internal class Result<T> : IResult<T>
    {
        private readonly bool _hasValue;
        private readonly T _value;

        private readonly Input _remainder;
        private readonly ResultError[] _errors;

        public Result(T value, Input remainder, IEnumerable<ResultError> errors)
        {
            _hasValue = true;
            _value = value;
            _remainder = remainder;
            _errors = errors.ToArray();
        }

        public Result(Input remainder, IEnumerable<ResultError> errors)
        {
            _hasValue = false;
            _value = default(T);

            _remainder = remainder;
            _errors = errors.ToArray();
        }

        public T Value
        {
            get
            {
                if (!HasValue)
                    throw new InvalidOperationException("No value can be computed.");

                return _value;
            }
        }

        public bool HasValue { get { return _hasValue; } }

        public IEnumerable<ResultError> Errors { get { return _errors; }}

        public Input Remainder { get { return _remainder; } }

        public override string ToString()
        {
            string intro;

            if (!HasValue)
            {
                var recentlyConsumed = CalculateRecentlyConsumed();
                intro = string.Format("Parsing failure. Recently consumed: '{0}'.", recentlyConsumed);
            }
            else
            {
                intro = string.Format("Successfuly parsed value: {0}.", Value);
            }

            string errors = string.Join(Environment.NewLine, Errors
                .Select(x => x.Message));

            return String.Format("{0} Errors: {2}{1}", intro, errors, Environment.NewLine);
        }

        private string CalculateRecentlyConsumed()
        {
            const int windowSize = 10;

            var totalConsumedChars = Remainder.Position;
            var windowStart = totalConsumedChars - windowSize;
            windowStart = windowStart < 0 ? 0 : windowStart;

            var numberOfRecentlyConsumedChars = totalConsumedChars - windowStart;

            return Remainder.Source.Substring(windowStart, numberOfRecentlyConsumedChars);
        }
    }
}
