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
                throw new ExecutionHandlerNotSpecifiedException();
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
                throw new RollbackHandlerNotSpecifiedException();
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