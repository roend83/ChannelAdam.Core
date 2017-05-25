using ChannelAdam.TestFramework.MSTest;
using System;
using TechTalk.SpecFlow;

namespace ChannelAdam.Core.BehaviourSpecs
{
    [Binding]
    [Scope(Feature = "String Extensions")]
    public class StringExtensionsUnitTestSteps : MoqTestFixture
    {
        private string actualFormattedText;
        private string expectedFormattedText;

        [When(@"a string with named placeholders is formatted")]
        public void WhenAStringWithNamedPlaceholdersIsFormatted()
        {
            var format = "Hello {Name}, a {@Cat} ate my {{{{{ {a {ddd { fff {__{Homework}__}} fff }} ddd}} a}} }}}}"; // without doubling the closing brace, there is a FormatException ;)
            Logger.Log($"The template to format: '{format}'");

            this.expectedFormattedText = "Hello Adam, a burmese ate my essay__} fff } ddd} a} }}";
            Logger.Log($"The expected output: '{this.expectedFormattedText}'");

            this.actualFormattedText = format.FormatNamedPlaceholdersWith("Adam", "burmese", "essay");
        }

        [Then(@"the result has the named placeholders correctly replaced")]
        public void ThenTheResultHasTheNamedPlaceholdersCorrectlyReplaced()
        {
            LogAssert.AreEqual("Expected text", this.expectedFormattedText, this.actualFormattedText);
        }
    }
}
