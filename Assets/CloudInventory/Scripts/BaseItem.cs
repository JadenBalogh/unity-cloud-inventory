namespace CloudInventory
{
    public class BaseItem
    {
        public int IID { get; set; }
        public int PlayerIID { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }

        public BaseItem() : base() { }

        public BaseItem(int playerIID, string name, int type)
        {
            PlayerIID = playerIID;
            Name = name;
            Type = type;
        }

        public virtual void Serialize(ItemData output) { }

        public virtual void Deserialize(ItemData input) { }
    }
}
