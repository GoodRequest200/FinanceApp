using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceApp.Data.DataModels;

[Table("accounts")]
public partial class AccountEntity
{
    public int AccountId { get; set; }

    public int UserId { get; set; }

    public string CurrencyType { get; set; } = null!;

    public decimal Balance { get; set; }

    public virtual ICollection<TransferEntity> TransferAccounts { get; set; } = new List<TransferEntity>();

    public virtual ICollection<TransferEntity> TransferAppointmentAccounts { get; set; } = new List<TransferEntity>();

    public virtual UserEntity User { get; set; } = null!;
}
