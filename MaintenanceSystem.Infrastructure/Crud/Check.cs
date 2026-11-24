namespace MaintenanceSystem.Infrastructure.Crud
{
    /// <summary>
    /// Small validation/normalization helpers with Spanish messages.
    /// </summary>
    internal static class Check
    {
        public static string? Clean(string? value) =>
            string.IsNullOrWhiteSpace(value) ? null : value.Trim();

        public static string? Code(string? value) => Clean(value)?.ToUpperInvariant();

        public static void Text(List<string> errors, string? value, string label, int maxLength, bool required = false)
        {
            if (value is null)
            {
                if (required) errors.Add($"{label} es obligatorio.");
                return;
            }

            if (value.Length > maxLength)
                errors.Add($"{label} admite máximo {maxLength} caracteres.");
        }

        public static void Required<T>(List<string> errors, T? value, string label) where T : struct
        {
            if (value is null) errors.Add($"{label} es obligatorio.");
        }
    }
}