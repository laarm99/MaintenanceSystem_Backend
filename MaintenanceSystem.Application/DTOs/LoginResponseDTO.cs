using System;
using System.Collections.Generic;
using System.Text;

namespace MaintenanceSystem.Application.DTOs
{
    public class LoginResponseDTO
    {
        public int Id { get; set; }

        public string DomainUserName { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
