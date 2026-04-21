using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using StBurger.Application.Core.Enums;

namespace StBurger.Infrastructure.Persistence.Models;

public class AuditEntry(EntityEntry entry = default!)
{
    public EntityEntry Entry { get; } = entry;
    public string TableName { get; set; } = string.Empty;
    public Dictionary<string, object> KeyValues { get; } = [];
    public Dictionary<string, object> OldValues { get; } = [];
    public Dictionary<string, object> NewValues { get; } = [];
    public List<PropertyEntry> TemporaryProperties { get; } = [];
    public AuditType AuditType { get; set; } = AuditType.None;
    public List<string> ChangedColumns { get; } = [];
    public bool HasTemporaryProperties => TemporaryProperties.Any();

    public Audit ToAudit()
    {
        var audit = new Audit
        {
            Type = AuditType.ToString(),
            TableName = TableName,
            DateTime = DateTime.UtcNow,
            PrimaryKey = JsonConvert.SerializeObject(KeyValues),
            OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues),
            NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues),
            AffectedColumns = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns)
        };
        return audit;
    }
}