namespace RollbackableOperations
{
    public interface IOperation: IExecutable, IRollbackable
    {
    }
}