using System;

namespace RollbackableOperations.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Atomic operations usage");
            AtomicOperationsUsage.Demostrate();
            Console.ReadKey();

            Console.WriteLine("Complex operations usage");
            ComplexOperationsUsage.Demostrate();
            Console.ReadKey();

            Console.WriteLine("Nested complex operations usage");
            NestedComplexOperationsUsage.Demostrate();
            Console.ReadKey();

            Console.WriteLine("Operations execution failure handling");
            OperationsExecutionFailureHandling.Demostrate();
            Console.ReadKey();

            Console.WriteLine("Operation's unhandled exceptions handling");
            OperationsUnhandledExceptionsHandling.Demostrate();
            Console.ReadKey();

            Console.WriteLine("Failed operation's self-rollback configuration");
            FailedOperationsSelfRollbackConfiguration.Demostrate();
            Console.ReadKey();

            Console.WriteLine("Execution rollback failure handling");
            ExecutionRollbackFailureHandling.Demostrate();
            Console.ReadKey();
        }
    }
}
