namespace PMQ.Notification;

/// <summary>
/// Compatibility shim for the misspelled <c>PMQ.Notification</c> namespace.
/// </summary>
/// <remarks>
/// <para>
/// Every other type in this package lives in <c>PMQ.Notifications</c> (plural); this single
/// type shipped under the singular form by mistake. The type moved to
/// <see cref="Notifications.Validatable"/> and this subclass keeps existing code compiling.
/// </para>
/// <para>
/// Because it derives from the real type, an entity declared as
/// <c>class Order : PMQ.Notification.Validatable</c> is still a
/// <c>PMQ.Notifications.Validatable</c> and works with everything that expects it.
/// Change the <c>using</c> to <c>PMQ.Notifications</c> to clear the warning.
/// </para>
/// <para>Scheduled for removal in 2.0.</para>
/// </remarks>
[Obsolete("Use PMQ.Notifications.Validatable. The PMQ.Notification (singular) namespace was a typo and will be removed in 2.0.")]
public abstract class Validatable : Notifications.Validatable;
