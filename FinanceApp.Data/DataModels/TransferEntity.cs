using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceApp.Data.DataModels;

[Table("transfers")]
public partial class TransferEntity
{
    public int TransferId { get; set; }

    public int AccountId { get; set; }

    public int AppointmentAccountId { get; set; }

    public string CurrencyType { get; set; } = null!;

    public DateTime TransferDate { get; set; }

    public decimal TransferSum { get; set; }

    public virtual AccountEntity Account { get; set; } = null!;

    public virtual AccountEntity AppointmentAccount { get; set; } = null!;
}
