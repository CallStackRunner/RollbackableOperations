using System;

namespace RollbackableOperations.Examples
{
    class OperationsUnhandledExceptionsHandling
    {
        public static void Demostrate()
        {
            /* If some operation throwed exception, it considered as a failed operation */
            var complexOperation = ComplexOperationBuilder.Create()
                .WithStartingOperation(SampleOperations.StartingOperation)
                .WithFollowingOperation(SampleOperations.FirstMiddleOperation)
                .WithFollowingOperation(AtomicOperationBuilder.Create()
                    .WithExecutionHandler(() =>
                    {
                        throw new Exception("some exception message");
                    })
                    .Operation)
                .WithEndingOperation(SampleOperations.EndingOperation) /* will never be fired */
                .Operation;

            var executionResult = complexOperation.Execute();
            Console.WriteLine($"Rollback reason: {executionResult.Message}"); /* executionResult.Message is equal to "some exception message" */
            /*Output:
                starting operation executed
                first middle operation executed
                first middle operation rollled back
                starting operation rollled back
                Rollback reason: some exception message
            */
        }
    }
}