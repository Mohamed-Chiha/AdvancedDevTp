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
        var product = new Product("Laptop", "Un bon laptop", 10, (decimal)999.99, true);

        // Assert
        product.Id.Should().NotBeEmpty();
        product.Name.Should().Be("Laptop");
        product.Description.Should().Be("Un bon laptop");
        product.Price.Should().Be((decimal)999.99);
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
        var act = () => new Product(name!, "desc", 5, (decimal)10, true);

        // Assert
        act.Should().Throw<DomainException>()
           .WithMessage("*nom*obligatoire*");
    }

    [Fact]
    public void Constructor_WithNameTooLong_ShouldThrowDomainException()
    {
        var longName = new string('A', 201);

        var act = () => new Product(longName, "desc", 5, (decimal)10, true);

        act.Should().Throw<DomainException>()
           .WithMessage("*200 caractères*");
    }

    [Fact]
    public void Constructor_WithNegativePrice_ShouldThrowDomainException()
    {
        var act = () => new Product("Laptop", "desc", 5, (decimal)(-1), true);

        act.Should().Throw<DomainException>()
           .WithMessage("*prix*négatif*");
    }

    [Fact]
    public void Constructor_WithNegativeStock_ShouldThrowDomainException()
    {
        var act = () => new Product("Laptop", "desc", -1, (decimal)10, true);

        act.Should().Throw<DomainException>()
           .WithMessage("*stock*négatif*");
    }

    #endregion

    #region ChangePrice

    [Fact]
    public void ChangePrice_WithValidIncrease_ShouldUpdatePrice()
    {
        var product = new Product("Laptop", "desc", 5, (decimal)100, true);

        product.ChangePrice((decimal)140); // +40% OK

        product.Price.Should().Be((decimal)140);
    }

    [Fact]
    public void ChangePrice_WithMoreThan50PercentIncrease_ShouldThrowDomainException()
    {
        var product = new Product("Laptop", "desc", 5, (decimal)100, true);

        var act = () => product.ChangePrice((decimal)151); // +51% NOT OK

        act.Should().Throw<DomainException>()
           .WithMessage("*50%*");
    }

    [Fact]
    public void ChangePrice_WithDecrease_ShouldBeAllowed()
    {
        var product = new Product("Laptop", "desc", 5, (decimal)100, true);

        product.ChangePrice((decimal)50);

        product.Price.Should().Be((decimal)50);
    }

    [Fact]
    public void ChangePrice_WithNegativePrice_ShouldThrowDomainException()
    {
        var product = new Product("Laptop", "desc", 5, (decimal)100, true);

        var act = () => product.ChangePrice((decimal)(-10));

        act.Should().Throw<DomainException>()
           .WithMessage("*prix*négatif*");
    }

    [Fact]
    public void ChangePrice_FromZero_ShouldAllowAnyPositivePrice()
    {
        var product = new Product("Laptop", "desc", 5, (decimal)0, true);

        product.ChangePrice((decimal)1000);

        product.Price.Should().Be((decimal)1000);
    }

    #endregion

    #region Stock

    [Fact]
    public void DecreaseStock_WithValidQuantity_ShouldDecrease()
    {
        var product = new Product("Laptop", "desc", 10, (decimal)100, true);

        product.DeacreaseStock(3); // matches your typo in Product.cs

        product.Stock.Should().Be(7);
    }

    [Fact]
    public void DecreaseStock_WithMoreThanAvailable_ShouldThrowDomainException()
    {
        var product = new Product("Laptop", "desc", 5, (decimal)100, true);

        var act = () => product.DeacreaseStock(10);

        act.Should().Throw<DomainException>()
           .WithMessage("*insuffisant*");
    }

    [Fact]
    public void DecreaseStock_WithZeroQuantity_ShouldThrowDomainException()
    {
        var product = new Product("Laptop", "desc", 5, (decimal)100, true);

        var act = () => product.DeacreaseStock(0);

        act.Should().Throw<DomainException>()
           .WithMessage("*quantité*positive*");
    }

    [Fact]
    public void IncreaseStock_WithValidQuantity_ShouldIncrease()
    {
        var product = new Product("Laptop", "desc", 10, (decimal)100, true);

        product.IncreaseStock(5);

        product.Stock.Should().Be(15);
    }

    [Fact]
    public void IncreaseStock_WithZeroQuantity_ShouldThrowDomainException()
    {
        var product = new Product("Laptop", "desc", 10, (decimal)100, true);

        var act = () => product.IncreaseStock(0);

        act.Should().Throw<DomainException>()
           .WithMessage("*quantité*positive*");
    }

    #endregion

    #region Update

    [Fact]
    public void Update_WithValidData_ShouldUpdateAllFields()
    {
        var product = new Product("Old Name", "Old desc", 5, (decimal)100, true);

        // matches your Update(string, string, int, decimal, bool)
        product.Update("New Name", "New desc", 15, (decimal)200, true);

        product.Name.Should().Be("New Name");
        product.Description.Should().Be("New desc");
        product.Price.Should().Be((decimal)200);
        product.Stock.Should().Be(15);
    }

    [Fact]
    public void Update_WithEmptyName_ShouldThrowDomainException()
    {
        var product = new Product("Laptop", "desc", 5, (decimal)100, true);

        var act = () => product.Update("", "desc", 5, (decimal)100, true);

        act.Should().Throw<DomainException>();
    }

    #endregion
}