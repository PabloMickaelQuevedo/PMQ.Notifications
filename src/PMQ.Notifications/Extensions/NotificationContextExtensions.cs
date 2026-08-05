namespace PMQ.Notifications;

/// <summary>
/// Bridges the validation state accumulated by a <see cref="Validatable"/> into an
/// <see cref="INotificationContext"/>.
/// </summary>
/// <remarks>
/// Lets a domain object stay unaware of the request it happens to be serving: it accumulates
/// its own rule violations, and the caller decides whether and how to promote them.
/// </remarks>
public static class NotificationContextExtensions
{
    /// <summary>
    /// Copies the validation failures of <paramref name="validatable"/> into the context.
    /// </summary>
    /// <param name="notificationContext">The notification context to add to.</param>
    /// <param name="validatable">The validated object.</param>
    /// <param name="type">
    /// Category applied to the generated notifications. Defaults to
    /// <see cref="NotificationType.Validation"/>. Pass <see cref="NotificationType.BusinessRule"/>
    /// when the failures come from a domain invariant rather than from malformed input.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when there were failures — and therefore the operation should stop.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="notificationContext"/> or <paramref name="validatable"/> is null.
    /// </exception>
    public static bool AddFrom(
        this INotificationContext notificationContext,
        Validatable validatable,
        NotificationType? type = null)
    {
        ArgumentNullException.ThrowIfNull(notificationContext);
        ArgumentNullException.ThrowIfNull(validatable);

        if (validatable.IsValid)
            return false;

        foreach (var failure in validatable.ValidationResult.Errors)
        {
            notificationContext.Add(
                failure.PropertyName ?? string.Empty,
                failure.ErrorMessage,
                type ?? NotificationType.Validation);
        }

        return true;
    }

    /// <summary>
    /// Copies the validation failures of every object in <paramref name="validatables"/> into the context.
    /// </summary>
    /// <param name="notificationContext">The notification context to add to.</param>
    /// <param name="validatables">The validated objects.</param>
    /// <param name="type">Category applied to the generated notifications.</param>
    /// <returns><see langword="true"/> when at least one object had failures.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="notificationContext"/> or <paramref name="validatables"/> is null.
    /// </exception>
    public static bool AddFrom(
        this INotificationContext notificationContext,
        IEnumerable<Validatable> validatables,
        NotificationType? type = null)
    {
        ArgumentNullException.ThrowIfNull(notificationContext);
        ArgumentNullException.ThrowIfNull(validatables);

        var hasFailures = false;

        foreach (var validatable in validatables)
            hasFailures |= notificationContext.AddFrom(validatable, type);

        return hasFailures;
    }
}
