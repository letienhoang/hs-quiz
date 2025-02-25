using Microsoft.Extensions.Localization;

namespace Identity.API.Helpers.Localization
{
    public interface IGenericControllerLocalizer<out T>
    {
        LocalizedString this[string name] { get; }

        LocalizedString this[string name, params object[] arguments] { get; }

        IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures);
    }
}