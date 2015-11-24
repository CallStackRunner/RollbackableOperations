using System;

namespace RollbackableOperations.Examples
{
    class AtomicOperationsUsage
    {
        public static void Demostrate()
        {
            var atomicOperation = AtomicOperationBuilder.Create()
                .WithExecutionHandler(() =>
                {
                    Console.WriteLine("atomic operation execution");
                    return OperationResult.Success;
                })
                .WithRollbackHandler(() =>
                {
                    Console.WriteLine("atomic operation rollback");
                    return OperationResult.Success;
                })
                .Operation;

            atomicOperation.Execute();
            /*Output is:
                atomic operation execution
            */
            atomicOperation.Rollback();
            /*Output is:
                atomic operation rollback
            */
        }
    }
}