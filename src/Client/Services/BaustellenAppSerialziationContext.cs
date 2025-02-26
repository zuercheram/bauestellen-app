using Baustellen.App.Shared.Models.ViewModels;
using System.Text.Json.Serialization;

namespace Baustellen.App.Client.Services;

[JsonSourceGenerationOptions(
    PropertyNameCaseInsensitive = true,
    NumberHandling = JsonNumberHandling.AllowReadingFromString)]
[JsonSerializable(typeof(UserDto))]
internal partial class BaustellenAppSerialziationContext : JsonSerializerContext
{

}
