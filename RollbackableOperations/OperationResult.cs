namespace RollbackableOperations
{
    public class OperationResult
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; } = string.Empty;

        public static OperationResult Success => new OperationResult() { Succeeded = true };

        public static OperationResult Fail(string message = "")
        {
            return new OperationResult() {Message = message};
        }
    }
}