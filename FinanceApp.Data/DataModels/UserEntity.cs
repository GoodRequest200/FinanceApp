using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceApp.Data.DataModels;

[Table("users")]
public partial class UserEntity
{
    [Key]
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int AccountCount { get; set; }

    public virtual ICollection<AccountEntity> Accounts { get; set; } = new List<AccountEntity>();
}
