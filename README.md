# FluentAssertions to NFluent Migrator

A simple command-line tool for migrating your code from **FluentAssertions** to **NFluent**. This tool automatically updates all `using` directives and assertion statements in your C# project files (`.cs` and `.csproj` files) to use **NFluent** syntax instead of **FluentAssertions**.

## Features
- Replaces `using FluentAssertions` with `using NFluent`.
- Converts FluentAssertions assertions to equivalent NFluent assertions (e.g., `.Should().BeEquivalentTo()` → `.IsEquivalentTo()`).
- Supports `.cs` and `.csproj` files.
- Process multiple files in a directory and subdirectories.

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
    cd fluentassertions-to-nfluent-migrator/fluentassertions-to-nfluent-migrator
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


### Example Usage

[![demo](https://img.youtube.com/vi/iXzMbR6yFEQ/0.jpg)](https://www.youtube.com/watch?v=iXzMbR6yFEQ)

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
                