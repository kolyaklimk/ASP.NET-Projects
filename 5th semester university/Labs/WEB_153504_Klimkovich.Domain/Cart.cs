using WEB_153504_Klimkovich.Domain.Entities;

namespace WEB_153504_Klimkovich.Domain
{
    public class Cart
    {
        public Dictionary<int, CartItem> CartItems { get; set; } = new();

        public virtual void AddToCart(Electronics electronic)
        {
            int electronicId = electronic.Id;

            if (CartItems.ContainsKey(electronicId))
            {
                CartItems[electronicId].Count++;
            }
            else
            {
                CartItems[electronicId] = new CartItem
                {
                    Id = electronic.Id,
                    Electronic = electronic,
                    Count = 1
                };
            }
        }

        public virtual void RemoveItem(int id)
        {
            if (CartItems.ContainsKey(id))
            {
                if (CartItems[id].Count > 1)
                {
                    CartItems[id].Count--;
                }
                else
                {
                    CartItems.Remove(id);
                }
            }
        }

        public virtual void ClearAll()
        {
            CartItems.Clear();
        }

        public int Count
        {
            get { return CartItems.Values.Sum(item => item.Count); }
        }


        public decimal TotalPrice
        {
            get { return CartItems.Values.Sum(item => item.Count * item.Electronic.Price); }
        }
    }

}
