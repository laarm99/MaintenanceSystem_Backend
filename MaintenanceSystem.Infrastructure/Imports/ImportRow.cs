using MaintenanceSystem.Application.Modules.Imports;
using MaintenanceSystem.Application.Modules.Imports.DTOs;
using System.Globalization;

namespace MaintenanceSystem.Infrastructure.Imports
{
    /// <summary>
    /// One data row of the uploaded sheet. Parsing helpers collect validation errors instead of throwing.
    /// </summary>
    internal sealed class ImportRow
    {
        public static readonly IReadOnlyList<string> BooleanValues = ["SI", "NO"];

        private static readonly string[] DateFormats = ["yyyy-MM-dd", "dd/MM/yyyy", "d/M/yyyy", "yyyy/MM/dd"];
        private static readonly HashSet<string> TrueValues = new(StringComparer.OrdinalIgnoreCase) { "SI", "SÍ", "S", "YES", "Y", "TRUE", "VERDADERO", "1" };
        private static readonly HashSet<string> FalseValues = new(StringComparer.OrdinalIgnoreCase) { "NO", "N", "FALSE", "FALSO", "0" };

        private readonly IReadOnlyDictionary<string, string?> _values;

        public ImportRow(int rowNumber, IReadOnlyDictionary<string, string?> values)
        {
            RowNumber = rowNumber;
            _values = values;
        }

        public int RowNumber { get; }

        public string Key { get; set; } = string.Empty;

        public ImportRowAction Action { get; set; } = ImportRowAction.Create;

        public List<string> Errors { get; } = [];

        public bool HasErrors => Errors.Count > 0;

        public void AddError(string message) => Errors.Add(message);

        public string? Raw(string column) =>
            _values.TryGetValue(column, out var value) ? value : null;

        public string? Text(string column, int maxLength, bool required = false)
        {
            var value = Raw(column);

            if (value is null)
            {
                if (required) AddError($"{column}: es obligatorio.");
                return null;
            }

            if (value.Length > maxLength)
            {
                AddError($"{column}: excede {maxLength} caracteres.");
                return null;
            }

            return value;
        }

        public string? Code(string column, int maxLength, bool required = false) =>
            Text(column, maxLength, required)?.ToUpperInvariant();

        public bool Bool(string column, bool defaultValue)
        {
            var value = Raw(column);

            if (value is null) return defaultValue;
            if (TrueValues.Contains(value)) return true;
            if (FalseValues.Contains(value)) return false;

            AddError($"{column}: '{value}' no es válido, use SI o NO.");
            return defaultValue;
        }

        public int? Int(string column, bool required, int min = 0)
        {
            var value = Raw(column);

            if (value is null)
            {
                if (required) AddError($"{column}: es obligatorio.");
                return null;
            }

            if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var number)
                || number != decimal.Truncate(number)
                || number < min
                || number > int.MaxValue)
            {
                AddError($"{column}: '{value}' debe ser un número entero mayor o igual a {min}.");
                return null;
            }

            return (int)number;
        }

        public DateOnly? Date(string column, bool required)
        {
            var value = Raw(column);

            if (value is null)
            {
                if (required) AddError($"{column}: es obligatorio.");
                return null;
            }

            if (DateOnly.TryParseExact(value, DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                return date;

            AddError($"{column}: '{value}' no es una fecha válida (use AAAA-MM-DD).");
            return null;
        }

        public TEnum? Enum<TEnum>(string column, bool required) where TEnum : struct, System.Enum
        {
            var value = Raw(column);

            if (value is null)
            {
                if (required) AddError($"{column}: es obligatorio.");
                return null;
            }

            // Reject numeric strings so only the names stored in the DB are accepted.
            if (!int.TryParse(value, out _)
                && System.Enum.TryParse<TEnum>(value, true, out var result)
                && System.Enum.IsDefined(result))
                return result;

            AddError($"{column}: '{value}' no es válido. Valores permitidos: {string.Join(", ", System.Enum.GetNames<TEnum>())}.");
            return null;
        }

        public void EnsureUnique(HashSet<string> seenKeys, string key)
        {
            if (!seenKeys.Add(key))
                AddError($"Registro duplicado en el archivo ({key}).");
        }

        public ImportRowResultDTO ToDTO() => new()
        {
            RowNumber = RowNumber,
            Key = Key,
            Action = HasErrors ? ImportRowAction.Error : Action,
            Errors = Errors
        };
    }
}