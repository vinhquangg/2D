[System.Serializable]
public class SlotClass
{
    public ItemData item;
    public int amount;

    public void Clear()
    {
        item = null;
        amount = 0;
    }
}
