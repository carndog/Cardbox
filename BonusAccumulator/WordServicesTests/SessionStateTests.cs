using BonusAccumulator.WordServices;
using FluentAssertions;

namespace WordServicesTests;

[TestFixture]
public class SessionStateTests
{
    [Test]
    public void SaveAdded_DoubleNewlines()
    {
        string tempPath = Path.GetTempFileName();
        TestSettingsProvider settingsProvider = new TestSettingsProvider(tempPath);
        SessionState sessionState = new SessionState(settingsProvider);
        
        sessionState.Add(new[] { "WORD1", "WORD2" });
        string savedPath = sessionState.SaveAdded();
        
        string content = File.ReadAllText(savedPath);
        content.Should().NotContain("\r\n\r\n", "File should not contain double newlines");
        
        File.Delete(tempPath);
    }

    [Test]
    public void SaveAdded_MissingFormatString()
    {
        TestSettingsProvider settingsProvider = new TestSettingsProvider("");
        SessionState sessionState = new SessionState(settingsProvider);
        
        sessionState.Add(new[] { "WORD1" });
        
        Action act = () => sessionState.SaveAdded();
        
        act.Should().Throw<InvalidOperationException>();
    }
}
