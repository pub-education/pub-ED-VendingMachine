using System;
using System.Collections.Generic;
using System.Globalization;

namespace VendingMachine
{

    public abstract class CashBox : Denomination, ICashBox
    {
        public Dictionary<int, int> CashBoxDrawer = new Dictionary<int, int>();

        public CashBox()
        {
            foreach (int denom in DenominationValues)
            {
                CashBoxDrawer[denom] = 0;
            }
        }
        public virtual Cash[] CashBoxContent
        {
            get
            {
                Cash[] ret = new Cash[this.CashBoxDrawer.Keys.Count];
                int i = 0;
                foreach (int key in this.CashBoxDrawer.Keys)
                {
                    ret[i].Denomination = key;
                    ret[i].Quantity = this.CashBoxDrawer[key];
                }
                return ret;
            }
        }

        public virtual int GetTotalAmount()
        {
            int sum = 0;

            foreach (int key in this.CashBoxDrawer.Keys)
            {
                sum += this.CashBoxDrawer[key] * key;
            }
            return sum;
        }

        public virtual string GetTotalAmountString()
        {
            return GetTotalAmount().ToString() + "kr";
        }
        public virtual void DepositCash(Cash[] deposit)
        {
            for (int i = 0; i < deposit.Length; i++)
            {
                this.CashBoxDrawer[deposit[i].Denomination] += deposit[i].Quantity;
            }
        }
        public virtual void DepositCash(Cash deposit)
        {
            this.CashBoxDrawer[deposit.Denomination] += deposit.Quantity;
        }
        public virtual Cash[] WithdrawCash(int amount)
        {
            Cash[] ret = new Cash[0];
            double tmp;
            int _quantity;
            bool ok = true;

            while (ok && amount > 0)
            {
                if (IsCurrencyDenomination(amount))
                {
                    this.CashBoxDrawer[amount] -= 1;
                    amount = 0;
                    Array.Resize<Cash>(ref ret, ret.Length + 1);
                    ret[ret.Length - 1].Denomination = amount;
                    ret[ret.Length - 1].Quantity = 1;
                }
                else
                {
                    for (int i = this.DenominationValues.Length - 1; i > -1; i--)
                    {
                        tmp = (double)amount / this.DenominationValues[i];
                        if (tmp >= 1.0)
                        {
                            _quantity = Convert.ToInt32(Math.Truncate(tmp));
                            if (this.CashBoxDrawer[this.DenominationValues[i]] >= _quantity)
                            {
                                this.CashBoxDrawer[this.DenominationValues[i]] -= _quantity;
                                amount = Convert.ToInt32(Math.Round((tmp - Math.Truncate(tmp)) * this.DenominationValues[i]));
                                Array.Resize<Cash>(ref ret, ret.Length + 1);
                                ret[ret.Length - 1].Denomination = this.DenominationValues[i];
                                ret[ret.Length - 1].Quantity = _quantity;
                            }
                        }
                    }
                    if (amount > 0) { throw new CannotProvideChangeException("Cannot provide the right amount of change. Please, inform the manager Mr. Krall"); }
                }
            }
            return ret;
        }
        public virtual Cash WithdrawCash(int denomination, int quantity)
        {
            Cash ret = new Cash();
            if (this.CashBoxDrawer[denomination] >= quantity)
            {
                this.CashBoxDrawer[denomination] -= quantity;
                ret.Denomination = denomination;
                ret.Quantity = quantity;
                return ret;
            }
            else
            {
                throw new CannotProvideChangeException("The requested amount " + quantity.ToString() + " of the denomination " + denomination.ToString() + " is not available.");
            }
        }

        public virtual Cash WithdrawCash(Cash cash)
        {
            if (this.CashBoxDrawer[cash.Denomination] >= cash.Quantity)
            {
                this.CashBoxDrawer[cash.Denomination] -= cash.Quantity;
                return cash;
            }
            else
            {
                throw new CannotProvideChangeException("The requested amount " + cash.Quantity.ToString() + " of the denomination " + cash.Denomination.ToString() + " is not available.");
            }
        }
    }
    public abstract class Denomination : IDenomination
    {
        /// <summary>
        /// Integer array of accepted denominations.
        /// </summary>
        readonly int[] denominations = { 1, 5, 10, 20, 50, 100, 500, 1000 };

        /// <summary>
        /// The culture info set to Sweden for formated string values.
        /// </summary>
        NumberFormatInfo nfi = new CultureInfo("se-SE").NumberFormat;

        /// <summary>
        /// Public class defining accepted denominations of Sedish currency.
        /// Provides integer and formated string data for the acceptable denominations.
        /// Implements the ICurrency interface.
        /// </summary>
        public Denomination()
        {
            nfi.CurrencyGroupSeparator = " ";
            nfi.CurrencyDecimalDigits = 0;
        }

        public int[] DenominationValues
        {
            get { return this.denominations; }
        }

        public string[] Denominations
        {
            get
            {

                string[] ret = new string[this.denominations.Length];
                for (int i = 0; i < denominations.Length; i++)
                {
                    ret[i] = denominations[i].ToString("C", nfi);
                }
                return ret;
            }
        }

        public bool IsCurrencyDenomination(int denom)
        {
            int i = 0;
            bool ok = false;
            do
            {
                if (denom == denominations[i]) ok = true;
                i++;
            } while (!ok && i < denominations.Length);

            return ok;
        }


        public string GetDenomination(int index)
        {
            if (index > -1 && index < denominations.Length)
            {
                return denominations[index].ToString("C", nfi);
            }
            else
            {
                throw new IndexOutOfRangeException("Index out of range. Index should be between 0 and " + (denominations.Length - 1).ToString());
            }
        }


        public int GetDenominationValue(int index)
        {
            if (index > -1 && index < denominations.Length)
            {
                return denominations[index];
            }
            else
            {
                throw new IndexOutOfRangeException("Index out of range. Index should be between 0 and " + (denominations.Length - 1).ToString());
            }
        }
    }

    public class IsNotDefinedDenominationException : Exception
    {
        public IsNotDefinedDenominationException() : base() { }
        public IsNotDefinedDenominationException(string message) : base(message) { }
        public IsNotDefinedDenominationException(string message, Exception innerException) : base(message, innerException) { }
    }
    public class CannotProvideChangeException : Exception
    {
        public CannotProvideChangeException() : base() { }
        public CannotProvideChangeException(string message) : base(message) { }
        public CannotProvideChangeException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException() : base() { }
        public InsufficientFundsException(string message) : base(message) { }
        public InsufficientFundsException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Public interface for defining currency.
    /// </summary>
    public interface IDenomination
    {
        /// <summary>
        /// Returns an integer array of all the availabel currency denominations.
        /// </summary>
        public int[] DenominationValues { get; }

        /// <summary>
        /// Returns a string array of all available currency denominations.
        /// Ready formated as Swedish currency and without decimals.
        /// </summary>
        public string[] Denominations { get; }

        /// <summary>
        /// Controls if the entered denomination is an accepted denomination.
        /// </summary>
        /// <param name="denom">The denomination as integer</param>
        /// <returns>Boolean True for accepted denomination otherwise false.</returns>
        public bool IsCurrencyDenomination(int denomination);

        /// <summary>
        /// Provides a string representation of the denomination index formated as Swedish currency.
        /// </summary>
        /// <param name="index">The index of the denomination</param>
        /// <returns>The string-formatted denomination corresponding to the index or throws an out-of-range exception.</returns>
        public string GetDenomination(int index);

        /// <summary>
        /// Provides an integer representation of the denomination index.
        /// </summary>
        /// <param name="index">The index of the denomination</param>
        /// <returns>The integer value of the denomination corresponding to the index or throws an out-of-range exception.</returns>
        public int GetDenominationValue(int index);
    }

    /// <summary>
    /// Public interface defining a cash box that handles both deposits and withdrawals.
    /// </summary>
    public interface ICashBox
    {
        /// <summary>
        /// Returns an array of Cash structs of all content in the cash box.
        /// </summary>
        public Cash[] CashBoxContent { get; }

        /// <summary>
        /// Deposits more than one denmination of cash into the CashBox.
        /// </summary>
        /// <param name="deposit">An array of type Cash struct.</param>
        public void DepositCash(Cash[] deposit);

        /// <summary>
        /// Deposits a single denomination of cash into the CashBox.
        /// </summary>
        /// <param name="deposit">A single Cash struct.</param>
        public void DepositCash(Cash deposit);

        /// <summary>
        /// Withdraws cash from the CashBox in more than one denomination.
        /// </summary>
        /// <param name="amount">The amount that should be withdrawn as an integer value.</param>
        /// <returns>An array of type Cash.</returns>
        public Cash[] WithdrawCash(int amount);


        /// <summary>
        /// Withdraws cash from the CashBox in a single denomination.
        /// </summary>
        /// <param name="denomination">The denomination being withdrawn.</param>
        /// <param name="quantity">The quantity of the denomination to be withdrawn.</param>
        /// <returns></returns>
        public Cash WithdrawCash(int denomination, int quantity);
    }

    /// <summary>
    /// A struct to handle the transfer and storage of money holding both the denomintaion and the quantity.
    /// </summary>
    public struct Cash
    {
        /// <summary>
        /// The denomination of the current cash object.
        /// </summary>
        public int Denomination { get; set; }

        /// <summary>
        /// The quantity of the current cash object's denomination.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Calculates the amount the current object is worth.
        /// </summary>
        /// <returns>The amount the current object is worth as an integer value.</returns>
        public int Amount() { return Denomination * Quantity; }

        /// <summary>
        /// Calculates the amount the current object is worth.
        /// </summary>
        /// <returns>The amount the current object is worth as a string value with kr at the end.</returns>
        public string DisplayAmount() { return (Denomination * Quantity).ToString() + "kr"; }
    }
}
