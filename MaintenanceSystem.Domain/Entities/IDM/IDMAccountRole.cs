using MaintenanceSystem.Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MaintenanceSystem.Domain.Entities.IDM
{
    [Table(DbTables.IdmAccountRole)]
    public class IDMAccountRole
    {
        [Column("ACCOUNT_ID")]
        public int AccountId { get; set; }

        [Column("ROLE_ID")]
        public int RoleId { get; set; }

        public IDMAccount Account { get; set; } = null!;

        public IDMRole Role { get; set; } = null!;
    }
}
