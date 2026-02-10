using AdvancedDevTP.Domain.Exceptions;

namespace AdvancedDevTP.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public DateTime OrderDate { get; private set; }
    public string CustomerName { get; private set; } = string.Empty;
    public decimal TotalAmount { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order() { }

    public Order(string customerName)
    {
        if (string.IsNullOrWhiteSpace(customerName))
            throw new DomainException("Le nom du client est obligatoire.");

        Id = Guid.NewGuid();
        CustomerName = customerName;
        OrderDate = DateTime.UtcNow;
        TotalAmount = 0;
    }

    public void AddItem(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("La quantité doit être positive.");

        if (unitPrice < 0)
            throw new DomainException("Le prix unitaire ne peut pas être négatif.");

        var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem is not null)
            throw new DomainException("Ce produit est déjà dans la commande.");

        _items.Add(new OrderItem(productId, productName, unitPrice, quantity));
        RecalculateTotal();
    }

    public void RemoveItem(Guid productId)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null)
            throw new DomainException("Produit non trouvé dans la commande.");

        _items.Remove(item);
        RecalculateTotal();
    }

    private void RecalculateTotal()
    {
        TotalAmount = _items.Sum(i => i.UnitPrice * i.Quantity);
    }
}