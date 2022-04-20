public static class ProductRepository
{
    public static List<Product>? Products { get; set; }

    public static Product GetById(string code)
    {
        return Products?.FirstOrDefault(p => p.Code == code);
    }

    public static void Add(Product product)
    {
        if (Products == null)
            Products = new List<Product>();

        Products.Add(product);
    }

    public static void Remove(Product product)
    {
        Products.Remove(product);
    }
}