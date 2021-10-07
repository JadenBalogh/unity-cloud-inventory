namespace CloudInventory.Example
{
    public class Item : BaseItem
    {
        public int Price { get; set; }

        public Item(int iid, string name, int price) : base(iid, name)
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
