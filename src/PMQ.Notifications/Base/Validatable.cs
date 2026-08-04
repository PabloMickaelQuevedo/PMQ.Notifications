using System.Text.Json.Serialization;
using FluentValidation.Results;

namespace PMQ.Notifications;

/// <summary>
/// Base abstract class that provides validation capabilities to derived classes.
/// </summary>
/// <remarks>
/// Accumulates rule violations instead of throwing, so that a broken business rule stays an
/// expected outcome rather than exceptional control flow. Callers check <see cref="IsValid"/>
/// and decide what to do with <see cref="ValidationResult"/>.
/// </remarks>
public abstract class Validatable
{
    /// <summary>
    /// Gets the validation result containing any validation errors.
    /// </summary>
    [JsonIgnore]
    public ValidationResult ValidationResult { get; } = new();

    /// <summary>
    /// Gets a value indicating whether the object is valid.
    /// </summary>
    [JsonIgnore]
    public bool IsValid => ValidationResult.IsValid;

    /// <summary>
    /// Gets a value indicating whether the object is invalid.
    /// </summary>
    [JsonIgnore]
    public bool IsInvalid => !IsValid;

    /// <summary>
    /// Clears all validation errors.
    /// </summary>
    public void ClearValidation() => ValidationResult.Errors.Clear();

    /// <summary>
    /// Adds a collection of validation failures.
    /// </summary>
    /// <param name="failures">The collection of validation failures to add.</param>
    protected void AddNotifications(IEnumerable<ValidationFailure> failures)
    {
        foreach (var failure in failures)
            AddNotification(failure);
    }

    /// <summary>
    /// Adds a validation notification without a specific property.
    /// </summary>
    /// <param name="message">The validation error message.</param>
    protected void AddNotification(string message)
        => AddNotification(string.Empty, message);

    /// <summary>
    /// Adds a validation notification for a specific property.
    /// </summary>
    /// <param name="property">The property name.</param>
    /// <param name="message">The validation error message.</param>
    protected void AddNotification(string property, string message)
        => AddNotification(new ValidationFailure(property, message));

    /// <summary>
    /// Adds a validation failure if it doesn't already exist.
    /// </summary>
    /// <param name="failure">The validation failure to add.</param>
    private void AddNotification(ValidationFailure failure)
    {
        if (string.IsNullOrWhiteSpace(failure.ErrorMessage))
            return;

        if (ValidationResult.Errors.Any(e =>
            e.PropertyName == failure.PropertyName &&
            e.ErrorMessage == failure.ErrorMessage))
            return;

        ValidationResult.Errors.Add(failure);
    }
}
