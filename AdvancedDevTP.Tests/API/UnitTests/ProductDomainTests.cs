using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Exceptions;
using FluentAssertions;

namespace AdvancedDevTP.Tests.API.UnitTests;

public class ProductDomainTests
{
    #region Construction

    [Fact]
    public void Constructor_WithValidData_ShouldCreateProduct()
    {
        // Act — order: name, description, stock, price, isActive
        var product = new Product("Laptop", "Un bon laptop", 10, 999.99m, true);

        // Assert
        product.Id.Should().NotBeEmpty();
        product.Name.Should().Be("Laptop");
        product.Description.Should().Be("Un bon laptop");
        product.Price.Should().Be(999.99m);
        product.Stock.Should().Be(10);
        product.IsActive.Should().Be(true);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidName_ShouldThrowDomainException(string? name)
    {
        // Act — stock=5, price=10m
        var act = () => new Product(name!, "desc", 5, 10m, true);

        // Assert
        act.Should().Throw<DomainException>()
           .WithMessage("*nom*obligatoire*");
    }

    [Fact]
    public void Constructor_WithNameTooLong_ShouldThrowDomainException()
    {
        var longName = new string('A', 201);

        var act = () => new Product(longName, "desc", 5, 10m, true);

        act.Should().Throw<DomainException>()
           .WithMessage("*200 caractères*");
    }

    [Fact]
    public void Constructor_WithNegativePrice_ShouldThrowDomainException()
    {
        var act = () => new Product("Laptop", "desc", 5, -1m, true);

        act.Should().Throw<DomainException>()
           .WithMessage("*prix*négatif*");
    }

    [Fact]
    public void Constructor_WithNegativeStock_ShouldThrowDomainException()
    {
        var act = () => new Product("Laptop", "desc", -1, 10m, true);

        act.Should().Throw<DomainException>()
           .WithMessage("*stock*négatif*");
    }

    #endregion

    #region ChangePrice

    [Fact]
    public void ChangePrice_WithValidIncrease_ShouldUpdatePrice()
    {
        var product = new Product("Laptop", "desc", 5, 100m, true);

        product.ChangePrice(140m); // +40% OK

        product.Price.Should().Be(140m);
    }

    [Fact]
    public void ChangePrice_WithMoreThan50PercentIncrease_ShouldThrowDomainException()
    {
        var product = new Product("Laptop", "desc", 5, 100m, true);

        var act = () => product.ChangePrice(151m); // +51% NOT OK

        act.Should().Throw<DomainException>()
           .WithMessage("*50%*");
    }

    [Fact]
    public void ChangePrice_WithDecrease_ShouldBeAllowed()
    {
        var product = new Product("Laptop", "desc", 5, 100m, true);

        product.ChangePrice(50m);

        product.Price.Should().Be(50m);
    }

    [Fact]
    public void ChangePrice_WithNegativePrice_ShouldThrowDomainException()
    {
        var product = new Product("Laptop", "desc", 5, 100m, true);

        var act = () => product.ChangePrice(-10m);

        act.Should().Throw<DomainException>()
           .WithMessage("*prix*négatif*");
    }

    [Fact]
    public void ChangePrice_FromZero_ShouldAllowAnyPositivePrice()
    {
        var product = new Product("Laptop", "desc", 5, 0m, true);

        product.ChangePrice(1000m);

        product.Price.Should().Be(1000m);
    }

    #endregion

    #region Stock

    [Fact]
    public void DecreaseStock_WithValidQuantity_ShouldDecrease()
    {
        var product = new Product("Laptop", "desc", 10, 100m, true);

        product.DeacreaseStock(3); // matches your typo in Product.cs

        product.Stock.Should().Be(7);
    }

    [Fact]
    public void DecreaseStock_WithMoreThanAvailable_ShouldThrowDomainException()
    {
        var product = new Product("Laptop", "desc", 5, 100m, true);

        var act = () => product.DeacreaseStock(10);

        act.Should().Throw<DomainException>()
           .WithMessage("*insuffisant*");
    }

    [Fact]
    public void DecreaseStock_WithZeroQuantity_ShouldThrowDomainException()
    {
        var product = new Product("Laptop", "desc", 5, 100m, true);

        var act = () => product.DeacreaseStock(0);

        act.Should().Throw<DomainException>()
           .WithMessage("*quantité*positive*");
    }

    [Fact]
    public void IncreaseStock_WithValidQuantity_ShouldIncrease()
    {
        var product = new Product("Laptop", "desc", 10, 100m, true);

        product.IncreaseStock(5);

        product.Stock.Should().Be(15);
    }

    [Fact]
    public void IncreaseStock_WithZeroQuantity_ShouldThrowDomainException()
    {
        var product = new Product("Laptop", "desc", 10, 100m, true);

        var act = () => product.IncreaseStock(0);

        act.Should().Throw<DomainException>()
           .WithMessage("*quantité*positive*");
    }

    #endregion

    #region Update

    [Fact]
    public void Update_WithValidData_ShouldUpdateAllFields()
    {
        var product = new Product("Old Name", "Old desc", 5, 100m, true);

        // matches your Update(string, string, int, decimal, bool)
        product.Update("New Name", "New desc", 15, 200m, true);

        product.Name.Should().Be("New Name");
        product.Description.Should().Be("New desc");
        product.Price.Should().Be(200m);
        product.Stock.Should().Be(15);
    }

    [Fact]
    public void Update_WithEmptyName_ShouldThrowDomainException()
    {
        var product = new Product("Laptop", "desc", 5, 100m, true);

        var act = () => product.Update("", "desc", 5, 100m, true);

        act.Should().Throw<DomainException>();
    }

    #endregion
}