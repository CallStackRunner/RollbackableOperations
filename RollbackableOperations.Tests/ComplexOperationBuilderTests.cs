using NUnit.Framework;
using RollbackableOperations.Tests.Stubs;

namespace RollbackableOperations.Tests
{
    [TestFixture]
    public class ComplexOperationBuilderTests
    {
        [Test]
        public void ComplexOperationBuilderShouldConstructOperationWithStartingHandlerBeforeAnyOther()
        {
            var stubs = new[]
            {
                GetOperationStub("1"),
                GetOperationStub("2"),
                GetOperationStub("3")
            };
            var operation = ComplexOperationBuilder.Create()
                .WithStartingOperation(stubs[0])
                .WithFollowingOperation(stubs[1])
                .WithFollowingOperation(stubs[2]);
            Assert.That(operation.Operation.Operations, Is.EqualTo(stubs));
        }

        [Test]
        public void ComplexOperationBuilderShouldConstructOperationWithEndingHandlerAfterAnyOther()
        {
            var stubs = new[]
            {
                GetOperationStub("1"),
                GetOperationStub("2"),
                GetOperationStub("3")
            };
            var operation = ComplexOperationBuilder.Create()
                .WithEndingOperation(stubs[2])
                .WithFollowingOperation(stubs[0])
                .WithFollowingOperation(stubs[1]);
            Assert.That(operation.Operation.Operations, Is.EqualTo(stubs));
        }

        [Test]
        public void ComplexOperationBuilderShouldConstructOperationWithNestedOperationsFollowingInRightOrder()
        {
            var stubs = new[]
            {
                GetOperationStub("1"),
                GetOperationStub("2"),
                GetOperationStub("3")
            };
            var operation = ComplexOperationBuilder.Create()
                .WithFollowingOperation(stubs[0])
                .WithFollowingOperation(stubs[1])
                .WithFollowingOperation(stubs[2]);
            Assert.That(operation.Operation.Operations, Is.EqualTo(stubs));
        }

        [Test]
        public void ComplexOperationBuilderShouldConstructOperationWithConfiguration()
        {
            var operation = ComplexOperationBuilder.Create()
                .WithConfiguration(new ComplexOperationExecutionConfiguration() {DoNotRollbackOnExecutionFailure = true})
                .Operation;
            Assert.That(operation.Configuration.DoNotRollbackOnExecutionFailure, Is.EqualTo(true));
        }

        private IOperation GetOperationStub(string id)
        {
            return new OperationStub() {Id = id};
        }
    }
}
