using StBurger.Domain.Core.Abstractions;

namespace StBurger.Domain.Core.Entities;

public abstract class AuditableEntity<TId> : IAuditableEntity<TId> where TId : class, IEquatable<TId>
{
    public TId Id { get; set; } = default!;
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}