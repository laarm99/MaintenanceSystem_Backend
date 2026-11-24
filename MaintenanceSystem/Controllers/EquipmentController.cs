using MaintenanceSystem.Application.Modules.Common;
using MaintenanceSystem.Application.Modules.Equipments.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.Web.Controllers
{
    [Route("api/equipment")]
    public class EquipmentController(ICrudService<EquipmentDTO, SaveEquipmentRequest> service)
        : CrudControllerBase<EquipmentDTO, SaveEquipmentRequest>(service);
}