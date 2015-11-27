namespace RollbackableOperations
{
    /// <summary>
    /// A class represents result of some operation
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        /// Indicated whether operation finished successfully or not
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// Contains a message provided by operation
        /// </summary>
        /// 
        /// <remarks>
        /// Usually this property is not empty in case of operation failure
        /// </remarks>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Providing <c>OperationResult</c>, indicating successfull result
        /// </summary>
        public static OperationResult Success => new OperationResult() { Succeeded = true };

        /// <summary>
        /// Providing <c>OperationResult</c> indicating unsuccessfull result
        /// </summary>
        /// <param name="message">A message for <c>OperationResult.Message</c> property</param>
        public static OperationResult Fail(string message = "")
        {
            return new OperationResult() {Message = message};
        }
    }
}