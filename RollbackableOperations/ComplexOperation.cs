using System.Collections.Generic;
using System.Linq;

namespace RollbackableOperations
{
    /// <summary>
    /// A class representing composite operation, i.e. operation which execution/rollback means sequential execution/rollback of some nested operations (which could be both of complex and atomic)
    /// </summary>
    public class ComplexOperation : IOperation
    {
        private IList<NestedOperation> NestedOperations { get; set; }  = new List<NestedOperation>();

        /// <summary>
        /// Execution configuration for complex operation
        /// </summary>
        public ComplexOperationExecutionConfiguration Configuration { get; set; } =
            ComplexOperationExecutionConfiguration.Default;

        /// <summary>
        /// Nested operations
        /// </summary>
        public IEnumerable<IOperation> Operations
        {
            get { return NestedOperations.Select(operation => operation.Operation); }
        }

        /// <summary>
        /// Inserting an operation to the begining of the nested operations
        /// </summary>
        /// <param name="operation">An operation</param>
        /// <param name="configuration">Inserted operation execution configuration within containing operation</param>
        public void AddOperationAtTheStart(IOperation operation, OperationExecutionConfiguration configuration = null)
        {
            NestedOperations.Insert(0, new NestedOperation()
            {
                Operation = operation,
                ExecutionConfiguration = configuration ?? OperationExecutionConfiguration.Default
            });
        }

        /// <summary>
        /// Inserting an operation to the ending of the nested operations
        /// </summary>
        /// <param name="operation">An operation</param>
        /// <param name="configuration">Inserted operation execution configuration within containing operation</param>
        public void AddOperationAtTheEnd(IOperation operation, OperationExecutionConfiguration configuration = null)
        {
            NestedOperations.Add(new NestedOperation()
            {
                Operation = operation,
                ExecutionConfiguration = configuration ?? OperationExecutionConfiguration.Default
            });
        }

        /// <summary>
        /// Executing nested operations in order they has been inserted.
        /// </summary>
        /// 
        /// <remarks>
        /// Nested operations are executing either to the moment when no operations left or to the first failed.
        /// In the latter case, if <c>DoNotRollbackOnExecutionFailure</c> configuration option was not specified, complex operation will try to rollback all successfully completed operations (with failed operation itself if corresponding option specified) in reverse order.
        /// If any rollback operation wasn't completed successfully, resulting <c>OperationResult</c> will contain both of datas: cause of execution fail and cause of rollback fail.
        /// </remarks>
        /// 
        /// <returns>An <c>OperationResult</c> containing information about whether your operation executed successfully or not, and a message in latter case</returns>
        public OperationResult Execute()
        {
            foreach (var operation in NestedOperations
                .Select((item, index) => new {item, index}))
            {
                var executionResult = operation.item.Operation.Execute();
                if (!executionResult.Succeeded)
                {
                    if (!Configuration.DoNotRollbackOnExecutionFailure)
                    {
                        foreach (var previousOperation in NestedOperations
                            .Take(operation.item.ExecutionConfiguration.RollbackOperationItselftOnFail
                                ? operation.index + 1
                                : operation.index)
                            .Reverse())
                        {
                            var rollbackResult = previousOperation.Operation.Rollback();
                            if (!rollbackResult.Succeeded)
                            {
                                return
                                    OperationResult.Fail(
                                        $"Execution fail cause: {executionResult.Message}. Rollback fail cause: {rollbackResult.Message}");
                            }
                        }
                    }

                    return executionResult;
                }
            }

            return OperationResult.Success;
        }

        /// <summary>
        /// Tries to rollback all nested operations in reverse order
        /// </summary>
        /// 
        /// <remarks>
        /// If any operation failed during rollback, resulting <c>OperationResult</c> will contain cause of fail
        /// </remarks>
        /// 
        /// <returns>An <c>OperationResult</c> containing information about whether your operation rolled back successfully or not, and a message in latter case</returns>
        public OperationResult Rollback()
        {
            foreach (var operation in NestedOperations
                .Reverse())
            {
                var rollbackResult = operation.Operation.Rollback();
                if (!rollbackResult.Succeeded)
                {
                    return OperationResult.Fail(rollbackResult.Message);
                }
            }

            return OperationResult.Success;
        }
    }
}