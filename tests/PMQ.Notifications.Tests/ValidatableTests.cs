using FluentValidation.Results;
using PMQ.Notifications;

namespace PMQ.Notifications.Tests;

public class ValidatableTests
{
    private sealed class Order : Validatable
    {
        public void Reject(string property, string message) => AddNotification(property, message);

        public void Reject(string message) => AddNotification(message);

        public void RejectMany(IEnumerable<ValidationFailure> failures) => AddNotifications(failures);
    }

    [Fact]
    public void NewInstance_ShouldBeValid()
    {
        var order = new Order();

        order.IsValid.ShouldBeTrue();
        order.IsInvalid.ShouldBeFalse();
        order.ValidationResult.Errors.ShouldBeEmpty();
    }

    [Fact]
    public void AddNotification_WithProperty_ShouldRecordPropertyAndMessage()
    {
        var order = new Order();

        order.Reject("Total", "Total must be positive.");

        order.IsInvalid.ShouldBeTrue();

        var failure = order.ValidationResult.Errors.ShouldHaveSingleItem();
        failure.PropertyName.ShouldBe("Total");
        failure.ErrorMessage.ShouldBe("Total must be positive.");
    }

    [Fact]
    public void AddNotification_WithoutProperty_ShouldLeavePropertyEmpty()
    {
        var order = new Order();

        order.Reject("Something went wrong.");

        order.ValidationResult.Errors.ShouldHaveSingleItem().PropertyName.ShouldBeEmpty();
    }

    [Fact]
    public void AddNotification_WithBlankMessage_ShouldBeIgnored()
    {
        var order = new Order();

        order.Reject("Total", "   ");

        order.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void AddNotification_WithDuplicate_ShouldRecordOnlyOnce()
    {
        var order = new Order();

        order.Reject("Total", "Total must be positive.");
        order.Reject("Total", "Total must be positive.");

        order.ValidationResult.Errors.Count.ShouldBe(1);
    }

    [Fact]
    public void AddNotification_WithSameMessageOnAnotherProperty_ShouldRecordBoth()
    {
        var order = new Order();

        order.Reject("Total", "Required.");
        order.Reject("Customer", "Required.");

        order.ValidationResult.Errors.Count.ShouldBe(2);
    }

    [Fact]
    public void AddNotifications_ShouldRecordEveryFailure()
    {
        var order = new Order();

        order.RejectMany([new ValidationFailure("A", "first"), new ValidationFailure("B", "second")]);

        order.ValidationResult.Errors.Count.ShouldBe(2);
    }

    [Fact]
    public void ClearValidation_ShouldMakeTheObjectValidAgain()
    {
        var order = new Order();
        order.Reject("Total", "Total must be positive.");

        order.ClearValidation();

        order.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void ObsoleteNamespaceShim_ShouldStillBeAValidatable()
    {
        // Garante que o alias depreciado continua intercambiável com o tipo real,
        // para que consumidores anteriores à correção do namespace sigam compilando.
        // O global:: é necessário porque, dentro deste namespace, `Notification` sozinho
        // resolve para o tipo PMQ.Notifications.Notification e não para o namespace.
#pragma warning disable CS0618 // Type or member is obsolete
        typeof(global::PMQ.Notification.Validatable).IsAssignableTo(typeof(Validatable)).ShouldBeTrue();
#pragma warning restore CS0618
    }
}
