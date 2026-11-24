using MaintenanceSystem.Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MaintenanceSystem.Domain.Entities.IDM
{
    [Table(DbTables.IdmRole)]
    public class IDMRole
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("ROLE_NAME")]
        public string RoleName { get; set; } = string.Empty;

        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; }
    }
}
