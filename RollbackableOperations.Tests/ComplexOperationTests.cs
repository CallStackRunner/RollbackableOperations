using System.Collections.Generic;
using NUnit.Framework;

namespace RollbackableOperations.Tests
{
    [TestFixture]
    public class ComplexOperationTests
    {
        [Test]
        public void ComplexOperationShouldExecuteOperationsInRightOrder()
        {
            var items = new List<int>();
            var operation = new ComplexOperation();

            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(4, items, OperationResult.Success, OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(8, items, OperationResult.Success, OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(15, items, OperationResult.Success, OperationResult.Success));
            operation.Execute();

            Assert.That(items, Is.EqualTo(new[] {4, 8, 15}));
        }

        [Test]
        public void ComplexOperationShouldRollbackAllOperationsInReverseOrderInCaseOfSomeNonFirstOperationFailure()
        {
            var items = new List<int>();
            var operation = new ComplexOperation();

            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(4, items, OperationResult.Success, OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(8, items, OperationResult.Success, OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(15, items, OperationResult.Success, OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(16, items, OperationResult.Fail("fail"), OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(23, items, OperationResult.Success, OperationResult.Success));
            operation.Execute();

            Assert.That(items, Is.EqualTo(new[] { -4, -8, -15, 16}));
        }

        [Test]
        public void ComplexOperationShouldRollbackFailedOperationItselfIfConfigured()
        {
            var items = new List<int>();
            var operation = new ComplexOperation();

            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(4, items, OperationResult.Success, OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(8, items, OperationResult.Success, OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(15, items, OperationResult.Success, OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(16, items, OperationResult.Fail("fail"), OperationResult.Success),
                new OperationExecutionConfiguration() {RollbackOperationItselftOnFail = true});
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(23, items, OperationResult.Success, OperationResult.Success));
            operation.Execute();

            Assert.That(items, Is.EqualTo(new[] { -4, -8, -15, -16 }));
        }

        [Test]
        public void ComplexOperationShouldReturnOperationResultCorrespondingToFailedOperation()
        {
            var items = new List<int>();
            var operation = new ComplexOperation();

            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(4, items, OperationResult.Success, OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(8, items, OperationResult.Success, OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(15, items, OperationResult.Success, OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(16, items, OperationResult.Fail("fail"), OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(23, items, OperationResult.Success, OperationResult.Success));
            var executionResult = operation.Execute();

            Assert.That(executionResult.Succeeded, Is.False);
            Assert.That(executionResult.Message, Is.EqualTo("fail"));
        }

        [Test]
        public void ComplexOperationShouldHandleFailedOperationRollbackAfterExecution()
        {
            var items = new List<int>();
            var executionFailMessage = "execution fail";
            var rollbackFailMessage = "rollback fail";
            var operation = new ComplexOperation();

            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(4, items, OperationResult.Success, OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(8, items, OperationResult.Success, OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(15, items, OperationResult.Success, OperationResult.Fail(rollbackFailMessage)));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(16, items, OperationResult.Fail(executionFailMessage), OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(23, items, OperationResult.Success, OperationResult.Success));
            var result = operation.Execute();

            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Message, Is.EqualTo($"Execution fail cause: {executionFailMessage}. Rollback fail cause: {rollbackFailMessage}"));
        }

        [Test]
        public void ComplexOperationShouldRollbackOperationsInReverseOrder()
        {
            var items = new List<int>();
            var operation = new ComplexOperation();

            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(4, items, OperationResult.Success, OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(8, items, OperationResult.Success, OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(15, items, OperationResult.Success, OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(16, items, OperationResult.Success, OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(23, items, OperationResult.Success, OperationResult.Success));

            operation.Execute();
            Assert.That(items, Is.EqualTo(new[] {4, 8, 15, 16, 23}));

            operation.Rollback();
            Assert.That(items, Is.EqualTo(new[] { -4, -8, -15, -16, -23 }));
        }

        [Test]
        public void ComplexOperationShouldHandleRollbackOperationFailure()
        {
            var items = new List<int>();
            var operation = new ComplexOperation();

            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(4, items, OperationResult.Success, OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(8, items, OperationResult.Success, OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(15, items, OperationResult.Success, OperationResult.Fail("rollback failure")));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(16, items, OperationResult.Success, OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(23, items, OperationResult.Success, OperationResult.Success));

            operation.Execute();
            Assert.That(items, Is.EqualTo(new[] { 4, 8, 15, 16, 23 }));

            var rollbackResult = operation.Rollback();
            Assert.That(rollbackResult.Succeeded, Is.False);
            Assert.That(rollbackResult.Message, Is.EqualTo("rollback failure"));
            Assert.That(items, Is.EqualTo(new[] { 4, 8, -15, -16, -23 }));
        }

        [Test]
        public void ComplexOperationShouldNotRollbackChangesOnFailureIfCorrespondingConfigurationSpecified()
        {
            var items = new List<int>();
            var operation = new ComplexOperation();

            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(4, items, OperationResult.Success, OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(8, items, OperationResult.Success, OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(15, items, OperationResult.Success, OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(16, items, OperationResult.Fail(), OperationResult.Success));
            operation.AddOperationAtTheEnd(GetTestingAtomicOperation(23, items, OperationResult.Success, OperationResult.Success));
            operation.Configuration = new ComplexOperationExecutionConfiguration()
            {
                DoNotRollbackOnExecutionFailure = true
            };

            operation.Execute();

            Assert.That(items, Is.EqualTo(new[] { 4, 8, 15, 16 }));
        }

        private AtomicOperation GetTestingAtomicOperation(int item, 
            IList<int> items, 
            OperationResult executionResult,
            OperationResult rollbackResult)
        {
            return new AtomicOperation()
            {
                ExecutionHandler = () =>
                {
                    items.Add(item);
                    return executionResult;
                },
                RollbackHandler = () =>
                {
                    var index = items.IndexOf(item);
                    items.RemoveAt(index);
                    items.Insert(index, -item);
                    return rollbackResult;
                }
            };
        }
    } 
}
