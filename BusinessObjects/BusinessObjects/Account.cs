﻿using System;

namespace BusinessObjects.BusinessObjects;

public enum AccountType
{
    Checking = 0,
    Investment = 1
}

public interface Idable
{
    public int Id { get; set; }
    public bool Retired { get; set; }
}

public class Account : Idable
{
    public string Description { get; set; }
    public decimal Balance { get; set; }
    public decimal Equity { get; set; }
    public int CurrencyId { get; set; }
    public string CurrencyStr { get; set; }
    public int WalletId { get; set; }
    public int TerminalId { get; set; }
    public int PersonId { get; set; }
    public long Number { get; set; }
    public DateTime? Lastupdate { get; set; }
    public virtual bool Retired { get; set; }
    public AccountType Typ { get; set; }
    public decimal DailyProfit { get; set; }
    public decimal DailyProfitPercent { get; set; }
    public decimal DailyMaxGain { get; set; }
    public bool StopTrading { get; set; }
    public string StopReason { get; set; }
    public int Id { get; set; }
    
}

public class BalanceInfo
{
    public decimal Balance { get; set; }
    public decimal Equity { get; set; }
    public long Account { get; set; }
}
