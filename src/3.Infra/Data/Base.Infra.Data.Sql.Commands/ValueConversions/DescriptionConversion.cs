namespace Base.Infra.Data.Sql.Commands.ValueConversions;

public class DescriptionConversion() : ValueConverter<Description, string>(c => c.Value, c => Description.FromString(c));