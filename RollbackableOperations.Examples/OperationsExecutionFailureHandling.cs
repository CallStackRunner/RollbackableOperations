using System;

namespace RollbackableOperations.Examples
{
    class OperationsExecutionFailureHandling
    {
        public static void Demostrate()
        {
            /* Let's create a complex operation with fail in the middle of execution */
            var complexOperation = ComplexOperationBuilder.Create()
                .WithStartingOperation(SampleOperations.StartingOperation)
                .WithFollowingOperation(SampleOperations.FirstMiddleOperation)
                .WithFollowingOperation(AtomicOperationBuilder.Create()
                    .WithExecutionHandler(() =>
                    {
                        Console.WriteLine("Fail. Rolling back...");
                        return OperationResult.Fail(); /* <---- execution fail */
                    })
                    .Operation)
                .WithEndingOperation(SampleOperations.EndingOperation) /* will never be fired */
                .Operation;

            complexOperation.Execute();
            /*Output is:
                starting operation executed
                first middle operation executed
                Fail. Rolling back...
                first middle operation rollled back
                starting operation rollled back
            */
        }
    }
}