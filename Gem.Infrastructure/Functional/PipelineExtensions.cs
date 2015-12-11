using System;

namespace Gem.Infrastructure.Functional
{
    public static class PipelineExtensions
    {
        #region Pending Result

        public static Pipeline<TValue> Catch<TValue>(this Pipeline<TValue> pendingResult, Func<Exception, Result<TValue>> exceptionHandler)
        {
            pendingResult.ExceptionHandler = exceptionHandler;
            return pendingResult;
        }

        public static Pipeline Catch(this Pipeline pendingResult, Func<Exception, Result> exceptionHandler)
        {
            pendingResult.ExceptionHandler = exceptionHandler;
            return pendingResult;
        }

        #endregion

        #region Try Success


        public static Pipeline<TValue> ContinueWith<TValue>(
            this Pipeline<TValue> future,
            Func<TValue, Result<TValue>> func)
        {
            return new Pipeline<TValue>(() =>
            {
                var res = future.PendingResult();
                return
                (res.Success)
                ? func(res.Value)
                : res;
            });
        }

        public static Pipeline<TNext> ContinueWithNew<TValue, TNext>(
            this Pipeline<TValue> future,
            Func<TValue, Result<TNext>> func)
        {
            return new Pipeline<TNext>(() =>
            {
                var res = future.PendingResult();
                return
                (res.Success)
                ? func(res.Value)
                : Result.Fail<TNext>(res.Error);
            });
        }

        public static Pipeline ContinueWith(
            this Pipeline future,
            Action action)
        {
            return new Pipeline(() =>
            {
                var res = future.PendingResult();
                if (res.Success)
                {
                    action();
                    return Result.Ok();
                }
                return res;
            });
        }


        #endregion
    }
}
