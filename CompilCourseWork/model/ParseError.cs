namespace CompilCourseWork.model;

public class ParseError
{
    public string Message { get; }
    public int Position { get; }

    public ParseError(string message, int position)
    {
        Message = message;
        Position = position;
    }

    public override string ToString() => $"Ошибка в позиции {Position}: {Message}";
}