# Documentation du projet AdvancedDevTP

## Images dans la documentation

Les diagrammes suivants ont été ajoutés à la documentation MkDocs :

### 1. **architecture-diagram.jpg**
   - **Localisation** : Utilisée dans `index.md` et `architecture.md`
   - **Description** : Diagramme de l'architecture en couches du projet
   - **Contenu** : Montre la dépendance entre les couches (API → Application → Domain ← Infrastructure)

### 2. **class-diagram.png**
   - **Localisation** : Utilisée dans `architecture.md`
   - **Description** : Diagramme des classes et entités du domaine
   - **Contenu** : Montre les relations entre Product, Category, Order, et OrderItem

### 3. **use-case-diagram.jpg**
   - **Localisation** : Utilisée dans `architecture.md`
   - **Description** : Diagramme des cas d'utilisation
   - **Contenu** : Les 3 groupes de fonctionnalités (Gestion des Produits, Catégories, Commandes)

### 4. **flow-diagram.jpg**
   - **Localisation** : Utilisée dans `architecture.md`
   - **Description** : Diagramme du flux d'une requête HTTP
   - **Contenu** : Montre comment une requête traverse les couches (Controller → Service → Entity → Repository)

## Visualiser la documentation

Pour voir la documentation avec les images intégrées :

```bash
pip install mkdocs mkdocs-material
cd C:\Users\lenovo\RiderProjects\AdvancedDevTP
mkdocs serve
```

Puis ouvrez `http://localhost:8000` dans votre navigateur.

## Structure des fichiers

```
docs/
├── index.md                      # Page d'accueil avec image d'architecture
├── architecture.md               # Architecture avec 4 images (diagrammes)
├── api-endpoints.md             # Endpoints de l'API
├── tests.md                     # Documentation des tests
├── ci-cd.md                     # Pipeline CI/CD
├── architecture-diagram.jpg     # Image du diagramme d'architecture
├── class-diagram.png            # Image du diagramme de classes
├── use-case-diagram.jpg         # Image du diagramme de cas d'utilisation
└── flow-diagram.jpg             # Image du diagramme de flux
```

## Fichiers de configuration

- `mkdocs.yml` : Configuration MkDocs avec thème Material et langue française

