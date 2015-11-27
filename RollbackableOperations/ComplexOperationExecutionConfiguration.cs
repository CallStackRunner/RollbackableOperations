namespace RollbackableOperations
{
    /// <summary>
    /// A class respresenting complex operation execution configuration
    /// </summary>
    public class ComplexOperationExecutionConfiguration
    {
        /// <summary>
        /// Indicates whether complex operation should try to rollback operations in case of execution failure or not
        /// </summary>
        public bool DoNotRollbackOnExecutionFailure { get; set; }

        public static ComplexOperationExecutionConfiguration Default => new ComplexOperationExecutionConfiguration();
    }
}