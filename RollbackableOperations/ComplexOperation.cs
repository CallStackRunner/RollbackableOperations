using System.Collections.Generic;
using System.Linq;

namespace RollbackableOperations
{
    public class ComplexOperation : IOperation
    {
        private IList<NestedOperation> NestedOperations { get; set; }  = new List<NestedOperation>();

        public ComplexOperationExecutionConfiguration Configuration { get; set; } =
            ComplexOperationExecutionConfiguration.Default;

        public IEnumerable<IOperation> Operations
        {
            get { return NestedOperations.Select(operation => operation.Operation); }
        }

        public void AddOperationAtTheStart(IOperation operation, OperationExecutionConfiguration configuration = null)
        {
            NestedOperations.Insert(0, new NestedOperation()
            {
                Operation = operation,
                ExecutionConfiguration = configuration ?? OperationExecutionConfiguration.Default
            });
        }

        public void AddOperationAtTheEnd(IOperation operation, OperationExecutionConfiguration configuration = null)
        {
            NestedOperations.Add(new NestedOperation()
            {
                Operation = operation,
                ExecutionConfiguration = configuration ?? OperationExecutionConfiguration.Default
            });
        }

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