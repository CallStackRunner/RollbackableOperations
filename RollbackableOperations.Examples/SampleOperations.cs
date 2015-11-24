using System;

namespace RollbackableOperations.Examples
{
    class SampleOperations
    {
        public static AtomicOperation StartingOperation => AtomicOperationBuilder.Create()
            .WithExecutionHandler(() =>
            {
                Console.WriteLine("starting operation executed");
                return OperationResult.Success;
            })
            .WithRollbackHandler(() =>
            {
                Console.WriteLine("starting operation rollled back");
                return OperationResult.Success;
            }).Operation;
        public static AtomicOperation FirstMiddleOperation => AtomicOperationBuilder.Create()
            .WithExecutionHandler(() =>
            {
                Console.WriteLine("first middle operation executed");
                return OperationResult.Success;
            })
            .WithRollbackHandler(() =>
            {
                Console.WriteLine("first middle operation rollled back");
                return OperationResult.Success;
            }).Operation;
        public static AtomicOperation SecondMiddleOperation => AtomicOperationBuilder.Create()
            .WithExecutionHandler(() =>
            {
                Console.WriteLine("second middle operation executed");
                return OperationResult.Success;
            })
            .WithRollbackHandler(() =>
            {
                Console.WriteLine("second middle operation rollled back");
                return OperationResult.Success;
            }).Operation;
        public static AtomicOperation EndingOperation => AtomicOperationBuilder.Create()
            .WithExecutionHandler(() =>
            {
                Console.WriteLine("ending operation executed");
                return OperationResult.Success;
            })
            .WithRollbackHandler(() =>
            {
                Console.WriteLine("ending operation rollled back");
                return OperationResult.Success;
            }).Operation;
    }
}