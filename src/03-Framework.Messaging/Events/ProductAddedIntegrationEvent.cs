namespace App.Framework.Messaging.Events;

public sealed record ProductAddedIntegrationEvent(long ProductId, string Name, List<string> Category, string Description, string ImageFile, decimal Price): IntegrationEvent;

