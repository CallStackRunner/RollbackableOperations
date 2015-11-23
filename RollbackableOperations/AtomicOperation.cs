using System;

namespace RollbackableOperations
{
    public class AtomicOperation : IOperation
    {
        public Func<OperationResult> ExecutionHandler { get; set; } = null;
        public Func<OperationResult> RollbackHandler { get; set; } = null;

        public OperationResult Execute()
        {
            if (ExecutionHandler == null)
            {
                return OperationResult.Success;
            }

            try
            {
                return ExecutionHandler.Invoke();
            }
            catch (Exception e)
            {
                return OperationResult.Fail(e.Message);
            }
        }

        public OperationResult Rollback()
        {
            if (RollbackHandler == null)
            {
                return OperationResult.Success;
            }

            try
            {
                return RollbackHandler.Invoke();
            }
            catch (Exception e)
            {
                return OperationResult.Fail(e.Message);
            }
        }
    }
}