## What you already can do
1. Split [atomic operation's](https://github.com/CallStackRunner/RollbackableOperations/wiki/Atomic-operation) execution and rollback logic with further independent invocation of each one
2. Provide an operation execution/rollback results in your logic, containing a fact about either success or failure (more useful in composite operations which are discussed below)
3. Build [composite operations](https://github.com/CallStackRunner/RollbackableOperations/wiki/Complex-operation) consisting of either atomic operations or complex ones (with unlimited nesting) and execute/rollback them independently
4. Enjoy [the way](https://github.com/CallStackRunner/RollbackableOperations/wiki/Complex-operation#execution) composite operations are executing, handling execution fails and rolling back operations automatically
5. Use [fluent API](https://github.com/CallStackRunner/RollbackableOperations/wiki/Examples) to create atomic/complex operations (convenience is questionable BTW)

## What you can't do yet
1. Pass parameters in your operation's execution/rollback logic in moment of corresponding invocation
2. Return results from operations except the indication of success (and message if it's not)
3. Fire operation and forget (you should return some operation result)

## Minimal requirements for consumers
.NET 3.5 (because there is usage of Func-s)
