namespace Base.Infra.Data.Sql.Commands.ValueConversions;

public class NationalCodeConversion()
    : ValueConverter<NationalCode, string>(c => c.Value, c => NationalCode.FromString(c));