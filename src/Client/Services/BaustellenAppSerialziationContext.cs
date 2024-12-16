using Baustellen.App.Client.Models.Project;
using System.Text.Json.Serialization;

namespace Baustellen.App.Client.Services;

[JsonSourceGenerationOptions(
    PropertyNameCaseInsensitive = true,
    NumberHandling = JsonNumberHandling.AllowReadingFromString)]
[JsonSerializable(typeof(ProjectItem))]
internal partial class BaustellenAppSerialziationContext : JsonSerializerContext
{

}
