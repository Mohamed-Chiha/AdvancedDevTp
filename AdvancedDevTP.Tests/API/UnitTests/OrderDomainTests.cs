using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Exceptions;
using FluentAssertions;

namespace AdvancedDevTP.Tests.API.UnitTests;

/// <summary>
/// Tests unitaires pour les règles métier de l'entité Order.
/// </summary>
public class OrderDomainTests
{
    #region Construction

    [Fact]
    public void Constructor_WithValidData_ShouldCreateOrder()
    {
        var order = new Order("Mohamed Chiha");

        order.Id.Should().NotBeEmpty();
        order.CustomerName.Should().Be("Mohamed Chiha");
        order.OrderDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        order.TotalAmount.Should().Be(0);
        order.Items.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidCustomerName_ShouldThrowDomainException(string? name)
    {
        var act = () => new Order(name!);

        act.Should().Throw<DomainException>()
           .WithMessage("*nom*client*obligatoire*");
    }

    #endregion

    #region AddItem

    [Fact]
    public void AddItem_WithValidData_ShouldAddItemAndUpdateTotal()
    {
        var order = new Order("Client Test");
        var productId = Guid.NewGuid();

        order.AddItem(productId, "Laptop", 500m, 2);

        order.Items.Should().HaveCount(1);
        order.TotalAmount.Should().Be(1000m);
    }

    [Fact]
    public void AddItem_MultipleProducts_ShouldCalculateTotalCorrectly()
    {
        var order = new Order("Client Test");

        order.AddItem(Guid.NewGuid(), "Laptop", 500m, 2);
        order.AddItem(Guid.NewGuid(), "Mouse", 25m, 3);

        order.Items.Should().HaveCount(2);
        order.TotalAmount.Should().Be(1075m); // (500*2) + (25*3)
    }

    [Fact]
    public void AddItem_WithZeroQuantity_ShouldThrowDomainException()
    {
        var order = new Order("Client Test");

        var act = () => order.AddItem(Guid.NewGuid(), "Laptop", 500m, 0);

        act.Should().Throw<DomainException>()
           .WithMessage("*quantité*positive*");
    }

    [Fact]
    public void AddItem_WithNegativeQuantity_ShouldThrowDomainException()
    {
        var order = new Order("Client Test");

        var act = () => order.AddItem(Guid.NewGuid(), "Laptop", 500m, -1);

        act.Should().Throw<DomainException>()
           .WithMessage("*quantité*positive*");
    }

    [Fact]
    public void AddItem_WithNegativePrice_ShouldThrowDomainException()
    {
        var order = new Order("Client Test");

        var act = () => order.AddItem(Guid.NewGuid(), "Laptop", -10m, 1);

        act.Should().Throw<DomainException>()
           .WithMessage("*prix*négatif*");
    }

    [Fact]
    public void AddItem_DuplicateProduct_ShouldThrowDomainException()
    {
        var order = new Order("Client Test");
        var productId = Guid.NewGuid();

        order.AddItem(productId, "Laptop", 500m, 1);
        var act = () => order.AddItem(productId, "Laptop", 500m, 2);

        act.Should().Throw<DomainException>()
           .WithMessage("*déjà dans la commande*");
    }

    #endregion

    #region RemoveItem

    [Fact]
    public void RemoveItem_WithExistingProduct_ShouldRemoveAndRecalculate()
    {
        var order = new Order("Client Test");
        var productId1 = Guid.NewGuid();
        var productId2 = Guid.NewGuid();

        order.AddItem(productId1, "Laptop", 500m, 2);
        order.AddItem(productId2, "Mouse", 25m, 3);

        order.RemoveItem(productId1);

        order.Items.Should().HaveCount(1);
        order.TotalAmount.Should().Be(75m); // 25*3
    }

    [Fact]
    public void RemoveItem_WithNonExistingProduct_ShouldThrowDomainException()
    {
        var order = new Order("Client Test");

        var act = () => order.RemoveItem(Guid.NewGuid());

        act.Should().Throw<DomainException>()
           .WithMessage("*non trouvé*");
    }

    #endregion
}