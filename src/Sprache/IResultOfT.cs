using System.Collections.Generic;

namespace Sprache
{
    /// <summary>
    /// Represents a parsing result.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    public interface IResult<out T>
    {
        /// <summary>
        /// Gets the resulting value.
        /// </summary>
        T Value { get; }

        /// <summary>
        /// Gets a value indicating whether parsing was able to return a value.
        /// </summary>
        bool HasValue { get; }

        /// <summary>
        /// Gets any errors as a result of parsing.
        /// </summary>
        IEnumerable<ResultError> Errors { get; }
            
        /// <summary>
        /// Gets the remainder of the input.
        /// </summary>
        Input Remainder { get; }
    }
}
