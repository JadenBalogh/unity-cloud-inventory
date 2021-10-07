public class BaseItem
{
    public int IID { get; set; }
    public string Name { get; set; }

    public BaseItem(int iid, string name)
    {
        IID = iid;
        Name = name;
    }

    public virtual void Serialize(ItemData output) { }

    public virtual void Deserialize(ItemData input) { }
}
