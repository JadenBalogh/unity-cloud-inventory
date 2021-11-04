namespace CloudInventory
{
    public class BaseItem
    {
        public string IID { get; set; }
        public string PlayerIID { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }

        public BaseItem() : base() { }

        public BaseItem(string playerIID, string name, int type)
        {
            PlayerIID = playerIID;
            Name = name;
            Type = type;
        }

        public virtual void Serialize(ItemData output) { }

        public virtual void Deserialize(ItemData input) { }
    }
}
