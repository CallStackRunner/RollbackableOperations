namespace RollbackableOperations
{
    public abstract class OperationBuilderBase<TOperationType>
    {
        public TOperationType Operation { get; protected set; }
    }
}