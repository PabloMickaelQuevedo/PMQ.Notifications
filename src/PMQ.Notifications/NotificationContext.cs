namespace PMQ.Notifications;

/// <summary>
/// Represents a context for managing notifications.
/// </summary>
public class NotificationContext : INotificationContext
{
    /// <summary>
    /// Represents a collection of notifications.
    /// </summary>
    private readonly List<Notification> _notifications = [];

    /// <summary>
    /// Gets the read-only collection of notifications.
    /// </summary>
    public IReadOnlyCollection<Notification> Notifications => _notifications;

    /// <summary>
    /// Gets a value indicating whether there are any notifications.
    /// </summary>
    public bool HasNotifications => _notifications.Count != 0;

    /// <summary>
    /// Adds a notification to the context.
    /// </summary>
    /// <param name="notification">The notification to add.</param>
    public void Add(Notification notification)
    {
        _notifications.Add(notification);
    }

    /// <summary>
    /// Adds a notification with a message and optional type.
    /// </summary>
    /// <param name="message">The notification message.</param>
    /// <param name="type">The notification type (optional).</param>
    public void Add(string message, NotificationType? type = null)
    {
        _notifications.Add(new Notification(message, type));
    }

    /// <summary>
    /// Adds a notification with a key, message, and optional type.
    /// </summary>
    /// <param name="key">The notification key.</param>
    /// <param name="message">The notification message.</param>
    /// <param name="type">The notification type (optional).</param>
    public void Add(string key, string message, NotificationType? type = null)
    {
        _notifications.Add(new Notification(key, message, type));
    }

    /// <summary>
    /// Adds a range of notifications to the context.
    /// </summary>
    /// <param name="notifications">The notifications to add.</param>
    public void AddRange(IEnumerable<Notification> notifications)
    {
        _notifications.AddRange(notifications);
    }

    /// <summary>
    /// Clears all notifications from the context.
    /// </summary>
    public void Clear()
    {
        _notifications.Clear();
    }

    /// <summary>
    /// Gets all notifications of a specific type.
    /// </summary>
    /// <param name="type">The notification type to filter by.</param>
    /// <returns>Notifications matching the specified type.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="type"/> is null.</exception>
    public IEnumerable<Notification> GetByType(NotificationType type)
    {
        if (type is null)
            throw new ArgumentNullException(nameof(type), "Notification type cannot be null.");

        return _notifications.Where(n => n.Type != null && n.Type.Equals(type));
    }

    /// <summary>
    /// Determines whether there are any notifications of a specific type.
    /// </summary>
    /// <param name="type">The notification type to check for.</param>
    /// <returns>True if any notifications of the specified type exist; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="type"/> is null.</exception>
    public bool HasType(NotificationType type)
    {
        if (type is null)
            throw new ArgumentNullException(nameof(type), "Notification type cannot be null.");

        return _notifications.Any(n => n.Type != null && n.Type.Equals(type));
    }

    /// <summary>
    /// Gets all notification messages, optionally filtered by type.
    /// </summary>
    /// <param name="type">The notification type to filter by (optional).</param>
    /// <returns>Notification messages.</returns>
    public IEnumerable<string> GetMessages(NotificationType? type = null)
    {
        return type is null
            ? _notifications.Select(n => n.Message)
            : _notifications.Where(n => n.Type == type).Select(n => n.Message);
    }
}
