using NFluent;

namespace UnitTests.CsFileContentReplacer;

public class EncodingTest: CsTestContentReplacerTest
{
    [Fact]
    public void TestEncoding()
    {
       var actual= CsFileContentReplacer.Replace("éåäöü");

       Check.That(actual).IsEqualTo("éåäöü");
    }
}