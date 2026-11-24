using MaintenanceSystem.Application.Modules.Catalogs.DTOs;
using MaintenanceSystem.Application.Modules.Common;
using MaintenanceSystem.Application.Modules.Equipments.DTOs;
using MaintenanceSystem.Application.Modules.Imports;
using MaintenanceSystem.Domain.Constants;
using MaintenanceSystem.Infrastructure.Crud.Services;
using MaintenanceSystem.Infrastructure.Imports;
using MaintenanceSystem.Infrastructure.Imports.Importers;
using MaintenanceSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MaintenanceSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ProjectDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sql => sql.MigrationsHistoryTable(DbTables.MigrationsHistory)));

            services.AddCrudServices();
            services.AddImports();

            return services;
        }

        private static IServiceCollection AddCrudServices(this IServiceCollection services)
        {
            services.AddScoped<ICrudService<SiteDTO, SaveSiteRequest>, SiteService>();
            services.AddScoped<ICrudService<PlantDTO, SavePlantRequest>, PlantService>();
            services.AddScoped<ICrudService<AreaDTO, SaveAreaRequest>, AreaService>();
            services.AddScoped<ICrudService<EquipmentTypeDTO, SaveEquipmentTypeRequest>, EquipmentTypeService>();
            services.AddScoped<ICrudService<ProcedureDTO, SaveProcedureRequest>, ProcedureService>();
            services.AddScoped<ICrudService<NonWorkingDayDTO, SaveNonWorkingDayRequest>, NonWorkingDayService>();
            services.AddScoped<ICrudService<EquipmentDTO, SaveEquipmentRequest>, EquipmentService>();
            services.AddScoped<ILookupService, LookupService>();

            return services;
        }

        private static IServiceCollection AddImports(this IServiceCollection services)
        {
            services.AddScoped<IEntityImporter, SitesImporter>();
            services.AddScoped<IEntityImporter, PlantsImporter>();
            services.AddScoped<IEntityImporter, AreasImporter>();
            services.AddScoped<IEntityImporter, EquipmentTypesImporter>();
            services.AddScoped<IEntityImporter, ProceduresImporter>();
            services.AddScoped<IEntityImporter, ProcedureChecklistItemsImporter>();
            services.AddScoped<IEntityImporter, NonWorkingDaysImporter>();
            services.AddScoped<IEntityImporter, EquipmentImporter>();
            services.AddScoped<IImportService, ImportService>();

            return services;
        }
    }
}