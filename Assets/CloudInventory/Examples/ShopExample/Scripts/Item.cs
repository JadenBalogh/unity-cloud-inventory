using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloudInventory.Examples.ShopExample
{
    public class Item : BaseItem
    {
        public int Price { get; set; }

        public Item() : base() { }

        public Item(int playerIID, string name, int type, int price) : base(playerIID, name, type)
        {
            Price = price;
        }

        public override void Serialize(ItemData output)
        {
            output["price"] = Price;
        }

        public override void Deserialize(ItemData input)
        {
            Price = (int)input["price"];
        }
    }
}
