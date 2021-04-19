using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine
{
    class Drink : Product
    {
        public Drink() : base(ProductType.Drink)
        {

        }

        public override string Consume
        {
            get { return "I clenched my thirst with " + ProductName + " one of my favorite drinks."; }
        }

        public override string ProductDescription { get; set; }
        public string Volume { get; set; }
    }

    class Snack : Product
    {
        public Snack() : base(ProductType.Snacks)
        {

        }

        public override string Consume
        {
            get { return  ProductName + " is my favorite snack and it tasted awsome! Yummy!"; }
        }

        public override string ProductDescription { get; set; }

        public string Weight { get; set; }
    }

    class Food : Product
    {
        public Food() : base(ProductType.Food)
        {

        }

        public override string Consume
        {
            get { return "This " + ProductName + " tasted so good! I want another " + ProductName; }
        }

        public override string ProductDescription { get; set; }

        public string Package { get; set; }
    }
    public abstract class Product : IProduct, IDescription
    {
        private ProductType _type;
        public Product(ProductType type)
        {
            this._type = type;
        }
        public string ProductName { get; set; }
        public int ProductPrice { get; set; }
        public ProductType Type { get { return this._type; } }
        public abstract string Consume { get; }
        public int NumberOfProducts { get; set; }
        public abstract string ProductDescription { get; set; }

        public void UpdateStock(int numberOfItemsSold)
        {
            NumberOfProducts -= numberOfItemsSold;
        }
    }

    public interface IProduct
    {
        /// <summary>
        /// The name of the product.
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// The products price in SEK.
        /// </summary>
        public int ProductPrice { get; set; }
        /// <summary>
        /// The type of product as defined in enum ProductType.
        /// </summary>
        public ProductType Type { get; }
        /// <summary>
        /// Returns a string that will simulate consuming the product.
        /// </summary>
        public string Consume { get; }
        /// <summary>
        /// The total number of the specific product in stock.
        /// </summary>
        public int NumberOfProducts { get; set; }

        /// <summary>
        /// Updates the available number of items of the specific product.
        /// </summary>
        /// <param name="numberOfItemsSold">
        /// The number of items sold that should be subtracted from the available stock.
        /// Negative numbers adds to the available stock.
        /// </param>
        public void UpdateStock(int numberOfItemsSold);
    }

    public interface IDescription
    {
        public string ProductDescription { get; set; }
    }

    public enum ProductType
    {
        Drink,
        Snacks,
        Food
    }

    public class ProductNotAvailableException : Exception
    {
        public ProductNotAvailableException() : base() { }
        public ProductNotAvailableException(string message) : base(message) { }
        public ProductNotAvailableException(string message, Exception innerException) : base(message, innerException) { }
    }

}
