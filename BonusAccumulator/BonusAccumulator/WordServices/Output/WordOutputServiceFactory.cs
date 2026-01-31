namespace BonusAccumulator.WordServices.Output;

public static class WordOutputServiceFactory
{
    public static IWordOutputService Create()
    {
        return new DefaultWordOutputService();
    }
}
