using MaintenanceSystem.Application.Modules.Imports;
using MaintenanceSystem.Domain.Entities.Catalogs;
using MaintenanceSystem.Domain.Enums;
using MaintenanceSystem.Domain.Entities.Equipments;
using MaintenanceSystem.Domain.Entities.IDM;
using Microsoft.EntityFrameworkCore;

namespace MaintenanceSystem.Infrastructure.Imports.Importers
{
    internal sealed class EquipmentImporter : EntityImporter
    {
        public override ImportType Type => ImportType.Equipment;

        public override int Order => 8;

        public override IReadOnlyList<ImportType> DependsOn =>
            [ImportType.Plants, ImportType.Areas, ImportType.EquipmentTypes, ImportType.Procedures];

        public override IReadOnlyList<ImportColumn> Columns { get; } =
        [
            ImportColumn.Text("TAG", true, "Identificador único del equipo (máx. 50).", "SRV-J1-001"),
            ImportColumn.Text("NAME", true, "Nombre o descripción del equipo (máx. 150).", "Servidor de archivos"),
            ImportColumn.Text("EQUIPMENT_TYPE_CODE", true, "Código del tipo de equipo (debe existir).", "SRV"),
            ImportColumn.Text("BRAND", false, "Marca (máx. 100).", "Dell"),
            ImportColumn.Text("MODEL", false, "Modelo (máx. 100).", "PowerEdge R740"),
            ImportColumn.Text("SERIAL_NUMBER", false, "Número de serie (máx. 100).", "ABC1234"),
            ImportColumn.Text("SITE_CODE", true, "Código del sitio.", "JRZ"),
            ImportColumn.Text("PLANT_CODE", true, "Código de la planta (debe existir en el sitio).", "J1"),
            ImportColumn.Text("AREA_NAME", false, "Nombre del área dentro de la planta (debe existir).", "Producción"),
            ImportColumn.Text("LOCATION", false, "Ubicación física libre (máx. 200).", "Site de comunicaciones, rack 2"),
            ImportColumn.Text("RESPONSIBLE_USER", false, "Usuario de dominio del responsable (debe estar registrado en la app).", "JPEREZ"),
            ImportColumn.Text("PROCEDURE_CODE", false, "Código del procedimiento de mantenimiento (debe existir).", "PROC-SRV-01"),
            ImportColumn.Enum<MaintenanceFrequency>("FREQUENCY", false, "Frecuencia. Vacío = frecuencia por defecto del tipo de equipo.", nameof(MaintenanceFrequency.Quarterly)),
            ImportColumn.Enum<CriticalityLevel>("CRITICALITY", true, "Criticidad del equipo.", nameof(CriticalityLevel.High)),
            ImportColumn.Enum<EquipmentStatus>("STATUS", false, "Estatus. Vacío = Active.", nameof(EquipmentStatus.Active)),
            ImportColumn.Boolean("HAS_BACKUP", "Cuenta con respaldo/redundancia. Vacío = NO.", "NO"),
            ImportColumn.Text("NOTES", false, "Notas (máx. 1000).")
        ];

        public override async Task ProcessAsync(IReadOnlyList<ImportRow> rows, ImportContext context, CancellationToken cancellationToken)
        {
            var db = context.Db;
            var types = await db.EquipmentTypes.ToDictionaryAsync(x => x.Code, StringComparer.OrdinalIgnoreCase, cancellationToken);
            var plants = (await db.Plants.Include(x => x.Site).ToListAsync(cancellationToken))
                .ToDictionary(x => PlantKey(x.Site.Code, x.Code), StringComparer.OrdinalIgnoreCase);
            var areas = (await db.Areas.ToListAsync(cancellationToken))
                .ToDictionary(x => $"{x.PlantId}|{x.Name}", StringComparer.OrdinalIgnoreCase);
            var procedures = await db.Procedures.ToDictionaryAsync(x => x.Code, StringComparer.OrdinalIgnoreCase, cancellationToken);
            var accounts = await db.IDMAccounts.ToDictionaryAsync(x => x.DomainUserName, StringComparer.OrdinalIgnoreCase, cancellationToken);
            var existing = await db.Equipments.ToDictionaryAsync(x => x.Tag, StringComparer.OrdinalIgnoreCase, cancellationToken);
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var row in rows)
            {
                var tag = row.Code("TAG", 50, true);
                var name = row.Text("NAME", 150, true);
                var typeCode = row.Code("EQUIPMENT_TYPE_CODE", 20, true);
                var brand = row.Text("BRAND", 100);
                var model = row.Text("MODEL", 100);
                var serialNumber = row.Text("SERIAL_NUMBER", 100);
                var siteCode = row.Code("SITE_CODE", 20, true);
                var plantCode = row.Code("PLANT_CODE", 20, true);
                var areaName = row.Text("AREA_NAME", 100);
                var location = row.Text("LOCATION", 200);
                var responsibleUser = row.Code("RESPONSIBLE_USER", 50);
                var procedureCode = row.Code("PROCEDURE_CODE", 30);
                var frequency = row.Enum<MaintenanceFrequency>("FREQUENCY", false);
                var criticality = row.Enum<CriticalityLevel>("CRITICALITY", true);
                var status = row.Enum<EquipmentStatus>("STATUS", false) ?? EquipmentStatus.Active;
                var hasBackup = row.Bool("HAS_BACKUP", false);
                var notes = row.Text("NOTES", 1000);

                EquipmentType? type = null;
                if (typeCode is not null && !types.TryGetValue(typeCode, out type))
                    row.AddError($"EQUIPMENT_TYPE_CODE: el tipo de equipo '{typeCode}' no existe.");

                Plant? plant = null;
                if (siteCode is not null && plantCode is not null && !plants.TryGetValue(PlantKey(siteCode, plantCode), out plant))
                    row.AddError($"PLANT_CODE: la planta '{plantCode}' no existe en el sitio '{siteCode}'.");

                Area? area = null;
                if (plant is not null && areaName is not null && !areas.TryGetValue($"{plant.Id}|{areaName}", out area))
                    row.AddError($"AREA_NAME: el área '{areaName}' no existe en la planta '{plantCode}'.");

                Procedure? procedure = null;
                if (procedureCode is not null && !procedures.TryGetValue(procedureCode, out procedure))
                    row.AddError($"PROCEDURE_CODE: el procedimiento '{procedureCode}' no existe.");

                IDMAccount? responsible = null;
                if (responsibleUser is not null && !accounts.TryGetValue(responsibleUser, out responsible))
                    row.AddError($"RESPONSIBLE_USER: el usuario '{responsibleUser}' no está registrado.");

                frequency ??= type?.DefaultFrequency;
                if (type is not null && frequency is null && row.Raw("FREQUENCY") is null)
                    row.AddError($"FREQUENCY: es obligatorio porque el tipo '{typeCode}' no tiene frecuencia por defecto.");

                row.Key = tag ?? string.Empty;
                if (tag is not null) row.EnsureUnique(seen, tag);
                if (row.HasErrors) continue;

                var isNew = !existing.TryGetValue(tag!, out var equipment);
                equipment ??= new Equipment { Tag = tag! };
                equipment.Name = name!;
                equipment.EquipmentTypeId = type!.Id;
                equipment.Brand = brand;
                equipment.Model = model;
                equipment.SerialNumber = serialNumber;
                equipment.PlantId = plant!.Id;
                equipment.AreaId = area?.Id;
                equipment.Location = location;
                equipment.ResponsibleAccountId = responsible?.Id;
                equipment.ProcedureId = procedure?.Id;
                equipment.Frequency = frequency!.Value;
                equipment.Criticality = criticality!.Value;
                equipment.Status = status;
                equipment.HasBackup = hasBackup;
                equipment.Notes = notes;

                if (isNew) existing[tag!] = equipment;
                Track(context, row, equipment, isNew);
            }
        }
    }
}