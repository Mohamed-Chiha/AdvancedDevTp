# API Endpoints

## ProductController (`/api/Product`)

| Methode | Route | Description | Code succes |
|---------|-------|-------------|-------------|
| GET | `/api/Product` | Liste tous les produits | 200 |
| GET | `/api/Product/{id}` | Recupere un produit | 200 / 404 |
| POST | `/api/Product` | Cree un produit | 201 |
| PUT | `/api/Product/{id}` | Met a jour un produit | 200 / 404 |
| DELETE | `/api/Product/{id}` | Supprime un produit | 204 / 404 |
| PATCH | `/api/Product/{id}/price` | Change le prix (limite 50%) | 200 / 400 |

### Exemple : Creer un produit

```json
POST /api/Product
{
  "name": "Laptop",
  "description": "Ordinateur portable",
  "price": 1299.99,
  "stock": 10
}
```

Reponse `201 Created` :

```json
{
  "id": "12345678-1234-1234-1234-123456789012",
  "name": "Laptop",
  "description": "Ordinateur portable",
  "price": 1299.99,
  "stock": 10,
  "isActive": true
}
```

---

## CategoryController (`/api/Category`)

| Methode | Route | Description | Code succes |
|---------|-------|-------------|-------------|
| GET | `/api/Category` | Liste toutes les categories | 200 |
| GET | `/api/Category/{id}` | Recupere une categorie | 200 / 404 |
| POST | `/api/Category` | Cree une categorie | 201 |
| PUT | `/api/Category/{id}` | Met a jour une categorie | 200 / 404 |
| DELETE | `/api/Category/{id}` | Supprime une categorie | 204 / 404 |

### Exemple : Creer une categorie

```json
POST /api/Category
{
  "name": "Electronique",
  "description": "Appareils electroniques"
}
```

---

## OrderController (`/api/Order`)

| Methode | Route | Description | Code succes |
|---------|-------|-------------|-------------|
| GET | `/api/Order` | Liste toutes les commandes | 200 |
| GET | `/api/Order/{id}` | Recupere une commande | 200 / 404 |
| POST | `/api/Order` | Cree une commande | 201 |
| DELETE | `/api/Order/{id}` | Supprime une commande | 204 / 404 |

### Exemple : Creer une commande

```json
POST /api/Order
{
  "customerName": "Jean Dupont",
  "items": [
    { "productId": "12345678-...", "quantity": 2 }
  ]
}
```

A la creation, le stock des produits est automatiquement decremente.

---

## Codes d'erreur

| Code | Signification |
|------|---------------|
| 200 | Succes |
| 201 | Cree avec succes |
| 204 | Supprime avec succes |
| 400 | Donnees invalides ou regle metier violee |
| 404 | Ressource introuvable |
| 500 | Erreur serveur |
