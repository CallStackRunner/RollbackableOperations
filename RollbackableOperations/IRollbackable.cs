namespace RollbackableOperations
{
    public interface IRollbackable
    {
        OperationResult Rollback();
    }
}