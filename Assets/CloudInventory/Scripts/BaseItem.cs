using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem
{
    public int IID { get; set; }
    public string Name { get; set; }

    public BaseItem(int iid, string name)
    {
        IID = iid;
        Name = name;
    }

    public virtual void Serialize()
    {

    }

    public virtual void Deserialize()
    {

    }
}
