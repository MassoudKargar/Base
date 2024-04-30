namespace Base.Infra.Data.Sql.Commands.ValueConversions;

public class BusinessIdConversion() : ValueConverter<BusinessId, Guid>(c => c.Value, c => BusinessId.FromGuid(c));