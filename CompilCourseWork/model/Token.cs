public class Token
{
    public int Code { get; set; }
    public string Type { get; set; }
    public string Lexeme { get; set; }
    public string Position { get; set; }
    public override string ToString()
    {
        return $"Токен: {{ Code = {Code}, Type = \"{Type}\", Lexeme = \"{Lexeme}\", Position = \"{Position}\" }}";
    }
}