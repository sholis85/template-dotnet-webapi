using System.Globalization;
using Microsoft.Extensions.Localization;
using Xunit;

namespace Infrastructure.Test.Localization;

public class LocalizationTests
{
    private const string _testString = "testString";
    private const string _testStringInSpanish = "testString in spanish";
    private const string _testStringInSpainSpanish = "testString in spain spanish";
    private const string _testString2 = "testString2";
    private const string _testString2InSpanish = "testString2 in spanish";

    private readonly IStringLocalizer _localizer;

    public LocalizationTests(IStringLocalizer<LocalizationTests> localizer) => _localizer = localizer;

    // there's no "en-US" folder
    // "es-ES/test.po" only contains testString
    // "es/test.po" contains both testString and testString2
    [Theory]
    [InlineData("en-US", _testString, _testString)]
    [InlineData("es-AR", _testString, _testStringInSpanish)]
    [InlineData("es-ES", _testString, _testStringInSpainSpanish)]
    [InlineData("es-AR", _testString2, _testString2InSpanish)]
    [InlineData("es-ES", _testString2, _testString2InSpanish)]
    public void TranslateToCultureTest(string culture, string testString, string translatedString)
    {
        Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(culture);

        var result = _localizer[testString];

        Assert.Equal(translatedString, result);
    }
}