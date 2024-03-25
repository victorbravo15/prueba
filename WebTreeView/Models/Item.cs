namespace WebTreeView2.Models
{
    public class Item
    {
        public string Name { get; set; }
        public List<Item> Children { get; set; }
    }
}
