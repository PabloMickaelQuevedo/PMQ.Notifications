# PMQ.Notifications

A package for managing notifications and validations in .NET applications, making it easy to control messages, errors, and business rules in a centralized and strongly-typed way.

## Installation

You can install via NuGet:

```shell
dotnet add package PMQ.Notifications
```

Or manually add the reference in your `.csproj`.

## How it works

The package provides a notification context (`NotificationContext`) to register, query, and handle validation messages, business rules, access errors, and more, using strongly-typed notification types (`NotificationType`).

### Main components

- `NotificationContext`: Manages the list of notifications.
- `Notification`: Represents a single notification (with key, message, and optional type).
- `NotificationType`: Enum-like value object for typed categorization of messages.

## Usage Examples

### Adding notifications

```csharp
var context = new NotificationContext();

// Add a simple notification
context.Add("Required field not provided.");

// Add with type
context.Add("Invalid email.", NotificationType.Validation);

// Add with custom key
context.Add("Email", "Email already registered.", NotificationType.BusinessRule);

// Add multiple notifications
context.AddRange(new[] {
    new Notification("Weak password.", NotificationType.Validation),
    new Notification("User not found.", NotificationType.NotFound)
});
```

### Querying notifications

```csharp
// All notifications
var all = context.Notifications;

// Check if any notification exists
if (context.HasNotifications)
{
    // handle logic
}

// Filter by type
var validations = context.GetByType(NotificationType.Validation);

// Check for a specific type
if (context.HasType(NotificationType.NotFound))
{
    // custom handling
}

// Get only the messages
var messages = context.GetMessages();
var validationMessages = context.GetMessages(NotificationType.Validation);
```

### Clearing notifications

```csharp
context.Clear();
```

## Usage in Filters and status code manipulation

`NotificationContext` can be injected and used in filters (e.g., `IActionFilter` or `IExceptionFilter`) to customize HTTP responses based on the notification type.

```csharp
public class NotificationFilter : IActionFilter
{
    private readonly NotificationContext _context;

    public NotificationFilter(NotificationContext context) => _context = context;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (_context.HasType(NotificationType.Validation))
        {
            context.Result = new BadRequestObjectResult(_context.GetMessages(NotificationType.Validation));
        }
        else if (_context.HasType(NotificationType.NotFound))
        {
            context.Result = new NotFoundObjectResult(_context.GetMessages(NotificationType.NotFound));
        }
        else if (_context.HasNotifications)
        {
            context.Result = new UnprocessableEntityObjectResult(_context.GetMessages());
        }
    }
}
```

## Handling responses with NotificationType

Use `NotificationType` to centralize and standardize business logic across your services, application, and API layers.

It enables better separation of concerns, allows easier unit testing, and makes your response logic explicit and reusable.

## Documentation

For more details, explore the source code and XML comments. Everything is written in a clean and extensible way, ready for your use and contribution.

MIT License © 2025 Pablo Mickael Quevedo
