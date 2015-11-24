using System;

namespace RollbackableOperations.Examples
{
    class NestedComplexOperationsUsage
    {
        public static void Demostrate()
        {
            var topLevelComplexOperation = ComplexOperationBuilder.Create()
                .WithStartingOperation(AtomicOperationBuilder.Create()
                    .WithExecutionHandler(() =>
                    {
                        Console.WriteLine("1 started");
                        return OperationResult.Success;
                    })
                    .WithRollbackHandler(() =>
                    {
                        Console.WriteLine("1 rolled back");
                        return OperationResult.Success;
                    })
                    .Operation)
                .WithFollowingOperation(ComplexOperationBuilder.Create()
                    .WithStartingOperation(AtomicOperationBuilder.Create()
                        .WithExecutionHandler(() =>
                        {
                            Console.WriteLine("2.1 started");
                            return OperationResult.Success;
                        })
                        .WithRollbackHandler(() =>
                        {
                            Console.WriteLine("2.1 rolled back");
                            return OperationResult.Success;
                        })
                        .Operation)
                    .WithFollowingOperation(AtomicOperationBuilder.Create()
                        .WithExecutionHandler(() =>
                        {
                            Console.WriteLine("2.2 started");
                            return OperationResult.Success;
                        })
                        .WithRollbackHandler(() =>
                        {
                            Console.WriteLine("2.2 rolled back");
                            return OperationResult.Success;
                        })
                        .Operation)
                    .WithEndingOperation(AtomicOperationBuilder.Create()
                        .WithExecutionHandler(() =>
                        {
                            Console.WriteLine("2.3 started");
                            return OperationResult.Success;
                        })
                        .WithRollbackHandler(() =>
                        {
                            Console.WriteLine("2.3 rolled back");
                            return OperationResult.Success;
                        })
                        .Operation)
                    .Operation)
                .WithEndingOperation(AtomicOperationBuilder.Create()
                    .WithExecutionHandler(() =>
                    {
                        Console.WriteLine("3 started");
                        return OperationResult.Success;
                    })
                    .WithRollbackHandler(() =>
                    {
                        Console.WriteLine("3 rolled back");
                        return OperationResult.Success;
                    })
                    .Operation)
                .Operation;

            topLevelComplexOperation.Execute();
            /*Output is:
                1 started
                2.1 started
                2.2 started
                2.3 started
                3 started
            */

            topLevelComplexOperation.Rollback();
            /*Output is:
                3 rolled back
                2.3 rolled back
                2.2 rolled back
                2.1 rolled back
                1 rolled back
            */
        }
    }
}