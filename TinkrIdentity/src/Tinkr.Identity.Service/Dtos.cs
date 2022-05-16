using System;
using System.ComponentModel.DataAnnotations;

namespace Tinkr.Identity.Service
{
    public record UserDto(
        Guid Id,
        string UserName,
        string Email,
        long Xp,
        DateTimeOffset CreatedDate);

    public record UpdateUserDto(
        [Required][EmailAddress] string Email,
        [Range(1, long.MaxValue)]long Xp);
    
}