﻿using FinalSurveyPractice.DTOs.Role;

namespace FinalSurveyPractice.DTOs.AuthUser
{
    public class GetUserDto
    {
        public int IdUser { get; set; }
        public string Name { get; set; } = null!;
        public string FirstSurname { get; set; } = null!;
        public string? LastSurname { get; set; }
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public byte[]? Photo { get; set; }
        public bool? Status { get; set; }
        public List<GetRoleDto> Role { get; set; }
    }
}
