namespace RollbackableOperations
{
    /// <summary>
    /// A class represents configuration of operation execution within some complex operation
    /// </summary>
    public class OperationExecutionConfiguration
    {
        /// <summary>
        /// Indicates whether complex operation should try to rollback failed operation itself in case of failure or not
        /// </summary>
        public bool RollbackOperationItselftOnFail { get; set; }

        public static OperationExecutionConfiguration Default => new OperationExecutionConfiguration();
    }
}