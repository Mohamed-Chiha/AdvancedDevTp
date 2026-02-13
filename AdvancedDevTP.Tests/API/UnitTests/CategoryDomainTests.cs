using AdvancedDevTP.Domain.Entities;
using AdvancedDevTP.Domain.Exceptions;
using FluentAssertions;

namespace AdvancedDevTP.Tests.API.UnitTests;

/// <summary>
/// Tests unitaires pour les règles métier de l'entité Category.
/// </summary>
public class CategoryDomainTests
{
    #region Construction

    [Fact]
    public void Constructor_WithValidData_ShouldCreateCategory()
    {
        var category = new Category("Électronique", "Appareils électroniques");

        category.Id.Should().NotBeEmpty();
        category.Name.Should().Be("Électronique");
        category.Description.Should().Be("Appareils électroniques");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidName_ShouldThrowDomainException(string? name)
    {
        var act = () => new Category(name!, "desc");

        act.Should().Throw<DomainException>()
            .WithMessage("*nom*obligatoire*");
    }

    [Fact]
    public void Constructor_WithNameTooLong_ShouldThrowDomainException()
    {
        var longName = new string('A', 101);

        var act = () => new Category(longName, "desc");

        act.Should().Throw<DomainException>()
            .WithMessage("*100 caractères*");
    }

    #endregion

    #region Update

    [Fact]
    public void Update_WithValidData_ShouldUpdateFields()
    {
        var category = new Category("Old", "Old desc");

        category.Update("New", "New desc");

        category.Name.Should().Be("New");
        category.Description.Should().Be("New desc");
    }

    [Fact]
    public void Update_WithEmptyName_ShouldThrowDomainException()
    {
        var category = new Category("Valid", "desc");

        var act = () => category.Update("", "desc");

        act.Should().Throw<DomainException>();
    }

    #endregion
}