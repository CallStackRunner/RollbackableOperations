using System;

namespace RollbackableOperations
{
    /// <summary>
    /// A class implementing fluent API for AtomicOperation instantiation
    /// </summary>
    public class AtomicOperationBuilder : OperationBuilderBase<AtomicOperation>
    {
        protected AtomicOperationBuilder() { }

        /// <summary>
        /// Creating an <c>AtomicOperationBuilder</c>
        /// </summary>
        public static AtomicOperationBuilder Create()
        {
            return new AtomicOperationBuilder() { Operation = new AtomicOperation() };
        }

        /// <summary>
        /// Setting an execution handler for constructing operation
        /// </summary>
        /// <param name="handler">Operation execution handler</param>
        public AtomicOperationBuilder WithExecutionHandler(Func<OperationResult> handler)
        {
            Operation.ExecutionHandler = handler;
            return this;
        }

        /// <summary>
        /// Setting a rollback handler for constructing operation
        /// </summary>
        /// <param name="handler">Operation rollback handler</param>
        public AtomicOperationBuilder WithRollbackHandler(Func<OperationResult> handler)
        {
            Operation.RollbackHandler = handler;
            return this;
        }
    }
}