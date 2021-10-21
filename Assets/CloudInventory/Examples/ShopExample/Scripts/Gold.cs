using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CloudInventory.Examples.ShopExample
{
    public class Gold : BaseItem
    {
        public int Amount { get; set; }

        public Gold() : base() { }

        public Gold(int playerIID, int amount) : base(playerIID, "Gold", (int)ItemType.Currency)
        {
            Amount = amount;
        }

        public override void Serialize(ItemData output)
        {
            output["amount"] = Amount;
        }

        public override void Deserialize(ItemData input)
        {
            Amount = (int)input["amount"];
        }
    }
}
