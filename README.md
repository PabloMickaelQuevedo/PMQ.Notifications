# PMQ.Notifications

A package for managing notifications and business rules in .NET applications, making it easy to handle messages, errors, and domain rules in a centralized and strongly-typed way.  
Designed to fit **CQRS**, **MediatR**, and **DDD** patterns, enabling clear separation of concerns.

[![NuGet](https://img.shields.io/nuget/v/PMQ.Notifications.svg)](https://www.nuget.org/packages/PMQ.Notifications/)  
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)

---

## Installation

Install via NuGet:

```shell
dotnet add package PMQ.Notifications
```

Or manually add the reference in your `.csproj`.

---

## How it works

The package provides a notification context (`INotificationContext`) to register, query, and handle business rule violations, not found errors, or any kind of custom notification, using strongly-typed notification types (`NotificationType`).

### Main components

- `INotificationContext`: Abstraction for managing notifications.
- `NotificationContext`: Default implementation of `INotificationContext`.
- `Notification`: Represents a single notification (key, message, type).
- `NotificationType`: Enum-like value object for message categorization.

---

## Usage Examples

### Registering the service

```csharp
builder.Services.AddScoped<INotificationContext, NotificationContext>();
```

---

### Command + Handler with Mediator (business rule example)

In this scenario we want to add a product to a category.  
- If the category does not exist → a notification is added.  
- If the product already exists in the category → a business rule notification is added.  

```csharp
using MediatR;
using PMQ.Notifications;

public record AddProductCommand(string CategoryId, string ProductName) : IRequest<bool>;

public class AddProductCommandHandler : IRequestHandler<AddProductCommand, bool>
{
    private readonly INotificationContext _notifications;
    private readonly ICategoryRepository _categoryRepository;

    public AddProductCommandHandler(
        INotificationContext notifications,
        ICategoryRepository categoryRepository)
    {
        _notifications = notifications;
        _categoryRepository = categoryRepository;
    }

    public async Task<bool> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            _notifications.Add("Category", $"Category with id '{request.CategoryId}' was not found.", NotificationType.NotFound);
            return false;
        }

        if (category.Products.Any(p => p.Name.Equals(request.ProductName, StringComparison.OrdinalIgnoreCase)))
        {
            _notifications.Add("Product", $"The product '{request.ProductName}' already exists in this category.", NotificationType.BusinessRule);
            return false;
        }

        //Business rule passed → add product
        category.AddProduct(request.ProductName);

        await _categoryRepository.UpdateAsync(category, cancellationToken);

        return true;
    }
}
```

---

### Querying notifications in application layer

```csharp
if (_notifications.HasType(NotificationType.NotFound))
{
    var notFoundMessages = _notifications.GetMessages(NotificationType.NotFound);
    // handle not found logic
}

if (_notifications.HasType(NotificationType.BusinessRule))
{
    var businessRuleErrors = _notifications.GetMessages(NotificationType.BusinessRule);
    // handle business rule violation
}

if (_notifications.HasNotifications)
{
    var allMessages = _notifications.GetMessages();
    // handle all notifications
}
```

---

### Clearing notifications

```csharp
_notifications.Clear();
```

---

## Usage in Filters (with `ResultExecutingContext`)

In API projects, you can use a filter to automatically map notifications into HTTP responses.  

```csharp
public class NotificationFilter : IResultFilter
{
    private readonly INotificationContext _context;

    public NotificationFilter(INotificationContext context) => _context = context;

    public void OnResultExecuting(ResultExecutingContext context)
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

    public void OnResultExecuted(ResultExecutedContext context) { }
}
```

Register globally:

```csharp
builder.Services.AddControllers(options =>
{
    options.Filters.Add<NotificationFilter>();
});
```

---

## Benefits of using NotificationContext

- **Separation of concerns**: business rules and not-found checks are decoupled from controllers and services.  
- **Consistency**: all rule violations and messages are centralized.  
- **Testability**: easy to assert domain notifications in unit tests.  
- **Extensibility**: custom notification types can be added (e.g., `Security`, `AccessDenied`).  

---

## Keywords for Discoverability

These keywords help developers and AI tools to find this package on NuGet and GitHub:  

`notifications, validation, business-rules, ddd, domain-driven-design, cqrs, mediator, mediatR, clean-architecture, error-handling, result-pattern, notification-pattern, application-layer, domain-events`

---

MIT License © 2025 Pablo Mickael Quevedo
