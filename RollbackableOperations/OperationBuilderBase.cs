namespace RollbackableOperations
{
    public abstract class OperationBuilderBase<TOperationType>
    {
        /// <summary>
        /// Constructed operation
        /// </summary>
        public TOperationType Operation { get; protected set; }
    }
}