using System.Collections.Generic;
using System.Linq;

namespace QLBH_LeatherNotebooksShopApp.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public void AddToCart(Product product, int quantity)
        {
            var item = Items.FirstOrDefault(i => i.Product.ProductID == product.ProductID);
            if (item == null)
            {
                Items.Add(new CartItem { Product = product, Quantity = quantity });
            }
            else
            {
                item.Quantity += quantity;
            }
        }

        public void RemoveFromCart(int productId)
        {
            var item = Items.FirstOrDefault(i => i.Product.ProductID == productId);
            if (item != null)
            {
                Items.Remove(item);
            }
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            var item = Items.FirstOrDefault(i => i.Product.ProductID == productId);
            if (item != null)
            {
                item.Quantity = quantity;
            }
        }

        public decimal GetTotalPrice()
        {
            return (decimal)Items.Sum(i => i.Product.Price * i.Quantity);
        }

        public int GetTotalQuantity()
        {
            return Items.Sum(i => i.Quantity);
        }

        public void ClearCart()
        {
            Items.Clear();
        }
    }

    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
