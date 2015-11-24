using System;

namespace RollbackableOperations.Examples
{
    class FailedOperationsSelfRollbackConfiguration
    {
        public static void Demostrate()
        {
            /* By default in case of operation execution failure, rollback will not invoked on failed operation itself.
               You can override this behaviour with OperationExecutionConfiguration provided to operation builder
            */
            var complexOperation = ComplexOperationBuilder.Create()
                .WithStartingOperation(SampleOperations.StartingOperation)
                .WithFollowingOperation(SampleOperations.FirstMiddleOperation)
                .WithFollowingOperation(AtomicOperationBuilder.Create()
                    .WithExecutionHandler(() =>
                    {
                        Console.WriteLine("Fail");
                        return OperationResult.Fail();
                    })
                    .WithRollbackHandler(() =>
                    {
                        Console.WriteLine("Failed operation rolling back"); /* by default it will not be fired... */
                        return OperationResult.Success;
                    })
                    .Operation, new OperationExecutionConfiguration() {RollbackOperationItselftOnFail = true}) /* ...but in this way it will */
                .WithEndingOperation(SampleOperations.EndingOperation)
                .Operation;

            complexOperation.Execute();
            /*Output is:
                starting operation executed
                first middle operation executed
                Fail
                Failed operation rolling back
                first middle operation rollled back
                starting operation rollled back
            */
        }
    }
}