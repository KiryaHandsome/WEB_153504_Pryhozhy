using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_153504_Pryhozhy.Domain.Models;

namespace WEB_153504_Pryhozhy.Domain.Entities
{
    public class Cart
    {
        /// <summary>
        /// Список объектов в корзине
        /// key - идентификатор объекта
        /// </summary>
        public Dictionary<int, CartItem> CartItems { get; set; } = new();
        /// <summary>
        /// Добавить объект в корзину
        /// </summary>
        /// <param name="pizza">Добавляемый объект</param>
        public virtual void AddToCart(Pizza pizza)
        {
            var cartItem = CartItems.GetValueOrDefault(pizza.Id, new CartItem(pizza, 0));
            cartItem.Quantity++;
            CartItems[pizza.Id] = cartItem;
        }
        /// <summary>
        /// Удалить объект из корзины
        /// </summary>
        /// <param name="id"> id удаляемого объекта</param>
        public virtual void RemoveItems(int id)
        {
            CartItems.Remove(id);
        }
        /// <summary>
        /// Очистить корзину
        /// </summary>
        public virtual void ClearAll()
        {
            CartItems.Clear();
        }
        /// <summary>
        /// Количество объектов в корзине
        /// </summary>
        public int Count { get => CartItems.Sum(item => item.Value.Quantity); }
        /// <summary>
        /// Общее количество калорий
        /// </summary>
        public double TotalPrice
        {
            get => CartItems.Sum(item => item.Value.Pizza.Price * item.Value.Quantity);
        }
    }
}
