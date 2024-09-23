﻿using System.ComponentModel.DataAnnotations;

namespace Infinion.Core.DTOs
{
    public class ForgotPasswordDto
    {
        [Required][EmailAddress] public string Email { get; set; }
    }
}

