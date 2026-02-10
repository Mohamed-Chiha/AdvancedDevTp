using AdvancedDevTP.Domain.Exceptions;

namespace AdvancedDevTP.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    private Category() { }

    public Category(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Le nom de la catégorie est obligatoire.");

        if (name.Length > 100)
            throw new DomainException("Le nom de la catégorie ne peut pas dépasser 100 caractères.");

        Id = Guid.NewGuid();
        Name = name;
        Description = description ?? string.Empty;
    }

    public void Update(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Le nom de la catégorie est obligatoire.");

        Name = name;
        Description = description ?? string.Empty;
    }
}