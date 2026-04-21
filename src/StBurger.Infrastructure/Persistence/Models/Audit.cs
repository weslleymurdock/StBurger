using Microsoft.EntityFrameworkCore.ChangeTracking;
using StBurger.Domain.Core.Abstractions;

namespace StBurger.Infrastructure.Persistence.Models;

public class Audit : IEntity<string>
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Type { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public DateTime DateTime { get; set; } = DateTime.UtcNow;
    public string OldValues { get; set; } = string.Empty;
    public string NewValues { get; set; } = string.Empty;
    public string AffectedColumns { get; set; } = string.Empty;
    public string PrimaryKey { get; set; } = string.Empty;
}
