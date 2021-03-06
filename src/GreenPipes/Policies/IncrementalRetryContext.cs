// Copyright 2007-2016 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace GreenPipes.Policies
{
    using System;
    using System.Threading.Tasks;
    using Util;


    public class IncrementalRetryContext<TContext> :
        BaseRetryContext,
        RetryContext<TContext>
        where TContext : class, PipeContext
    {
        readonly TimeSpan _delay;
        readonly TimeSpan _delayIncrement;
        readonly IncrementalRetryPolicy _policy;
        readonly int _retryCount;

        public IncrementalRetryContext(IncrementalRetryPolicy policy, TContext context, Exception exception, int retryCount, TimeSpan delay, TimeSpan delayIncrement)
            : base(context, typeof(TContext), retryCount)
        {
            _policy = policy;
            _retryCount = retryCount;
            _delay = delay;
            _delayIncrement = delayIncrement;
            Context = context;
            Exception = exception;
        }

        public TContext Context { get; }

        public Exception Exception { get; }

        public int RetryCount => _retryCount;

        public TimeSpan? Delay => _delay;

        public Task PreRetry()
        {
            return TaskUtil.Completed;
        }

        public Task RetryFaulted(Exception exception)
        {
            return TaskUtil.Completed;
        }

        public bool CanRetry(Exception exception, out RetryContext<TContext> retryContext)
        {
            retryContext = new IncrementalRetryContext<TContext>(_policy, Context, Exception, _retryCount + 1, _delay + _delayIncrement, _delayIncrement);

            return _retryCount < _policy.RetryLimit && _policy.Matches(exception);
        }
    }
}