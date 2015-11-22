using System;

namespace RollbackableOperations.Tests.Stubs
{
    public class OperationStub : IOperation
    {
        public string Id { get; set; }

        public OperationResult Execute()
        {
            throw new NotImplementedException();
        }

        public OperationResult Rollback()
        {
            throw new NotImplementedException();
        }
    }
}