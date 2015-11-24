namespace RollbackableOperations
{
    public class ComplexOperationExecutionConfiguration
    {
        public bool DoNotRollbackOnExecutionFailure { get; set; }

        public static ComplexOperationExecutionConfiguration Default => new ComplexOperationExecutionConfiguration();
    }
}