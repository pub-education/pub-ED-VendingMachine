using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VendingMachine
{
    public class VendingMachine : IVending
    {
        private List<Product> _products;
        private VendingAccount _account;
        private Customer _customer;
        public VendingMachine()
        {
            ReStock();
            this._account = new VendingAccount();
            this._customer = new Customer();
        }
        public void DisplayGoods()
        {
            //Console.WriteLine("\n\tEnter B and the item number to purchase an item.\n\tEnter D and the item number to display information about the item.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(String.Format("\n\n{0,3} {1,30} {2,8} {3,8}", "", "Product", "Price", "Quantity"));
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            for (int i = 0; i < this._products.Count; i++)
            {
                Console.WriteLine(String.Format("\n{0,3} {1,30} {2,8} {3,8}", i.ToString(), this._products[i].ProductName, this._products[i].ProductPrice.ToString() + "kr", this._products[i].NumberOfProducts.ToString()));
            }
        }


        /// <summary>
        /// Starts the vending machine.
        /// </summary>
        public void StartVending()
        {
            bool keepVending = true;
            string select;

            Console.ResetColor();
            Console.WriteLine("\n\tEnter B and the item number to purchase an item.\n\tEnter D and the item number to display information about the item.");
            DisplayGoods();
            DepositCash();

            while (keepVending)
            {
                Console.Write("\n\n\t\tYou have ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(this._account.AvailableAmount.ToString() + "kr");
                Console.ResetColor();
                Console.Write(" left to buy for.");
                Console.WriteLine("\n\n\tEnter 'B' before product number to buy the product.\n\tEnter 'I' before the product number to display information about the product.");
                Console.Write("\tChose your product or \n\tEnter 'D' to deposit more funds.\n\tEnter 'P' to print the product list.\n\tEnter 'R' to return cash.\n\tEnter 'X' to exit the Vending Machine: ");
                select = Console.ReadLine();
                try
                {
                    if (select.ToLower().Contains('b'))
                    {
                        string[] products = select.ToLower().Split('b');
                        if (IsNumber(products[1]))
                        {
                            this._customer.AddProduct(BuyProduct(Convert.ToInt32(products[1])));
                        }
                    }
                    else if (select.ToLower().Contains('i'))
                    {
                        string[] products = select.ToLower().Split('i');
                        if (IsNumber(products[1]))
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n" + this._products[Convert.ToInt32(products[1])].ProductDescription);
                            Console.ResetColor();
                        }
                    }
                    else if (select.ToLower().Contains('d'))
                    {
                        DepositCash();
                    }
                    else if (select.ToLower().Contains('p'))
                    {
                        DisplayGoods();
                    }
                    else if (select.ToLower().Contains('r'))
                    {
                        ReturnDeposit();
                    }
                    else if (select.ToLower().Contains('x'))
                    {
                        keepVending = false;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n\tYou entered an illegal character \"" + select + "\"");
                        Console.ResetColor();
                    }
                }
                catch (InsufficientFundsException ex)
                {
                    Console.Write("\n\t" + ex.Message + "\n\tDeposit more funds? Y/N ");
                    select = Console.ReadKey().KeyChar.ToString();
                    if (select.ToLower() == "y")
                    {
                        DepositCash();
                    }
                }
                catch (ProductNotAvailableException ex)
                {
                    Console.Write("\n\t" + ex.Message + "\n\tChose another product? Y/N ");
                    select = Console.ReadKey(true).KeyChar.ToString();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine("\n\t" + ex.Message);
                    Console.ResetColor();
                }

                if (this._customer.NumberOfAvailableProducts > 0)
                {
                    Console.Write("\n\tWant to consume any of your purchases? Y/N ");
                    select = Console.ReadKey(true).KeyChar.ToString();
                    if (select.ToLower() == "y")
                    {
                        // Create ConsumeProduct in Customer.
                        this._customer.ConsumeProducts();
                    }
                }
            }

        }

        public void ReStock()
        {
            this._products = new List<Product>();
            RestockDrink();
            RestockSnacks();
            RestockFood();
        }

        public void RestockFood()
        {
            Food _food = new Food();

            _food.ProductName = "Fransk Löksoppa";
            _food.ProductPrice = 25;
            _food.NumberOfProducts = 5;
            _food.ProductDescription = "\tKlassisk fransk löksoppa med rund och välbalanserad smak.\n\tPassar lika bra att servera till lunch som till kvällen eller som värmande mellanmål,\n\tmed eller utan bröd.";
            _food.Package = "Påse 77 gram";
            this._products.Add(_food);

            _food = new Food();
            _food.ProductName = "Potatis & Purjolök";
            _food.ProductPrice = 19;
            _food.NumberOfProducts = 5;
            _food.ProductDescription = "\tVarma Koppen - matig soppa i kopp- Unna dig ett enkelt och gott mellanmål\n\ten perfekt paus helt enkelt! Ett super enkelt mellanmål.\n\tTöm påsen i en mugg, tillsätt kokande vatten, låt stå några minuter…njut!";
            _food.Package = "3 påsar a 20 gram";
            this._products.Add(_food);

            _food = new Food();
            _food.ProductName = "Redd Kyckling soppa";
            _food.ProductPrice = 25;
            _food.NumberOfProducts = 5;
            _food.ProductDescription = "\tVarma Koppen-matig soppa i kopp- Unna dig ett enkelt och gott mellanmål – en perfekt paus helt enkelt!\n\tEtt super enkelt mellanmål, töm påsen i en mugg, tillsätt kokande vatten,\n\tlåt stå några minuter…njut!";
            _food.Package = "3 påsar a 20 gram";
            this._products.Add(_food);

            _food = new Food();
            _food.ProductName = "Blå Band Mexikansk soppa";
            _food.ProductPrice = 19;
            _food.NumberOfProducts = 5;
            _food.ProductDescription = "\tBlå Band Klassiska soppor är en serie krämiga och mycket välsmakande soppor.\n\tDe passar utmärkt som lunch, kvällsmat eller som ett värmande mellanmål.\n\tEn favorit hos många tack vare den snabba och enkla tillagningen.";
            _food.Package = "Påse 69 gram";
            this._products.Add(_food);
        }

        public void RestockSnacks()
        {
            Snack _snack = new Snack();

            _snack.ProductName = "Dajm";
            _snack.ProductPrice = 9;
            _snack.NumberOfProducts = 5;
            _snack.ProductDescription = "\tDaim Dubbel- En riktig klassiker i form av krispig mandelknäck\n\töverdragen med härlig mjölkchoklad.\n\tEn perfekt och balanserad kombination utav hårt och mjukt!";
            _snack.Weight = "56 gram";
            this._products.Add(_snack);

            _snack = new Snack();
            _snack.ProductName = "Guldnougat";
            _snack.ProductPrice = 10;
            _snack.NumberOfProducts = 5;
            _snack.ProductDescription = "\tKrämig och mjuk hasselnötsnougat. Passar när du är sugen på något sött!";
            _snack.Weight = "50 gram";
            this._products.Add(_snack);

            _snack = new Snack();
            _snack.ProductName = "Pigall";
            _snack.ProductPrice = 9;
            _snack.NumberOfProducts = 5;
            _snack.ProductDescription = "\tPigall- Visst kan en chokladkaka vara chick!\n\tSe bara på Pigall, som under sitt glänsande omslag bjuder på lätt,\n\tvispad hasselnötstryffel med ett täcke av mjölkchoklad.\n\tEn riktig elegant!";
            _snack.Weight = "40 gram";
            this._products.Add(_snack);

            _snack = new Snack();
            _snack.ProductName = "Center";
            _snack.ProductPrice = 9;
            _snack.NumberOfProducts = 5;
            _snack.ProductDescription = "\tEn ljus mjölkchokladpralin fylld med toffee.\n\tBjud på i biomörkret, på läktaren eller bara när som helst.\n\tRulla av ett varv och bjud!";
            _snack.Weight = "78 gram";
            this._products.Add(_snack);
        }

        public void RestockDrink()
        {
            Drink _drink = new Drink();

            _drink.ProductName = "Fruktsoda";
            _drink.ProductPrice = 8;
            _drink.NumberOfProducts = 10;
            _drink.ProductDescription = "\tFruktsoda är en klassisk svensk läskedryck.\n\tDen är färglös och påminner med sin citron- eller limesmak om 7Up och Sprite.";
            _drink.Volume = "33cl";
            this._products.Add(_drink);

            _drink = new Drink();
            _drink.ProductName = "Päronsoda";
            _drink.ProductPrice = 8;
            _drink.NumberOfProducts = 10;
            _drink.ProductDescription = "\tPäronsoda är en läskedryck med smak av päron.\n\tOrdet nämns ofta i filmen Repmånad.";
            _drink.Volume = "33cl";
            this._products.Add(_drink);

            _drink = new Drink();
            _drink.ProductName = "Sockerdricka";
            _drink.ProductPrice = 8;
            _drink.NumberOfProducts = 10;
            _drink.ProductDescription = "\tSockerdricka är en gammal och numera klassisk svensk läskedryck som började tillverkas på 1800-talet\n\toch som numera innehåller kolsyrat vatten, socker, citronsyra och aromämnen.\n\tNär man började tillverka sockerdricka tillsatte man inte kolsyra till vattnet utan istället jäste man den.\n\tDå bildades kolsyra men även etanol, och dessutom kryddade man den med ingefära[1],\n\tså sockerdrickan hade en helt annan smak än den vi är vana vid.\n\tSockerdricka var förr även populär bland äldre herrar som \"groggvirke\" i de konjaksgroggar eller\n\t\"mahognygroggar\"(bestående av cognac eller eau - de - vie plus sockerdricka) som dracks ur höga glas\n\tsom kunde rymma upp till 50 cl.\n\tEn känd tillverkare av sockerdricka är Apotekarnes, som började sälja sockerdricka redan 1908,\n\tmen numera tillverkas sockerdricka av de flesta svenska bryggerier.\n\tDrycken förekommer i familjefilmer som till exempel Pippi Långstrump,\n\tdär Pippi har ett träd i sin trädgård som det växer sockerdricka ur.\n\tEtt annat exempel är Emil i Lönneberga.";
            _drink.Volume = "33cl";
            this._products.Add(_drink);

            _drink = new Drink();
            _drink.ProductName = "Cuba Cola";
            _drink.ProductPrice = 10;
            _drink.NumberOfProducts = 5;
            _drink.ProductDescription = "\tCuba Cola är en läskedryck som lanserades 1953 och är därmed Sveriges äldsta colasort.\n\tReceptet på Cuba Cola ägs av Spendrups Bryggeri AB i Vårby,\n\tsom licensierade rättigheten för tillverkning till fyra svenska bryggerier.\n\tDessa var Vasa Bryggeri, Guttsta Källa, Krönleins och Hammars Bryggeri.\n\t2020 tog Spendrups över all tillverkning och designade om etiketten.\n\tSamtidigt lanserades en sockerfri variant.";
            _drink.Volume = "33cl";
            this._products.Add(_drink);

            _drink = new Drink();
            _drink.ProductName = "Trocadero";
            _drink.ProductPrice = 8;
            _drink.NumberOfProducts = 5;
            _drink.ProductDescription = "\tTrocadero, ibland Troca, är en svensk läskedryck med koffein som är smaksatt med apelsin och äpple.\n\tDen lanserades i Sverige sommaren 1953 av Saturnus AB i Malmö.\n\tTrocadero har genom åren varit särskilt populär i Norrland och har kallats \"Norrlands nationaldryck\"";
            this._products.Add(_drink);

            _drink = new Drink();
            _drink.ProductName = "Pucko";
            _drink.ProductPrice = 14;
            _drink.NumberOfProducts = 5;
            _drink.ProductDescription = "\tPucko är en chokladdryck som tillverkas i Danmark av Cocio,\n\tsedan 2008 ett dotterbolag till Arla. \n\tDen lanserades 1954 av Mjölkcentralen, sedermera Arla.\n\tPucko producerades på sterilgräddefabriken i Järlåsa fram till 1987. \n\tDå fick Semper i Laholm ta över tillverkningen och fabriken i Järlåsa lades ner. \n\tNär Semper såldes 2003 valde Arla att behålla namnet Pucko och Semper såldes utan rättigheterna för drycken. \n\tDrycken fortsatte att produceras i Laholm. \n\tSommaren 2005 flyttades tillverkningen av Pucko till Cocios fabrik i Esbjerg på Jylland i Danmark. \n\tCocio är sedan 2008 ett helägt dotterbolag till Arla, som sedan tidigare producerat flera sorters chokladdrycker.";
            _drink.Volume = "27cl";
            this._products.Add(_drink);
        }

        public void DepositCash()
        {
            string denomination, quantity;
            char select;
            bool wantToDeposit = true;

            while (wantToDeposit)
            {
                Console.WriteLine("\n\tYou have the following funds available for purchases.\n\tYou can only use the denominations that are available to a maximum of the available quantity.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n\tDenomination\tQuantity\tAmount");
                Console.ResetColor();
                foreach (int key in this._customer.GetWallet.CashBoxDrawer.Keys)
                {
                    Console.WriteLine("\t" + key.ToString() + "kr\t\t" + this._customer.GetWallet.CashBoxDrawer[key] + "\t\t" + this._customer.GetDenominationAmount(key).ToString() + "kr");
                }
                Console.WriteLine("\t\t\tTotal: \t\t" + this._customer.GetWallet.GetTotalAmountString());
                Console.Write("\n\tDeposit cash.\n\tEnter the denomination: ");
                denomination = Console.ReadLine();
                if (IsNumber(denomination) && this._account.IsCurrencyDenomination(Convert.ToInt32(denomination)))
                {
                    Console.Write("\tEnter the quantity: ");
                    quantity = Console.ReadLine();

                    if (IsNumber(quantity))
                    {
                        Cash tmp = new Cash();
                        tmp.Denomination = Convert.ToInt32(denomination);
                        tmp.Quantity = Convert.ToInt32(quantity);
                        try
                        {
                            this._account.DepositCash(this._customer.GetWallet.WithdrawCash(tmp));
                            Console.WriteLine("\n\tYou have " + this._account.AvailableAmount.ToString() + "kr availble for purchases.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    Console.Write("\n\tWant to deposit more? Y/N ");
                    select = Console.ReadKey(true).KeyChar;
                    if (select.ToString().ToLower() == "n") wantToDeposit = false;
                }
                else
                {
                    Console.Write("\n\tThe entered denomination ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(denomination);
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write(" is not a valid denomination. \n\tAvailable denominations are:\n\t");
                    for (int i = 0; i < this._account.Denominations.Length; i++)
                    {
                        if (i == this._account.Denominations.Length - 1)
                        {
                            Console.Write(this._account.Denominations[i]);
                        }
                        else if (i == this._account.Denominations.Length - 2)
                        {
                            Console.Write(this._account.Denominations[i] + " and ");
                        }
                        else
                        {
                            Console.Write(this._account.Denominations[i] + ", ");
                        }
                    }
                    Console.Write("\n\tPress any key to continue. ");
                    select = Console.ReadKey(true).KeyChar;
                }
            }
        }

        public static bool IsNumber(string data)
        {
            string pattern = @"[^0-9\.\-]";
            Regex rgx = new Regex(pattern);
            MatchCollection matches = rgx.Matches(data);
            if (matches.Count > 0 || data.Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public Product BuyProduct(int productNumber)
        {
            if (this._products[productNumber].NumberOfProducts > 0)
            {
                if (this._account.AvailableAmount >= this._products[productNumber].ProductPrice)
                {
                    this._products[productNumber].NumberOfProducts -= 1;
                    this._account.AvailableAmount -= this._products[productNumber].ProductPrice;
                    return this._products[productNumber];
                }
                else
                {
                    throw new InsufficientFundsException("You don't have enough funds to buy this item.");
                }
            }
            else
            {
                throw new ProductNotAvailableException("The requested product is unfortunately sold out.");
            }
        }

        public void ReturnDeposit()
        {
            int sum = 0;
            try
            {
                Cash[] change = this._account.WithdrawCash(this._account.AvailableAmount);
                this._customer.GetWallet.DepositCash(change);

                foreach (Cash x in change)
                {
                    sum += x.Amount();
                }
                this._account.AvailableAmount -= sum;
            }
            catch (CannotProvideChangeException ex)
            {
                Console.WriteLine("\n\tThe machine cannot give back exact change.\n\t" + ex.Message);
            }

            Console.WriteLine("\n\tThere is " + this._account.AvailableAmount + "kr available for purchase.");

            Console.WriteLine("\n\tYou have " + this._customer.GetWallet.GetTotalAmountString() + " in total.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\tDenomination\tQuantity\tAmount");
            Console.ResetColor();
            foreach (int key in this._customer.GetWallet.CashBoxDrawer.Keys)
            {
                Console.WriteLine("\t" + key.ToString() + "kr\t\t" + this._customer.GetWallet.CashBoxDrawer[key] + "\t\t" + this._customer.GetDenominationAmount(key).ToString() + "kr");
            }
        }
    }


    public interface IVending
    {
        /// <summary>
        /// Will re-set stock content to initial values and quantities.
        /// </summary>
        public void ReStock();

        /// <summary>
        /// Displays a list of available goods for purchase including:
        /// Item ID, Item Name, Item Price, Item Quantity available.
        /// </summary>
        public void DisplayGoods();

        /// <summary>
        /// Enables customer to deposit cash into the machine to purchase good.
        /// </summary>
        public void DepositCash();

        /// <summary>
        /// Returns the unused funds to the customer.
        /// </summary>
        public void ReturnDeposit();

        /// <summary>
        /// Simulates the actual buying of the product.
        /// </summary>
        /// <param name="productNumber">The product ID</param>
        /// <returns>AN object with the base type Product</returns>
        public Product BuyProduct(int productNumber);

    }

    public class VendingAccount : CashBox, IVendingAccount
    {

        public VendingAccount() : base()
        {
            List<int> keys = new List<int>(base.CashBoxDrawer.Keys);
            foreach (int key in keys)
            {
                base.CashBoxDrawer[key] = 10;
            }
        }

        public override void DepositCash(Cash deposit)
        {
            base.DepositCash(deposit);
            AvailableAmount += deposit.Amount();
        }
        public override void DepositCash(Cash[] deposit)
        {
            base.DepositCash(deposit);

            for (int i = 0; i < deposit.Length; i++)
            {
                AvailableAmount += deposit[i].Amount();
            }
        }

        public override Cash WithdrawCash(int denomination, int quantity)
        {
            AvailableAmount -= (denomination * quantity);
            return base.WithdrawCash(denomination, quantity);
        }
        public int AvailableAmount { get; set; }
    }

    public interface IVendingAccount
    {
        /// <summary>
        /// The sum of the deposited amount available for purchases.
        /// </summary>
        int AvailableAmount { get; set; }
    }
}
