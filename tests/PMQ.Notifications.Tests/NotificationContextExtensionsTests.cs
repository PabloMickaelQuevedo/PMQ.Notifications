using PMQ.Notifications;

namespace PMQ.Notifications.Tests;

public class NotificationContextExtensionsTests
{
    private sealed class Order : Validatable
    {
        public void Reject(string property, string message) => AddNotification(property, message);
    }

    private readonly NotificationContext _context = new();

    [Fact]
    public void AddFrom_WithValidObject_ShouldAddNothingAndReturnFalse()
    {
        var added = _context.AddFrom(new Order());

        added.ShouldBeFalse();
        _context.HasNotifications.ShouldBeFalse();
    }

    [Fact]
    public void AddFrom_WithInvalidObject_ShouldCopyPropertyAsKeyAndMessage()
    {
        var order = new Order();
        order.Reject("Total", "Total must be positive.");

        var added = _context.AddFrom(order);

        added.ShouldBeTrue();

        var notification = _context.Notifications.ShouldHaveSingleItem();
        notification.Key.ShouldBe("Total");
        notification.Message.ShouldBe("Total must be positive.");
    }

    [Fact]
    public void AddFrom_ByDefault_ShouldUseValidationType()
    {
        var order = new Order();
        order.Reject("Total", "Total must be positive.");

        _context.AddFrom(order);

        _context.HasType(NotificationType.Validation).ShouldBeTrue();
    }

    [Fact]
    public void AddFrom_WithExplicitType_ShouldUseIt()
    {
        var order = new Order();
        order.Reject("Total", "Total must be positive.");

        _context.AddFrom(order, NotificationType.BusinessRule);

        _context.HasType(NotificationType.BusinessRule).ShouldBeTrue();
    }

    [Fact]
    public void AddFrom_WithCollection_ShouldAggregateEveryFailure()
    {
        var first = new Order();
        first.Reject("Total", "Total must be positive.");

        var second = new Order();
        second.Reject("Customer", "Customer is required.");

        var added = _context.AddFrom([first, second, new Order()]);

        added.ShouldBeTrue();
        _context.Notifications.Count.ShouldBe(2);
    }

    [Fact]
    public void AddFrom_WithCollectionOfValidObjects_ShouldReturnFalse()
    {
        var added = _context.AddFrom([new Order(), new Order()]);

        added.ShouldBeFalse();
        _context.HasNotifications.ShouldBeFalse();
    }

    [Fact]
    public void AddFrom_WithNullContext_ShouldThrow()
    {
        INotificationContext context = null!;

        Should.Throw<ArgumentNullException>(() => context.AddFrom(new Order()));
    }

    [Fact]
    public void AddFrom_WithNullValidatable_ShouldThrow()
    {
        Should.Throw<ArgumentNullException>(() => _context.AddFrom((Validatable)null!));
    }
}
