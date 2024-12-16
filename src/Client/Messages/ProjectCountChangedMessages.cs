using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Baustellen.App.Client.Messages;

internal class ProjectCountChangedMessages(int count) : ValueChangedMessage<int>(count);
