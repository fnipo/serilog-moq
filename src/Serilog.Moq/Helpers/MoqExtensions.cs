using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Serilog.Moq.Helpers
{
    internal static class MoqExtensions
    {
        internal static void VerifyAny<T>(this Mock<T> mock, params Expression<Action<Mock<T>>>[] expressions)
            where T : class
        {
            var exceptions = new List<MockException>();
            bool success = false;
            foreach (var expression in expressions)
            {
                try
                {
                    expression.Compile().Invoke(mock);
                    success = true;
                    break;
                }
                catch (MockException ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (!success)
            {
                throw new AggregateException(exceptions);
            }
        }
    }
}
