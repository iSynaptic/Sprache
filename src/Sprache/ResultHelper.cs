using System;

namespace Sprache
{
    internal static class ResultHelper
    {
        public static IResult<U> IfSuccess<T, U>(this IResult<T> result, Func<IResult<T>, IResult<U>> next)
        {
            if(result == null) throw new ArgumentNullException("result");

            if (result.HasValue)
                return next(result);

            return Result.Failure<U>(result.Remainder, result.Observations);
        }

        public static IResult<T> IfFailure<T>(this IResult<T> result, Func<IResult<T>, IResult<T>> next)
        {
            if (result == null) throw new ArgumentNullException("result");

            return result.HasValue 
                ? result 
                : next(result);
        }
    }
}