using System.Runtime.Serialization;
 
[Serializable]
public class BusinessException : Exception
{
    public int ErrorCode { get; }
 
    public BusinessException()
    {
    }
 
    public BusinessException(string message)
        : base(message)
    {
    }
 
    public BusinessException(string message, Exception inner)
        : base(message, inner)
    {
    }
 
    public BusinessException(string message, int errorCode)
        : base(message)
    {
        ErrorCode = errorCode;
    }
 
    protected BusinessException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        if (info != null)
        {
            ErrorCode = info.GetInt32(nameof(ErrorCode));
        }
    }
 
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info != null)
        {
            info.AddValue(nameof(ErrorCode), ErrorCode);
        }
        base.GetObjectData(info, context);
    }
}