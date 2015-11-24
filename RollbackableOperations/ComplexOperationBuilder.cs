namespace RollbackableOperations
{
    public class ComplexOperationBuilder : OperationBuilderBase<ComplexOperation>
    {
        private ComplexOperation ConstructedOperation { get; set; }
        private NestedOperation FirstOperation { get; set; } = null;
        private NestedOperation FinalOperation { get; set; } = null;

        protected ComplexOperationBuilder() { }

        public static ComplexOperationBuilder Create()
        {
            return new ComplexOperationBuilder() { ConstructedOperation = new ComplexOperation() };
        }

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

        public ComplexOperationBuilder WithFollowingOperation(IOperation operation, 
            OperationExecutionConfiguration configuration = null)
        {
            ConstructedOperation.AddOperationAtTheEnd(operation, configuration);
            return this;
        }

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

        public ComplexOperationBuilder WithConfiguration(ComplexOperationExecutionConfiguration configuration)
        {
            ConstructedOperation.Configuration = configuration;
            return this;
        }

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