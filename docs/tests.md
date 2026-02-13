# Tests

## Strategie

Le projet utilise deux types de tests :

- **Tests unitaires** : testent les entites et services de maniere isolee (avec Moq)
- **Tests d'integration** : testent le pipeline HTTP complet (avec WebApplicationFactory)

## Tests unitaires

### Tests de domaine

Testent les regles metier directement sur les entites, sans aucune dependance.

| Classe | Ce qui est teste |
|--------|------------------|
| `ProductDomainTests` | Validation nom/prix/stock, limite augmentation prix 50%, gestion stock |
| `CategoryDomainTests` | Validation nom obligatoire, longueur max 100 caracteres |
| `OrderDomainTests` | Ajout/suppression articles, calcul total, doublons interdits |

### Tests de service

Testent la logique des services avec des mocks (Moq) des repositories.

| Classe | Ce qui est teste |
|--------|------------------|
| `ProductServiceTests` | CRUD produit, changement de prix, exceptions |
| `CategoryServiceTests` | CRUD categorie, exceptions |
| `OrderServiceTests` | Creation commande avec decrementation stock, exceptions |

Exemple :

```csharp
[Fact]
public async Task GetByIdAsync_WithNonExistingId_ShouldThrow()
{
    _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Product?)null);

    var act = () => _service.GetByIdAsync(Guid.NewGuid());

    await act.Should().ThrowAsync<ApplicationServiceException>();
}
```

## Tests d'integration

Testent le flux HTTP complet : requete -> controller -> service -> repository -> reponse.

| Classe | Ce qui est teste |
|--------|------------------|
| `ProductAsyncControllerIntegrationTest` | POST 201, GET 200, PUT 200, DELETE 204, PATCH prix |
| `CategoryControllerIntegrationTest` | POST 201, GET 200, PUT 200, DELETE 204 |
| `OrderControllerIntegrationTest` | POST 201 avec stock, DELETE 204, erreurs 404 |

Les tests utilisent `CustomWebApplicationFactory` qui remplace les repositories EF Core par des repositories en memoire (`InMemoryProductRepositoryAsync`, etc.).

## Execution

```bash
# Tous les tests
dotnet test

# Tests unitaires uniquement
dotnet test --filter "FullyQualifiedName!~Integrations"

# Tests d'integration uniquement
dotnet test --filter "FullyQualifiedName~Integrations"
```
