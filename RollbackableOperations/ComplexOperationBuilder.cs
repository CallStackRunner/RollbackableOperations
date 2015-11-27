namespace RollbackableOperations
{
    /// <summary>
    /// A class implementing fluent API for ComplexOperation instantiation
    /// </summary>
    public class ComplexOperationBuilder : OperationBuilderBase<ComplexOperation>
    {
        private ComplexOperation ConstructedOperation { get; set; }
        private NestedOperation FirstOperation { get; set; } = null;
        private NestedOperation FinalOperation { get; set; } = null;

        protected ComplexOperationBuilder() { }

        /// <summary>
        /// Creating an <c>ComplexOperationBuilder</c>
        /// </summary>
        public static ComplexOperationBuilder Create()
        {
            return new ComplexOperationBuilder() { ConstructedOperation = new ComplexOperation() };
        }

        /// <summary>
        /// Registering an operation which always will be placed before any other.
        /// Each calling of this method will overwrite such operation.
        /// </summary>
        /// <param name="operation">An operation</param>
        /// <param name="configuration">Registered operation execution configuration within containing operation</param>
        public ComplexOperationBuilder WithStartingOperation(IOperation operation,
            OperationExecutionConfiguration configuration = null)
        {
            FirstOperation = new NestedOperation()
            {
                Operation = operation,
                ExecutionConfiguration = configuration
            };
            return this;
        }

        /// <summary>
        /// Inserting a nested operation to the ending of constructing complex one nested operations
        /// </summary>
        /// <param name="operation">An operation</param>
        /// <param name="configuration">Registered operation execution configuration within containing operation</param>
        public ComplexOperationBuilder WithFollowingOperation(IOperation operation, 
            OperationExecutionConfiguration configuration = null)
        {
            ConstructedOperation.AddOperationAtTheEnd(operation, configuration);
            return this;
        }

        /// <summary>
        /// Registering an operation which always will be placed after any other.
        /// Each calling of this method will overwrite such operation.
        /// </summary>
        /// <param name="operation">An operation</param>
        /// <param name="configuration">Registered operation execution configuration within containing operation</param>
        public ComplexOperationBuilder WithEndingOperation(IOperation operation,
            OperationExecutionConfiguration configuration = null)
        {
            FinalOperation = new NestedOperation()
            {
                Operation = operation,
                ExecutionConfiguration = configuration
            };
            return this;
        }

        /// <summary>
        /// Setting a complex operation execution configuration to the constructing one
        /// </summary>
        /// <param name="configuration">Complex operation execution configuration</param>
        public ComplexOperationBuilder WithConfiguration(ComplexOperationExecutionConfiguration configuration)
        {
            ConstructedOperation.Configuration = configuration;
            return this;
        }

        /// <summary>
        /// Constructed complex operation
        /// </summary>
        public new ComplexOperation Operation
        {
            get
            {
                if (FirstOperation != null)
                {
                    ConstructedOperation.AddOperationAtTheStart(FirstOperation.Operation, FirstOperation.ExecutionConfiguration);
                }

                if (FinalOperation != null)
                {
                    ConstructedOperation.AddOperationAtTheEnd(FinalOperation.Operation, FinalOperation.ExecutionConfiguration);
                }

                return ConstructedOperation;
            }
        }
    }
}