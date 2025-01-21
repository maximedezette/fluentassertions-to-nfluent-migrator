# FluentAssertions to NFluent Migrator

[![Hits](https://hits.seeyoufarm.com/api/count/incr/badge.svg?url=https%3A%2F%2Fgithub.com%2Fmaximedezette%2Ffluentassertions-to-nfluent-migrator&count_bg=%2379C83D&title_bg=%23555555&icon=&icon_color=%23E7E7E7&title=PAGE+VIEWS&edge_flat=false)](https://hits.seeyoufarm.com)

A simple command-line tool for migrating your code from **FluentAssertions** to **NFluent**. This tool automatically updates all `using` directives and assertion statements in your C# project files (`.cs` and `.csproj` files) to use **NFluent** syntax instead of **FluentAssertions**.

## Features
- Replaces `using FluentAssertions` with `using NFluent`.
- Converts FluentAssertions assertions to equivalent NFluent assertions (e.g., `.Should().Be()` → `.IsEqualTo()`).
- Supports `.cs` and `.csproj` files.
- Process multiple files in a directory and subdirectories.
  
![how_to_part_1](https://github.com/user-attachments/assets/dcedb34c-e643-446d-9123-ae53eb329b91)

### Example Usage

[![demo](https://img.youtube.com/vi/oVfizaskDAU/0.jpg)](https://www.youtube.com/watch?v=oVfizaskDAU)

⚠️ **As of today, not all assertions are fully supported by the tool, and some manual transformations are likely to be required. However, it significantly eases the migration process by automating many common cases.**


See the section about the [supported assertions](#fluentAssertions-to-nfluent-migration-support) for more details.

## Installation

### Prerequisites
- **.NET 6.0+** SDK
- C# development environment (e.g., Visual Studio, Rider, or Visual Studio Code)

## Usage

### Run the Tool

Once the project is built, you can use the tool by running the compiled executable from the command line.

1. Clone the repository:
    ```bash
    git clone https://github.com/yourusername/fluentassertions-to-nfluent-migrator.git
    cd fluentassertions-to-nfluent-migrator
    ```

2. Build the project using the .NET CLI:
    ```bash
    dotnet build -o ./output    
    ```
   
### Command Format

```bash
.\output\Migrator.exe <path-to-directory>
```

Where <path-to-directory> is the path to the directory containing the C# project files (.cs and .csproj). The tool will process all matching files in the specified directory and its subdirectories.

### Expected Output

While the tool runs, you'll see a message indicating the progress of the replacement, along with a spinner:

```bash
Replacement started
Processing .cs files...
Processing SomeFile.cs...|
Processing AnotherFile.cs.../
Processing YetAnotherFile.cs...-
...
Replacement complete.
```

Once the replacement is complete, it will output:

Replacement complete.

File Types Processed

    .cs: C# source files containing FluentAssertions assertions.
    .csproj: Project files that may include references to FluentAssertions.

File Content Changes

  *  All occurrences of using FluentAssertions are replaced with using NFluent.
  *  Assertions using FluentAssertions methods (e.g., .Should().Be(...)) are updated to the equivalent NFluent assertion (e.g., .IsEqualTo(...)).

Example Changes

Before:

```c#
using FluentAssertions;

public void Test()
{
    var myObject = new MyClass();
    myObject.Should().Be(expectedObject);
}
```

After:

```c#
using NFluent;

public void Test()
{
    var myObject = new MyClass();
    Check.That(myObject).IsEqualTo(expectedObject);
}
```

## Customizing the Tool

You can customize or extend the tool's functionality by modifying the following classes:

   * `CsFileContentReplacer`: Contains the logic for replacing assertions in .cs files.
   * `CsProjFileContentReplacer`: Contains the logic for replacing content in .csproj files (e.g., using FluentAssertions).
   * `Program`: The entry point of the tool where file processing and updates are triggered.

## Known Issues

Complex Expressions: In cases where assertions are deeply nested or formatted irregularly, the tool may not fully capture the exact syntax. You may need to review the resulting code to ensure correctness.

This tool is under development, some FluentAssertions assertions may not be correctly replaced yet. 
   
## Contributions

Feel free to fork this repository and submit pull requests for any features or fixes you think are useful!

It's a very simple project, here is how you can add a transformation:

* Write a unit test like the following:

```c#
  [Fact]
  public void Should_replace_ShouldBeTrue()
  {
  const string fluentAssertions = "var.Should().BeTrue();";
  const string nfluentEquivalent = "Check.That(var).IsTrue();";

        var actual = CsFileContentReplacer.Replace(fluentAssertions);
        
        Check.That(actual).IsEqualTo(nfluentEquivalent);
  }
```

Add the regex to the `CsFileContentReplacer.cs` to make the test pass:

```c#
// .Should().BeTrue() -> Check.That(var).IsTrue();
(@"(?<subject>\S(?:.*\S)?)\s*\.Should\(\)\s*\.BeTrue\s*\(\s*\)\s*;", "Check.That(${subject}).IsTrue();"),
```

And that's it!

# FluentAssertions to NFluent Migration Support

This document provides an overview of the migration support offered by the `CsFileContentReplacer` for transitioning from FluentAssertions to NFluent in C# projects. While the tool automates the replacement of many common assertions, there are limitations, and some manual adjustments may be necessary.

## Supported Transformations

The following FluentAssertions methods are currently supported and are automatically replaced by their NFluent equivalents:

### General Assertions
| **FluentAssertions**                       | **NFluent**                                   |
|--------------------------------------------|-----------------------------------------------|
| `.Should().NotBeNull()`                    | `Check.That(var).IsNotNull();`               |
| `.Should().BeNull()`                       | `Check.That(var).IsNull();`                  |
| `.Should().Be(value)`                      | `Check.That(var).IsEqualTo(value);`          |
| `.Should().NotBe(value)`                   | `Check.That(var).IsNotEqualTo(value);`       |
| `.Should().BeGreaterThan(value)`           | `Check.That(var).IsGreaterThan(value);`      |
| `.Should().BeGreaterOrEqualTo(value)`      | `Check.That(var).IsGreaterOrEqualTo(value);` |
| `.Should().BeLessThan(value)`              | `Check.That(var).IsLessThan(value);`         |
| `.Should().BeLessOrEqualTo(value)`         | `Check.That(var).IsLessOrEqualTo(value);`    |
| `.Should().BeEquivalentTo(object)`         | `Check.That(var).HasFieldsWithSameValues(object);` |
| `.Should().Contain(value)`                 | `Check.That(var).Contains(value);`           |
| `.Should().OnlyContain(value)`             | `Check.That(var).ContainsOnlyElementsThatMatch(value);` |
| `.Should().HaveCount(value)`               | `Check.That(var).HasSize(value);`            |
| `.Should().HaveSameCount(value)`           | `Check.That(var).HasSameSizeAs(value);`      |
| `.Should().NotContain(value)`              | `Check.That(var).Not.Contains(value);`       |
| `.Should().BeEmpty()`                      | `Check.That(var).IsEmpty();`                 |
| `.Should().NotBeEmpty()`                   | `Check.That(var).IsNotEmpty();`              |
| `.Should().StartWith(value)`               | `Check.That(var).StartsWith(value);`         |
| `.Should().EndWith(value)`                 | `Check.That(var).EndsWith(value);`           |

### String Assertions

| **FluentAssertions**                | **NFluent**                                 |
|-------------------------------------|---------------------------------------------|
| `.Should().NotBeNull()`             | `Check.That(var).IsNotNull();`              |
| `.Should().BeNull()`                | `Check.That(var).IsNull();`                 |
| `.Should().BeEmpty()`               | `Check.That(var).IsEmpty();`                |
| `.Should().NotBeEmpty()`            | `Check.That(var).Not.IsEmpty();`            |
| `.Should().HaveLength(value)`       | `Check.That(var).HasSize(value);`           |
| `.Should().BeNullOrWhiteSpace()`    | `Check.That(var).IsNullOrWhiteSpace()`      |
| `.Should().NotBeNullOrWhiteSpace()` | `Check.That(var).Not.IsNullOrWhiteSpace();` |

### Boolean Assertions
| **FluentAssertions**               | **NFluent**                        |
|------------------------------------|------------------------------------|
| `.Should().BeTrue()`               | `Check.That(var).IsTrue();`        |
| `.Should().BeFalse()`              | `Check.That(var).IsFalse();`       |
| `.Should().NotBeTrue()`            | `Check.That(var).Not.IsTrue();`    |
| `.Should().NotBeFalse()`           | `Check.That(var).Not.IsFalse();`   |
| `.Should().Imply(other)`           | `Check.That(var).Imply(other);`    |

### Nullable Assertions
| **FluentAssertions**               | **NFluent**                        |
|------------------------------------|------------------------------------|
| `.Should().NotHaveValue()`         | `Check.That(var).Not.HasValue();`  |
| `.Should().HaveValue()`            | `Check.That(var).HasValue();`      |
| `.Should().Match(predicate)`       | `Check.That(var).Matches(predicate);` |

### Exception Assertions
| **FluentAssertions**                       | **NFluent**                                   |
|--------------------------------------------|-----------------------------------------------|
| `.Should().Throw<ExceptionType>()`         | `Check.ThatCode(action).Throws<ExceptionType>();` |
| `.Should().ThrowExactly<ExceptionType>()`  | `Check.ThatCode(action).ThrowsExactly<ExceptionType>();` |
| `.Should().NotThrow()`                     | `Check.ThatCode(action).DoesNotThrow();`      |

## Limitations

While the `CsFileContentReplacer` automates many assertions, some complex scenarios may require manual intervention, such as:

1. **Unsupported Assertions:** Certain FluentAssertions methods are not yet supported.
2. **Complex Lambda Expressions:** Assertions using intricate lambda expressions may not be transformed correctly.
3. **Formatting:** Multi-line or poorly formatted assertions might produce unexpected output.

### Example of Partial Support
For multi-line assertions, while the tool handles most cases, some extra spaces may appear in the output and need to be manually fixed:

**Input:**
```csharp
complexVar.Should()
    .BeEquivalentTo(
        new[]
        {
            Id = 1,
            Name = "Test"
        });
```

**Output:**
```csharp
Check.That(complexVar).HasFieldsWithSameValues(new[] { Id = 1, Name = "Test" });
```

                
