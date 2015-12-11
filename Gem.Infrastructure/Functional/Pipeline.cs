using System;

namespace Gem.Infrastructure.Functional
{
    public class Pipeline<TValue>
    {
        public Func<Result<TValue>> PendingResult { get; }

        public Result<TValue> Result => this;

        public Func<Exception, Result<TValue>> ExceptionHandler { get; set; }

        public Pipeline(Func<Result<TValue>> result)
        {
            PendingResult = result; 
        }

        public static implicit operator Result<TValue>(Pipeline<TValue> pending)
        {
            try
            {
                return pending.PendingResult();
            }
            catch (Exception ex)
            {
                return
                (pending.ExceptionHandler != null) 
                ? pending.ExceptionHandler(ex)
                : Functional.Result.Fail(default(TValue),"Exception in pending result");
            }
        }
    }

    public class Pipeline
    {
        public Func<Result> PendingResult { get; }

        public Result Result => this;
        
        public Func<Exception, Result> ExceptionHandler { get; set; }

        public Pipeline(Func<Result> result)
        {
            PendingResult = result;
        }

        public static Pipeline<TValue> For<TValue>(Func<Result<TValue>> futureFunc)
        {
            return new Pipeline<TValue>(futureFunc);
        }

        public static Pipeline For(Func<Result> futureAction)
        {
            return new Pipeline(futureAction);
        }

        public static implicit operator Result(Pipeline pending)
        {
            try
            {
                return pending.PendingResult();
            }
            catch (Exception ex)
            {
                return
                (pending.ExceptionHandler != null)
                ? pending.ExceptionHandler(ex)
                : Functional.Result.Fail("Exception in pending result");
            }
        }
    }
}

