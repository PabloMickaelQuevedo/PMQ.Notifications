namespace PMQ.Notifications;

/// <summary>
/// Represents a strongly-typed notification category.
/// </summary>
public sealed class NotificationType : IEquatable<NotificationType>
{
    /// <summary>
    /// Represents a validation notification type.
    /// </summary>
    public static readonly NotificationType Validation = new("VALIDATION");

    /// <summary>
    /// Represents a business rule notification type.
    /// </summary>
    public static readonly NotificationType BusinessRule = new("BUSINESSRULE");

    /// <summary>
    /// Represents a not found notification type.
    /// </summary>
    public static readonly NotificationType NotFound = new("NOTFOUND");

    /// <summary>
    /// Represents an inconsistent state notification type.
    /// </summary>
    public static readonly NotificationType InconsistentState = new("INCONSISTENTSTATE");

    /// <summary>
    /// Represents an access denied notification type.
    /// </summary>
    public static readonly NotificationType AccessDenied = new("ACCESSDENIED");

    /// <summary>
    /// Represents a value for notification types.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationType"/> class.
    /// </summary>
    /// <param name="value">The string value representing the notification type.</param>
    /// <exception cref="ArgumentException">Thrown if value is null or whitespace.</exception>
    private NotificationType(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Notification type cannot be null or whitespace.", nameof(value));

        Value = value.Trim().ToUpperInvariant();
    }

    /// <summary>
    /// Creates a custom notification type.
    /// </summary>
    /// <param name="value">The string value for the custom notification type.</param>
    /// <returns>A new <see cref="NotificationType"/> instance.</returns>
    public static NotificationType Custom(string value) => new(value);

    /// <summary>
    /// Returns the string representation of the notification type.
    /// </summary>
    /// <returns>The string value of the notification type.</returns>
    public override string ToString() => Value;

    /// <summary>
    /// Determines whether the current notification type is equal to another.
    /// </summary>
    /// <param name="other">The other notification type to compare.</param>
    /// <returns>True if equal; otherwise, false.</returns>
    public bool Equals(NotificationType? other)
        => other is not null && Value == other.Value;

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="NotificationType"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current notification type.</param>
    /// <returns>True if the specified object is equal to the current notification type; otherwise, false.</returns>
    public override bool Equals(object? obj)
        => obj is NotificationType other && Equals(other);

    /// <summary>
    /// Returns a hash code for the current <see cref="NotificationType"/>.
    /// </summary>
    /// <returns>A hash code for the current notification type.</returns>
    public override int GetHashCode()
        => Value.GetHashCode(StringComparison.Ordinal);

    /// <summary>
    /// Checks if two <see cref="NotificationType"/> instances are equal.
    /// </summary>
    public static bool operator ==(NotificationType? a, NotificationType? b)
        => a?.Equals(b) ?? b is null;

    /// <summary>
    /// Checks if two <see cref="NotificationType"/> instances are not equal.
    /// </summary>
    public static bool operator !=(NotificationType? a, NotificationType? b)
        => !(a == b);
}
