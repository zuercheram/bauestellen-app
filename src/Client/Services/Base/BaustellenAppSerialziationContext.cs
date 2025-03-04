using Baustellen.App.Shared.Models.InputModel;
using Baustellen.App.Shared.Models.ViewModels;
using System.Text.Json.Serialization;

namespace Baustellen.App.Client.Services;

[JsonSourceGenerationOptions(
    PropertyNameCaseInsensitive = true,
    NumberHandling = JsonNumberHandling.AllowReadingFromString)]
[JsonSerializable(typeof(UserDto))]
[JsonSerializable(typeof(List<UserDto>))]
[JsonSerializable(typeof(ProjectInputDto))]
[JsonSerializable(typeof(ProjectSyncInputDto))]
[JsonSerializable(typeof(ProjectUpdateInputDto))]
[JsonSerializable(typeof(RequestProjectsInputDto))]
[JsonSerializable(typeof(BackendStateDto))]
[JsonSerializable(typeof(ExternalLinkViewDto))]
[JsonSerializable(typeof(ProjectViewDto))]
[JsonSerializable(typeof(RequestProjectViewDto))]
[JsonSerializable(typeof(SyncProjectsViewDto))]
public partial class BaustellenAppSerialziationContext : JsonSerializerContext
{

}
