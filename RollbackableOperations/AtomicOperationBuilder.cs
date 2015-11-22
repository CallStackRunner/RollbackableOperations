using System;

namespace RollbackableOperations
{
    public class AtomicOperationBuilder : OperationBuilderBase<AtomicOperation>
    {
        protected AtomicOperationBuilder() { }

        public static AtomicOperationBuilder Create()
        {
            return new AtomicOperationBuilder() { Operation = new AtomicOperation() };
        }

        public AtomicOperationBuilder WithExecutionHandler(Func<OperationResult> handler)
        {
            Operation.ExecutionHandler = handler;
            return this;
        }

        public AtomicOperationBuilder WithRollbackHandler(Func<OperationResult> handler)
        {
            Operation.RollbackHandler = handler;
            return this;
        }
    }
}