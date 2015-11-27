using System;

namespace RollbackableOperations
{
    /// <summary>
    /// A class representing non-decomposable operation, that could be executed and rolled back
    /// </summary>
    public class AtomicOperation : IOperation
    {
        /// <summary>
        /// An execution handler invoking by <c>Execute</c> method
        /// </summary>
        /// 
        /// <remarks>
        /// If it doesn't provided, result of operation execution will be considered as success
        /// </remarks>
        public Func<OperationResult> ExecutionHandler { get; set; } = null;

        /// <summary>
        /// A rollback handler invoking by <c>Rollback</c> method
        /// </summary>
        /// 
        /// <remarks>
        /// If it doesn't provided, result of operation rollback will be considered as success
        /// </remarks>
        public Func<OperationResult> RollbackHandler { get; set; } = null;

        /// <summary>
        /// Executing operation using provided <c>ExecutionHandler</c>
        /// </summary>
        /// 
        /// <returns>
        /// An <c>OperationResult</c> representing result of execution or <c>OperationResult.Success</c> if <c>ExecutionHandler</c> is null
        /// </returns>
        /// 
        /// <remarks>
        /// If you handler doesn't catched exception it throws, OperationResult will be considered as fail and contain an exception message
        /// </remarks>
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

        /// <summary>
        /// Rolling back an operation using provided <c>RollbackHandler</c>
        /// </summary>
        /// 
        /// <returns>
        /// An <c>OperationResult</c> representing result of rollback or <c>OperationResult.Success</c> if <c>RollbackHandler</c> is null
        /// </returns>
        /// 
        /// <remarks>
        /// If you handler doesn't catched exception it throws, OperationResult will be considered as fail and contain an exception message
        /// </remarks>
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