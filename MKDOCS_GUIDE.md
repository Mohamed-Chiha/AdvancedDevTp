# 📖 Guide MkDocs

## Installation

### Prérequis
- Python 3.7 ou supérieur
- pip (gestionnaire de paquets Python)

### Installation de MkDocs

```bash
# Installer MkDocs et Material theme
pip install mkdocs mkdocs-material

# Vérifier l'installation
mkdocs --version
```

---

## Utilisation

### Démarrer le serveur local

```bash
cd C:\Users\lenovo\RiderProjects\AdvancedDevTP
mkdocs serve
```

**Accéder à :** `http://localhost:8000/`

La documentation se met à jour automatiquement lors de modifications.

### Générer le site statique

```bash
mkdocs build
```

Génère un dossier `site/` prêt pour le déploiement.

---

## Structure

```
docs/
├── index.md              # Page d'accueil
├── architecture.md       # Architecture en couches
├── domain.md            # Entités de domaine
├── application.md       # Services métier
├── api-endpoints.md     # Endpoints REST
├── dtos.md             # Objets de transfert
├── infrastructure.md    # Persistence
├── tests.md            # Stratégie de test
├── exceptions.md       # Gestion des exceptions
├── ci-cd.md           # Pipeline CI/CD
└── xml-documentation.md # Documentation XML

mkdocs.yml              # Configuration
```

---

## Configuration (mkdocs.yml)

Le fichier `mkdocs.yml` configure :
- Titre et description du site
- Thème Material
- Langue française
- Navigation structurée

---

## Déploiement

### GitHub Pages

```bash
# Push vers GitHub
git add .
git commit -m "Update documentation"
git push

# Configurer GitHub Pages
# Settings → Pages → Source : Deploy from a branch
# Branch : gh-pages, Folder : / (root)

# Publier
mkdocs gh-deploy
```

### Autres plateformes

- **Netlify** : Connecter repository, configurer build command
- **Vercel** : Configuration similaire à Netlify
- **Azure Static Web Apps** : Intégration GitHub native

---

## Personnalisation

### Ajouter une nouvelle page

1. Créer `docs/new-page.md`
2. Ajouter à `mkdocs.yml` :
```yaml
nav:
  - Nouvelle page: new-page.md
```

### Modifier le thème

Dans `mkdocs.yml` :
```yaml
theme:
  name: material
  palette:
    - scheme: default
      primary: blue
    - scheme: slate
      primary: blue
```

### Ajouter des extensions

```yaml
markdown_extensions:
  - pymdownx.highlight
  - pymdownx.superfences
  - admonition
```

---

## Conseils de rédaction

### Utiliser des admonitions

```markdown
!!! note
    Ceci est une note importante.

!!! warning
    Ceci est une avertissement.

!!! danger
    Ceci est un risque.
```

### Utiliser des blocs de code

```markdown
```csharp
var product = new Product("Laptop", "Desc", 10, 1299.99m, true);
```
```

### Utiliser des tableaux

```markdown
| Header 1 | Header 2 |
|----------|----------|
| Cell 1   | Cell 2   |
```

---

## Commandes utiles

```bash
# Démarrer le serveur
mkdocs serve

# Générer le site
mkdocs build

# Nettoyer les fichiers générés
mkdocs delete-docs-dir

# Lister les pages
mkdocs list-pages

# Déployer sur GitHub Pages
mkdocs gh-deploy
```

---

**Dernière mise à jour** : 2026-02-13

