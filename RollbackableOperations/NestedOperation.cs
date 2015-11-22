namespace RollbackableOperations
{
    public class NestedOperation
    {
        public IOperation Operation { get; set; }
        public OperationExecutionConfiguration ExecutionConfiguration { get; set; }
    }
}