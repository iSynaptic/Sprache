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
        /// Creates a success result.
        /// </summary>
        /// <typeparam name="T">The type of the result (value).</typeparam>
        /// <param name="value">The sucessfully parsed value.</param>
        /// <param name="remainder">The remainder of the input.</param>
        /// <param name="observations">The resulting observations.</param>
        /// <returns>The new <see cref="IResult&lt;T&gt;"/>.</returns>
        public static IResult<T> Success<T>(T value, Input remainder, params ResultObservation[] observations)
        {
            return Success(value, remainder, (IEnumerable<ResultObservation>) observations);
        }

        /// <summary>
        /// Creates a success result.
        /// </summary>
        /// <typeparam name="T">The type of the result (value).</typeparam>
        /// <param name="value">The sucessfully parsed value.</param>
        /// <param name="remainder">The remainder of the input.</param>
        /// <param name="observations">The resulting observations.</param>
        /// <returns>The new <see cref="IResult&lt;T&gt;"/>.</returns>
        public static IResult<T> Success<T>(T value, Input remainder, IEnumerable<ResultObservation> observations)
        {
            return new Result<T>(value, remainder, observations);
        }

        /// <summary>
        /// Creates a failure result.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="remainder">The remainder of the input.</param>
        /// <param name="observations">The resulting observations.</param>
        /// <returns>The new <see cref="IResult&lt;T&gt;"/>.</returns>
        public static IResult<T> Failure<T>(Input remainder, params ResultObservation[] observations)
        {
            return Failure<T>(remainder, (IEnumerable<ResultObservation>)observations);
        }

        /// <summary>
        /// Creates a failure result.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="remainder">The remainder of the input.</param>
        /// <param name="observations">The resulting observations.</param>
        /// <returns>The new <see cref="IResult&lt;T&gt;"/>.</returns>
        public static IResult<T> Failure<T>(Input remainder,  IEnumerable<ResultObservation> observations)
        {
            return new Result<T>(remainder, observations);
        }

        /// <summary>
        /// Creates a ResultObservation with a severity of Error.
        /// </summary>
        /// <param name="message">The observation's message</param>
        /// <param name="expectations">The parse expectations</param>
        /// <returns>The observation</returns>
        public static ResultObservation Error(string message, params string[] expectations) { return Error(message, (IEnumerable<string>)expectations); }

        /// <summary>
        /// Creates a ResultObservation with a severity of Error.
        /// </summary>
        /// <param name="message">The observation's message</param>
        /// <param name="expectations">The parse expectations</param>
        /// <returns>The observation</returns>
        public static ResultObservation Error(string message, IEnumerable<string> expectations) { return new ResultObservation(message, expectations); }

    }

    internal class Result<T> : IResult<T>
    {
        private readonly bool _hasValue;
        private readonly T _value;

        private readonly Input _remainder;
        private readonly ResultObservation[] _observations;

        public Result(T value, Input remainder, IEnumerable<ResultObservation> observations)
        {
            _hasValue = true;
            _value = value;
            _remainder = remainder;
            _observations = observations.ToArray();
        }

        public Result(Input remainder, IEnumerable<ResultObservation> observations)
        {
            _hasValue = false;
            _value = default(T);

            _remainder = remainder;
            _observations = observations.ToArray();
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

        public IEnumerable<ResultObservation> Observations { get { return _observations; }}

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

            string observations = string.Join(Environment.NewLine, Observations
                .Select(x => x.Message));

            return String.Format("{0} Observations: {2}{1}", intro , observations, Environment.NewLine);
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
