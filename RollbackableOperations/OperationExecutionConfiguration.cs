namespace RollbackableOperations
{
    public class OperationExecutionConfiguration
    {
        public bool RollbackOperationItselftOnFail { get; set; }

        public static OperationExecutionConfiguration Default => new OperationExecutionConfiguration();
    }
}