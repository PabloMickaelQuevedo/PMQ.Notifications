namespace PMQ.Notifications;

/// <summary>
/// Defines a contract for managing notifications within a context.
/// </summary>
public interface INotificationContext
{
    /// <summary>
    /// Gets the read-only collection of notifications.
    /// </summary>
    IReadOnlyCollection<Notification> Notifications { get; }

    /// <summary>
    /// Gets a value indicating whether there are any notifications.
    /// </summary>
    bool HasNotifications { get; }

    /// <summary>
    /// Adds a notification to the context.
    /// </summary>
    /// <param name="notification">The notification to add.</param>
    void Add(Notification notification);

    /// <summary>
    /// Adds a notification with a message and optional type.
    /// </summary>
    /// <param name="message">The notification message.</param>
    /// <param name="type">The notification type (optional).</param>
    void Add(string message, NotificationType? type = null);

    /// <summary>
    /// Adds a notification with a key, message, and optional type.
    /// </summary>
    /// <param name="key">The notification key.</param>
    /// <param name="message">The notification message.</param>
    /// <param name="type">The notification type (optional).</param>
    void Add(string key, string message, NotificationType? type = null);

    /// <summary>
    /// Adds a range of notifications to the context.
    /// </summary>
    /// <param name="notifications">The notifications to add.</param>
    void AddRange(IEnumerable<Notification> notifications);

    /// <summary>
    /// Determines whether there are any notifications of a specific type.
    /// </summary>
    /// <param name="type">The notification type to check for.</param>
    /// <returns>True if any notifications of the specified type exist; otherwise, false.</returns>
    bool HasType(NotificationType type);

    /// <summary>
    /// Gets all notifications of a specific type.
    /// </summary>
    /// <param name="type">The notification type to filter by.</param>
    /// <returns>Notifications matching the specified type.</returns>
    IEnumerable<Notification> GetByType(NotificationType type);

    /// <summary>
    /// Gets all notification messages, optionally filtered by type.
    /// </summary>
    /// <param name="type">The notification type to filter by (optional).</param>
    /// <returns>Notification messages.</returns>
    IEnumerable<string> GetMessages(NotificationType? type = null);

    /// <summary>
    /// Clears all notifications from the context.
    /// </summary>
    void Clear();
}
