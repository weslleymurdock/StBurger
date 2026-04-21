namespace StBurger.Domain.Core.Abstractions;

public interface IAuditableEntity<TId> : IAuditableEntity, IEntity<TId>
{
}

public interface IAuditableEntity : IEntity
{
    DateTime CreatedOn { get; set; }

    DateTime? LastModifiedOn { get; set; }
}
