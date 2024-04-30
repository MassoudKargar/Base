namespace Base.Infra.Data.Sql.Commands.ValueConversions;

public class TitleConversion() : ValueConverter<Title, string>(c => c.Value, c => Title.FromString(c));