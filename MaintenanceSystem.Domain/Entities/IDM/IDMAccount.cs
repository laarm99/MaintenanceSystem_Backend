using MaintenanceSystem.Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MaintenanceSystem.Domain.Entities.IDM
{
    [Table(DbTables.IdmAccount)]
    public class IDMAccount
    {
        [Column("ID")]
        public int Id { get; set; }
        
        [Column("DOMAIN_USER_NAME")]
        public string DomainUserName { get; set; } = string.Empty;
        
        [Column("FULL_NAME")]
        public string FullName { get; set; } = string.Empty;
        
        [Column("EMAIL")]
        public string Email { get; set; } = string.Empty;
        
        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; }

        [Column("CREATED_DATE")]
        public DateTime CreatedDate { get; set; }
    }
}
