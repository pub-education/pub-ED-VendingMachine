using System;
using System.Collections.Generic;

namespace VendingMachine
{

    public class Customer : ICustomer
    {
        private Wallet _wallet = new Wallet();
        private List<Product> _products = new List<Product>();

        public Wallet GetWallet { get { return this._wallet; } }

        public int GetDenominationAmount(int denomination)
        {
            if (this._wallet.IsCurrencyDenomination(denomination))
            {
                return this._wallet.CashBoxDrawer[denomination] * denomination;
            }
            else
            {
                throw new IsNotDefinedDenominationException("The denomination '" + denomination.ToString() + "' is not a recognized denomination.");
            }
        }

        public void AddProduct(Product product)
        {
            this._products.Add(product);
        }

        public void ConsumeProducts()
        {
            bool keepConsuming = true;
            string select;
            string[] products;

            if (!(this._products.Count > 0))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\n\tYou have no products available to consume.");
                Console.ResetColor();
                keepConsuming = false;
            }
            while (keepConsuming)
            {
                Console.WriteLine("\n\tYou can consume any of the following products:");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n\tIndex\tProduct Name");
                Console.ResetColor();
                for (int i = 0; i < this._products.Count; i++)
                {
                    Console.WriteLine("\t" + i.ToString() + "\t" + this._products[i].ProductName);
                }
                Console.Write("\n\tTo consume a product enter 'C' and the index for the item you want to consume..\n\tTo get information about a product enter 'I' and the index for your choice.\n\tTo stop consuming enter 'X': ");
                select = Console.ReadLine();

                if (select.ToLower().Contains('c'))
                {
                    products = select.ToLower().Split('c');

                    if (VendingMachine.IsNumber(products[1]))
                    {
                        if (Convert.ToInt32(products[1]) < this._products.Count)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n\t" + this._products[Convert.ToInt32(products[1])].Consume);
                            Console.ResetColor();
                            this._products.RemoveAt(Convert.ToInt32(products[1]));
                            if (!(this._products.Count > 0)) keepConsuming = false;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n\tEntered index for product is invalid.");
                            Console.ResetColor();
                        }
                    }
                }
                else if (select.ToLower().Contains('i'))
                {
                    products = select.ToLower().Split('i');

                    if (VendingMachine.IsNumber(products[1]))
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("\n\t" + this._products[Convert.ToInt32(products[1])].ProductDescription);
                        Console.ResetColor();
                    }
                }
                else if (select.ToLower().Contains('x'))
                {
                    keepConsuming = false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\tYou entered an unavailable selection \"" + select + "\"\n\tTry again!");
                    Console.ResetColor();
                }
            }
        }

        public List<Product> PurchasedProducts { get { return this._products; } }

        public int NumberOfAvailableProducts
        {
            get { return this._products.Count; }
        }

    }

    public class Wallet : CashBox
    {
        public Wallet() : base()
        {
            List<int> keys = new List<int>(base.CashBoxDrawer.Keys);
            foreach (int key in keys)
            {
                switch (key)
                {
                    case 1:
                        this.CashBoxDrawer[key] = 10;
                        break;
                    case 5:
                        this.CashBoxDrawer[key] = 5;
                        break;
                    case 10:
                        this.CashBoxDrawer[key] = 4;
                        break;
                    case 20:
                        this.CashBoxDrawer[key] = 5;
                        break;
                    case 50:
                        this.CashBoxDrawer[key] = 2;
                        break;
                    case 100:
                        this.CashBoxDrawer[key] = 2;
                        break;
                    default:
                        this.CashBoxDrawer[key] = 0;
                        break;
                }
            }
        }
        public Wallet(Cash[] deposits) : base()
        {
            base.DepositCash(deposits);
        }

        public Wallet(Cash deposit)
        {
            base.DepositCash(deposit);
        }
    }

    public interface ICustomer
    {
        public Wallet GetWallet { get; }

        public int GetDenominationAmount(int denomination);

        public void AddProduct(Product product);

        public List<Product> PurchasedProducts { get; }

        public void ConsumeProducts();

        public int NumberOfAvailableProducts { get; }
    }
}
