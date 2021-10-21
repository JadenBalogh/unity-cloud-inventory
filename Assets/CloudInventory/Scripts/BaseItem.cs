namespace CloudInventory
{
    public class BaseItem
    {
        public int IID { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }

        public virtual void Serialize(ItemData output) { }

        public virtual void Deserialize(ItemData input) { }
    }
}
