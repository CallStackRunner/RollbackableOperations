using System;

namespace RollbackableOperations.Examples
{
    class ExecutionRollbackFailureHandling
    {
        public static void Demostrate()
        {
            /* If rollback caused by some failed operation failed too then execution result will contain both of datas: reason of execution failure and reason of rollback failure */
            var complexOperation = ComplexOperationBuilder.Create()
                .WithStartingOperation(SampleOperations.StartingOperation)
                .WithFollowingOperation(AtomicOperationBuilder.Create()
                    .WithExecutionHandler(() =>
                    {
                        Console.WriteLine("first middle operation executed");
                        return OperationResult.Success;
                    })
                    .WithRollbackHandler(() => OperationResult.Fail("Rollback failed on the first middle operation"))
                    .Operation)
                .WithFollowingOperation(AtomicOperationBuilder.Create()
                    .WithExecutionHandler(() =>
                    {
                        Console.WriteLine("Fail. Rolling back...");
                        return OperationResult.Fail(); /*message didn't specified*/
                    }).Operation)
                .WithEndingOperation(SampleOperations.EndingOperation)
                .Operation;

            var executionResult = complexOperation.Execute();
            Console.WriteLine($"Execution result message: {executionResult.Message}");
            /*Output is:
                starting operation executed
                first middle operation executed
                Fail. Rolling back...
                Execution result message: Execution fail cause: . Rollback fail cause: Rollback failed on the first middle operation
            */
        }
    }
}