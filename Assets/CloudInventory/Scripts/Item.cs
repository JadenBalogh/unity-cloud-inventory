public class Item : BaseItem
{
    public int Price { get; set; }

    public Item(int iid, string name, int price) : base(iid, name)
    {
        Price = price;
    }

    public override void Serialize()
    {
        base.Serialize();
    }

    public override void Deserialize()
    {
        base.Deserialize();
    }
}
