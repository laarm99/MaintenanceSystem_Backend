using MaintenanceSystem.Application.Modules.Imports;
using MaintenanceSystem.Application.Modules.Imports.DTOs;

namespace MaintenanceSystem.Infrastructure.Imports
{
    internal sealed record ImportColumn(
        string Name,
        bool Required,
        ImportColumnKind Kind,
        string Description,
        string? Example = null,
        IReadOnlyList<string>? AllowedValues = null)
    {
        public static ImportColumn Text(string name, bool required, string description, string? example = null) =>
            new(name, required, ImportColumnKind.Text, description, example);

        public static ImportColumn Integer(string name, bool required, string description, string? example = null) =>
            new(name, required, ImportColumnKind.Integer, description, example);

        public static ImportColumn Date(string name, bool required, string description, string? example = null) =>
            new(name, required, ImportColumnKind.Date, description, example);

        public static ImportColumn Boolean(string name, string description, string? example = null) =>
            new(name, false, ImportColumnKind.Boolean, description, example, ImportRow.BooleanValues);

        public static ImportColumn Enum<TEnum>(string name, bool required, string description, string? example = null)
            where TEnum : struct, System.Enum =>
            new(name, required, ImportColumnKind.Enum, description, example, System.Enum.GetNames<TEnum>());

        public ImportColumnDTO ToDTO() => new()
        {
            Name = Name,
            Required = Required,
            Kind = Kind,
            Description = Description,
            Example = Example,
            AllowedValues = AllowedValues
        };
    }
}