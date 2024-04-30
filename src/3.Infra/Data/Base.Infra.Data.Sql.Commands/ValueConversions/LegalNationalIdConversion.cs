namespace Base.Infra.Data.Sql.Commands.ValueConversions;

public class LegalNationalIdConversion()
    : ValueConverter<LegalNationalId, string>(c => c.Value, c => LegalNationalId.FromString(c));