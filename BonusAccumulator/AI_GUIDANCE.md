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

Good (NUnit):
```csharp
Assert.That(expected, Is.EqualTo(actual));
Assert.That(result, Is.True);
```

Bad (message strings):
```csharp
Assert.That(result, Is.True, "result should be true");
Assert.That(expected, Is.EqualTo(actual), "values should match");
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
- extracting constants to name "magic values"

✅ Good (self-explanatory code)
```csharp
IReadOnlyList<string> tiles = rackParser.Parse(input);
WordCandidate bestPlay = playFinder.FindBest(tiles);
```

---

### 4) Use private methods instead of local methods

When extracting helper logic, prefer regular private methods over local methods (functions defined inside other methods).

✅ Good (private method)
```csharp
public void ProcessData()
{
    string result = FormatOutput(data);
    WriteLine(result);
}

private string FormatOutput(Data data)
{
    return $"{data.Id}: {data.Name}";
}
```

❌ Bad (local method)
```csharp
public void ProcessData()
{
    string FormatOutput(Data data) => $"{data.Id}: {data.Name}";
    
    string result = FormatOutput(data);
    WriteLine(result);
}
```

Rationale:
- Private methods are more discoverable and testable
- Better for code organization and reusability
- Easier to debug and step through
- Consistent with traditional C# patterns

---

### 5) Interface definitions must have blank lines between methods

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

### 6) One file per type

Each type (class, interface, record, enum) must be in its own file.

✅ Good (one type per file)
```
/Project/
  IGetDeckStatsByCardbox.cs     // Contains only IGetDeckStatsByCardbox interface
  GetDeckStatsByCardbox.cs     // Contains only GetDeckStatsByCardbox class
  CardboxStats.cs             // Contains only CardboxStats record
```

❌ Bad (multiple types in one file)
```
/Project/
  AnalyticsQueries.cs         // Contains IHealthCheckQueries, IDueQueries, ILeechedQueries
  ResultTypes.cs              // Contains CardboxStats, DueItem, ErrorRateStats
```

Rationale:
- Follows Single Responsibility Principle
- Easier to navigate and find specific types
- Reduces merge conflicts
- Cleaner file organization
- Each file has a single, clear purpose

---

## Testing standards

### Act-Assert pattern (no Arrange section)

Test methods must follow the **Arrange-Act-Assert** pattern using NUnit:
- Arrange should be done via the test name and the minimal setup required.
- If setup grows, extract helpers/builders rather than adding "Arrange" comments.
- Do not add `// Arrange`, `// Act`, `// Assert` comments.
- Use `[Test]` attribute for test methods and `[SetUp]` for test initialization.

✅ Good (Arrange-Act-Assert, no comments, NUnit)
```csharp
[Test]
public void Build_WhenRackContainsBlank_ReturnsWordsUsingBlank()
{
    WordBuilder builder = new WordBuilder(_lexicon);
    
    IReadOnlyList<string> result = builder.Build("A?EINRST");

    Assert.That(result, Does.Contain("ANESTRI"));
}
```

---

## General coding principles

- Make the smallest change that satisfies the requirement.
- Keep formatting consistent with the surrounding file.
- Prefer readability and maintainability over cleverness.

---

## Testing principles

- Use 3 part test names - methodName_Condition_Result.
- Avoid time-dependent tests unless properly controlled (clock abstraction / deterministic time).
- All tests must use NUnit framework with `[Test]`, `[SetUp]`, `[TearDown]`, and `[TestFixture]` attributes.
- Use NUnit assertions: `Assert.That(actual, Is.EqualTo(expected))` instead of MSTest assertions.
