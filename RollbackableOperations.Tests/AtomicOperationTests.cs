using System;
using NUnit.Framework;

namespace RollbackableOperations.Tests
{
    [TestFixture]
    public class AtomicOperationTests
    {
        [Test]
        public void AtomicOperationShouldThrowExceptionOnExecuteIfNoExecutionHandlerSpecified()
        {
            var operation = new AtomicOperation();
            var result = operation.Execute();
            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.Message, Is.Empty);
        }

        [Test]
        public void AtomicOperationShouldReturnSuccessResultFromRollbackIfNoRollbackHandlerSpecified()
        {
            var operation = new AtomicOperation();
            var result = operation.Rollback();
            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.Message, Is.Empty);
        }

        [Test]
        public void
            AtomicOperationShouldHandleUnhandledExceptionsInHandlersAndConsiderItAFailedOperationWithExceptionMessage()
        {
            var operation = new AtomicOperation()
            {
                ExecutionHandler = () => { throw new Exception("execution");},
                RollbackHandler = () => { throw new Exception("rollback");}
            };

            var executionResult = operation.Execute();
            Assert.That(executionResult.Succeeded, Is.False);
            Assert.That(executionResult.Message, Is.EqualTo("execution"));

            var rollbackResult = operation.Rollback();
            Assert.That(rollbackResult.Succeeded, Is.False);
            Assert.That(rollbackResult.Message, Is.EqualTo("rollback"));
        }
    }
}
