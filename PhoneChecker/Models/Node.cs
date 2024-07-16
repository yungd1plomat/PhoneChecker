namespace PhoneChecker.Models
{
    // Узел бинарного дерева
    public class Node
    {
        public string Phone { get; set; }

        public Node Left { get;set; }

        public Node Right { get;set; }

        public Node(string phone)
        {
            Phone = phone;
        }
    }
}
