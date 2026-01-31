namespace WordServices.Output;

public interface IWordOutputService
{
    string FormatWords(IList<string> words);
}
