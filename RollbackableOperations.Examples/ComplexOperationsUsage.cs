namespace RollbackableOperations.Examples
{
    class ComplexOperationsUsage
    {
        public static void Demostrate()
        {
            var complexOperation = ComplexOperationBuilder.Create()
                .WithStartingOperation(SampleOperations.StartingOperation)
                .WithFollowingOperation(SampleOperations.FirstMiddleOperation)
                .WithFollowingOperation(SampleOperations.SecondMiddleOperation)
                .WithEndingOperation(SampleOperations.EndingOperation)
                .Operation;

            complexOperation.Execute();
            /*Output is:
                starting operation executed
                first middle operation executed
                second middle operation executed
                ending operation executed
            */

            complexOperation.Rollback();
            /*Output is:
                ending operation rollled back
                second middle operation rollled back
                first middle operation rollled back
                starting operation rollled back
            */
        }
    }
}
