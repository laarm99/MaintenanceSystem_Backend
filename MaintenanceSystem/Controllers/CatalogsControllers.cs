using MaintenanceSystem.Application.Modules.Catalogs.DTOs;
using MaintenanceSystem.Application.Modules.Common;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.Web.Controllers
{
    [Route("api/catalogs/sites")]
    public class SitesController(ICrudService<SiteDTO, SaveSiteRequest> service)
        : CrudControllerBase<SiteDTO, SaveSiteRequest>(service);

    [Route("api/catalogs/plants")]
    public class PlantsController(ICrudService<PlantDTO, SavePlantRequest> service)
        : CrudControllerBase<PlantDTO, SavePlantRequest>(service);

    [Route("api/catalogs/areas")]
    public class AreasController(ICrudService<AreaDTO, SaveAreaRequest> service)
        : CrudControllerBase<AreaDTO, SaveAreaRequest>(service);

    [Route("api/catalogs/equipment-types")]
    public class EquipmentTypesController(ICrudService<EquipmentTypeDTO, SaveEquipmentTypeRequest> service)
        : CrudControllerBase<EquipmentTypeDTO, SaveEquipmentTypeRequest>(service);

    [Route("api/catalogs/procedures")]
    public class ProceduresController(ICrudService<ProcedureDTO, SaveProcedureRequest> service)
        : CrudControllerBase<ProcedureDTO, SaveProcedureRequest>(service);

    [Route("api/catalogs/non-working-days")]
    public class NonWorkingDaysController(ICrudService<NonWorkingDayDTO, SaveNonWorkingDayRequest> service)
        : CrudControllerBase<NonWorkingDayDTO, SaveNonWorkingDayRequest>(service);
}