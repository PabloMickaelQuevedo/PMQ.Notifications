/// <summary>
/// Represents a notification containing a key, message, and an optional type for categorization.
/// </summary>
namespace PMQ.Notifications;

public class Notification
{
    public string Key { get; }
    public string Message { get; }
    public NotificationType? Type { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Notification"/> class with a key, message, and optional type.
    /// </summary>
    /// <param name="key">The notification key.</param>
    /// <param name="message">The notification message.</param>
    /// <param name="type">The notification type (optional).</param>
    public Notification(string key, string message, NotificationType? type = null)
    {
        Validate(key, message);

        Key = key.Trim();
        Message = message.Trim();
        Type = type;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Notification"/> class with a message and optional type.
    /// </summary>
    /// <param name="message">The notification message.</param>
    /// <param name="type">The notification type (optional).</param>
    public Notification(string message, NotificationType? type = null)
       : this(string.Empty, message, type)
    {
    }

    /// <summary>
    /// Validates the key and message for a notification.
    /// </summary>
    /// <param name="key">The notification key.</param>
    /// <param name="message">The notification message.</param>
    /// <exception cref="ArgumentException">Thrown if key is null or message is null or whitespace.</exception>
    public static void Validate(string key, string message)
    {
        if (key is null)
            throw new ArgumentException("Key cannot be null.", nameof(key));

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be null or whitespace.", nameof(message));
    }

    /// <summary>
    /// Returns a string representation of the notification.
    /// </summary>
    /// <returns>A string describing the notification.</returns>
    public override string ToString()
    {
        var key = string.IsNullOrWhiteSpace(Key) ? null : Key.Trim();
        var type = Type?.ToString();

        if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(key))
            return $"{type} [{key}]: {Message}";

        if (!string.IsNullOrEmpty(type))
            return $"{type}: {Message}";

        if (!string.IsNullOrEmpty(key))
            return $"[{key}]: {Message}";

        return Message;
    }
}
