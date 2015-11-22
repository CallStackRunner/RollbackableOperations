using System;

namespace RollbackableOperations.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Atomic operations usage
            Console.WriteLine("Atomic operations usage");

            /* Instantiation */
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
            /* Instantiation */

            /* Execution */
            atomicOperation.Execute();
            Console.ReadKey();
            /* Execution */

            /* Rollback */
            atomicOperation.Rollback();
            Console.ReadKey();
            /* Rollback */

            Console.WriteLine();
            #endregion

            #region Complex operation usage

            Console.WriteLine("Complex operations usage");

            /* Instantiation */
            var complexOperation = ComplexOperationBuilder.Create()
                .WithStartingOperation(StartingOperation)
                .WithFollowingOperation(FirstMiddleOperation)
                .WithFollowingOperation(SecondMiddleOperation)
                .WithEndingOperation(EndingOperation)
                .Operation;
            /* Instantiation */

            /* Execution */
            complexOperation.Execute();
            Console.ReadKey();
            /* Execution */

            /* Rollback */
            complexOperation.Rollback();
            Console.ReadKey();
            /* Rollback */

            Console.WriteLine();
            #endregion

            #region Nested complex operations usage

            Console.WriteLine("Nested complex operations usage");

            /* Instantiation */
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
            /* Instantiation */

            /* Execution */
            topLevelComplexOperation.Execute();
            Console.ReadKey();
            /* Execution */

            /* Rollback */
            topLevelComplexOperation.Rollback();
            Console.ReadKey();
            /* Rollback */

            Console.WriteLine();
            #endregion

            #region Operations execution failure handling

            Console.WriteLine("Operations execution failure handling");

            /* Let's create a complex operation with fail in the middle of execution */
            complexOperation = ComplexOperationBuilder.Create()
                .WithStartingOperation(StartingOperation)
                .WithFollowingOperation(FirstMiddleOperation)
                .WithFollowingOperation(AtomicOperationBuilder.Create()
                    .WithExecutionHandler(() =>
                    {
                        Console.WriteLine("Fail. Rolling back...");
                        return OperationResult.Fail(); /* <---- execution fail */
                    })
                    .Operation)
                .WithEndingOperation(EndingOperation) /* will never be fired */
                .Operation;

            complexOperation.Execute();
            Console.ReadKey();
            Console.WriteLine();

            #endregion

            #region Operation's unhandled exceptions handling

            Console.WriteLine("Operation's unhandled exceptions handling");

            /* If some operation throwed exception, it considered as a failed operation */
            complexOperation = ComplexOperationBuilder.Create()
                .WithStartingOperation(StartingOperation)
                .WithFollowingOperation(FirstMiddleOperation)
                .WithFollowingOperation(AtomicOperationBuilder.Create()
                    .WithExecutionHandler(() =>
                    {
                        throw new Exception("some exception message");
                    })
                    .Operation)
                .WithEndingOperation(EndingOperation) /* will never be fired */
                .Operation;

            var executionResult = complexOperation.Execute();
            Console.WriteLine($"Rollback reason: {executionResult.Message}"); /* executionResult.Message is equal to "some exception message" */
            Console.ReadKey();
            Console.WriteLine();

            #endregion

            #region Failed operation's self-rollback configuration

            Console.WriteLine("Failed operation's self-rollback configuration");

            /* By default in case of operation execution failure, rollback will not invoked on failed operation itself.
               You can override this behaviour with OperationExecutionConfiguration provided to operation builder
            */
            complexOperation = ComplexOperationBuilder.Create()
                .WithStartingOperation(StartingOperation)
                .WithFollowingOperation(FirstMiddleOperation)
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
                .WithEndingOperation(EndingOperation) 
                .Operation;

            complexOperation.Execute();
            Console.ReadKey();
            Console.WriteLine();

            #endregion

            #region Execution rollback failure handling

            Console.WriteLine("Execution rollback failure handling");

            /* If rollback caused by some failed operation failed too then execution result will contain both of datas: reason of execution failure and reason of rollback failure */
            complexOperation = ComplexOperationBuilder.Create()
                .WithStartingOperation(StartingOperation)
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
                        return OperationResult.Fail();
                    }).Operation)
                .WithEndingOperation(EndingOperation)
                .Operation;

            executionResult = complexOperation.Execute();
            Console.WriteLine($"Execution result message: {executionResult.Message}");
            Console.ReadKey();
            Console.WriteLine();

            #endregion
        }

        #region sample operations
        static AtomicOperation StartingOperation => AtomicOperationBuilder.Create()
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
        static AtomicOperation FirstMiddleOperation => AtomicOperationBuilder.Create()
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
        static AtomicOperation SecondMiddleOperation => AtomicOperationBuilder.Create()
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
        static AtomicOperation EndingOperation => AtomicOperationBuilder.Create()
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
        #endregion
    }
}
