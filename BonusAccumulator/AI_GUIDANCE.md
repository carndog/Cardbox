## Non-negotiables

### 1) Do not use `var`

Use explicit types for all local variables and foreach variables.

Good:
```csharp
Dictionary<string, int> counts = new Dictionary<string, int>();
int total = counts.Count;

foreach (KeyValuePair<string, int> pair in counts)
{
    int value = pair.Value;
}
```

Bad:
```csharp
var counts = new Dictionary<string, int>();
var total = counts.Count;

foreach (var pair in counts)
{
    var value = pair.Value;
}
```

Notes:
- This rule applies everywhere: production code, tests, tooling, scripts inside the repo, etc.
- Prefer clarity over brevity.

---

### 2) Do not include description/reason strings in test asserts

Avoid adding message strings / descriptions in assertions. Assertions must be self-explanatory via:
- test name
- arranged data
- assertion itself

Good (xUnit / NUnit):
```csharp
Assert.Equal(expected, actual);
Assert.True(result);
```

Bad (message strings):
```csharp
Assert.True(result, "result should be true");
Assert.Equal(expected, actual, "values should match");
```

Good (FluentAssertions):
```csharp
actual.Should().Be(expected);
result.Should().BeTrue();
```

Bad (reasons):
```csharp
actual.Should().Be(expected, "because it should match");
result.Should().BeTrue("because the operation succeeded");
```

Rationale:
- The test name and structure should explain intent.
- Assertion messages tend to become stale and add noise during refactors.

### 3) Do not use comments in the code

Avoid adding `//` or `/* */` comments in production code and tests.

Use code structure, naming, and small focused methods to make intent obvious.
If something needs explaining, prefer:
- clearer names (types, methods, variables)
- extracting a method to name the concept
- extracting constants to name “magic values”

✅ Good (self-explanatory code)
```csharp
IReadOnlyList<string> tiles = rackParser.Parse(input);
WordCandidate bestPlay = playFinder.FindBest(tiles);
```

---

### 4) Interface definitions must have blank lines between methods

When defining interfaces, include blank lines between each method declaration for readability.

✅ Good (interface with blank lines)
```csharp
public interface IWordService
{
    Answer Anagram(string letters);

    Answer Build(string letters);

    Answer Pattern(string pattern);

    Answer Distance(string word);

    void RunQuiz(QuizOptions options, string endCommand, Action<string> output, Func<string?> input);
}
```

❌ Bad (no blank lines between methods)
```csharp
public interface IWordService
{
    Answer Anagram(string letters);
    Answer Build(string letters);
    Answer Pattern(string pattern);
    Answer Distance(string word);
    void RunQuiz(QuizOptions options, string endCommand, Action<string> output, Func<string?> input);
}
```

Rationale:
- Blank lines improve readability and visual separation
- Makes it easier to scan and locate specific methods
- Consistent with clean code principles for interface definitions

---

## Testing standards

### Act-Assert pattern (no Arrange section)

Test methods must follow the **Arrange-Act-Assert** pattern:
- Arrange should be done via the test name and the minimal setup required.
- If setup grows, extract helpers/builders rather than adding “Arrange” comments.
- Do not add `// Arrange`, `// Act`, `// Assert` comments.

✅ Good (Arrange-Act-Assert, no comments)
```csharp
[Fact]
public void Build_WhenRackContainsBlank_ReturnsWordsUsingBlank()
{
    WordBuilder builder = new WordBuilder(_lexicon);
	
    IReadOnlyList<string> result = builder.Build("A?EINRST");

    result.Should().Contain("ANESTRI"); // example
}

---

## General coding principles

- Make the smallest change that satisfies the requirement.
- Keep formatting consistent with the surrounding file.
- Prefer readability and maintainability over cleverness.

---

## Testing principles

- Use 3 part test names - methodName_Condition_Result.
- Avoid time-dependent tests unless properly controlled (clock abstraction / deterministic time).
