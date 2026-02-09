# 🚀 Pipeline CI/CD Complet - Guide d'Utilisation

## 📌 Résumé Rapide

Votre projet **AdvancedDevTP** dispose maintenant d'un **pipeline CI/CD automatisé** qui teste votre code **de bout en bout** à chaque `push` ou `pull request`.

### ⚡ En une phrase
Le pipeline **restaure** → **compile** → **teste** → **analyse la sécurité** → **génère les rapports** → **publie** votre application automatiquement.

---

## 📁 Fichiers Créés/Modifiés

| Fichier | Description |
|---------|------------|
| `.github/workflows/ci-cd.yml` | **Pipeline principal** (8 étapes) |
| `PIPELINE_DOCUMENTATION.md` | Documentation détaillée du pipeline |
| `run-tests.ps1` | Script de test local (Windows PowerShell) |
| `run-tests.sh` | Script de test local (Linux/Mac Bash) |
| `.env.pipeline` | Variables de configuration du pipeline |

---

## 🔍 Structure du Pipeline

```
┌─────────────────────────────────────────────────────────────┐
│  ÉTAPE 1: RESTORE & ANALYZE                                 │
│  └─ Restauration des packages NuGet                          │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│  ÉTAPE 2: BUILD                                             │
│  ├─ Compilation Release                                     │
│  └─ Compilation Debug                                       │
└─────────────────────────────────────────────────────────────┘
                            ↓
        ┌───────────────────┼───────────────────┐
        ↓                   ↓                   ↓
    TESTS UNIT          TESTS API          COUVERTURE
    (Unitaires)         (Intégration)       (Code Coverage)
        │                   │                   │
        └───────────────────┼───────────────────┘
                            ↓
                 SECURITY CHECK
                 (Vulnérabilités)
                            ↓
                      PUBLISH
                   (Artefacts)
                            ↓
                      SUMMARY
                   (Rapport final)
```

---

## 🏃 Comment Démarrer

### Option 1 : Test Local avant le Push

#### 🪟 Sur Windows (PowerShell)
```powershell
# Exécuter le script de test
./run-tests.ps1
```

#### 🐧 Sur Linux/Mac (Bash)
```bash
# Donner les permissions d'exécution
chmod +x run-tests.sh

# Exécuter le script
./run-tests.sh
```

### Option 2 : Déclencher Automatiquement (GitHub)

```bash
# Faire un commit et pousser
git add .
git commit -m "Nouvelle fonctionnalité"
git push origin master
```

Le pipeline se déclenche automatiquement ! 🤖

---

## 📊 Étapes Détaillées du Pipeline

### 1️⃣ Restauration & Analyse
```yaml
- Restaure les packages NuGet
- Vérifie la structure de la solution
- Condition: toujours exécutée
- Temps: ~30-60 secondes
```

### 2️⃣ Build (Compilation)
```yaml
- Compile en Release : dotnet build --configuration Release
- Compile en Debug : dotnet build --configuration Debug
- Dépend de: RESTORE
- Temps: ~2-5 minutes
```

### 3️⃣ Tests Unitaires
```yaml
- Exécute les tests avec xUnit
- Framework: xUnit + Moq + FluentAssertions
- Génère un rapport TRX (Test Results)
- Dépend de: BUILD
- Temps: ~1-3 minutes
```

### 4️⃣ Tests d'Intégration API
```yaml
- Teste les endpoints de l'API
- Utilise: Microsoft.AspNetCore.Mvc.Testing
- Génère rapports des résultats
- Dépend de: BUILD
- Temps: ~1-3 minutes
```

### 5️⃣ Analyse de Couverture de Code
```yaml
- Mesure le pourcentage de code testé
- Tool: Coverlet (Format Cobertura)
- Artefact: coverage.cobertura.xml
- Dépend de: BUILD
- Temps: ~2-4 minutes
```

### 6️⃣ Vérification de Sécurité
```yaml
- Scan des CVE (Common Vulnerabilities)
- Analyse SAST avec Roslyn
- Commande: dotnet list package --vulnerable
- Temps: ~30 secondes
```

### 7️⃣ Publication (Publish)
```yaml
- Publie l'API compilée
- Sortie: ./publish/api/
- Condition: Uniquement si tous les tests passent ✅
- Temps: ~1-2 minutes
```

### 8️⃣ Résumé & Rapport
```yaml
- Affiche le statut de toutes les étapes
- Affiche les résultats
- S'exécute: Toujours (même en erreur)
- Temps: ~10 secondes
```

---

## 📈 Artefacts Disponibles

Après l'exécution, téléchargez les artefacts depuis **GitHub Actions**:

```
📦 Artefacts disponibles:
├── test-results/
│   └── test-results.trx (Résultats tests unitaires)
├── api-test-results/
│   └── api-test-results.trx (Résultats tests API)
├── coverage-reports/
│   └── coverage.cobertura.xml (Rapport de couverture)
└── published-api/
    └── [Application compilée et prête pour déploiement]
```

---

## 🔐 Sécurité

### Vérifications Automatiques
✅ **Scan CVE NuGet** : Détecte les vulnérabilités dans les dépendances
✅ **Analyse SAST** : Détecte les problèmes de code
✅ **Style de Code** : Enforce le style unifié
✅ **Dépendances Sûres** : Vérifie les versions

---

## 🎯 Branches Surveillées

Le pipeline s'exécute sur:
- `master` ✅
- `main` ✅
- `develop` ✅

**Déclenché par:**
- `git push` sur ces branches
- `pull_request` vers ces branches

---

## 🛠️ Personnaliser le Pipeline

### Ajouter une Branche
Éditez `.github/workflows/ci-cd.yml`:
```yaml
on:
  push:
    branches: [master, main, develop, staging]  # ← Ajouter 'staging'
  pull_request:
    branches: [master, main, develop, staging]
```

### Modifier la Version .NET
```yaml
- name: Setup .NET 10
  uses: actions/setup-dotnet@v4
  with:
    dotnet-version: "11.0.x"  # ← Changer ici
```

### Ajouter des Tests d'Intégration
```csharp
[Trait("Category", "Integration")]
public async Task TestGetProductsEndpoint()
{
    // Votre test ici
}
```

---

## 📋 Checklist d'Installation

- [x] Pipeline `.github/workflows/ci-cd.yml` créé
- [x] Scripts de test local créés (`run-tests.ps1`, `run-tests.sh`)
- [x] Documentation complète (`PIPELINE_DOCUMENTATION.md`)
- [x] Variables de configuration (`.env.pipeline`)
- [ ] Configurer les notifications Slack (optionnel)
- [ ] Configurer les notifications Email (optionnel)
- [ ] Activer la protection de branche (optionnel)

---

## 🚀 Premiers Pas

### 1. Tester Localement
```powershell
# Windows
./run-tests.ps1
```

### 2. Pousser le Code
```bash
git add .
git commit -m "feat: ajout du pipeline CI/CD"
git push origin master
```

### 3. Voir les Résultats
Allez à **GitHub > Actions** et cliquez sur le workflow

### 4. Télécharger les Artefacts
Cliquez sur le workflow → Scroll down → Téléchargez les artefacts

---

## 🐛 Dépannage

### ❌ Le pipeline échoue au build
```bash
# Solution locale
dotnet clean
dotnet restore
dotnet build --configuration Release
```

### ❌ Les tests échouent
```bash
# Voir les détails des erreurs
dotnet test --verbosity normal
```

### ❌ Erreur CVE/Sécurité
```bash
# Voir les vulnérabilités
dotnet list package --vulnerable

# Mettre à jour les packages
dotnet package update
```

### ❌ Couverture de code manquante
Assurez-vous que `coverlet.collector` est dans les dépendances:
```xml
<PackageReference Include="coverlet.collector" Version="6.0.4"/>
```

---

## 📊 Exemple de Résultat

```
✅ Pipeline exécuté avec succès!

📊 Résumé des étapes:
  • Restauration & Analyse: SUCCESS
  • Build: SUCCESS
  • Tests Unitaires: SUCCESS (42 tests)
  • Tests API: SUCCESS (8 tests)
  • Couverture de Code: SUCCESS (78%)
  • Vérification Sécurité: SUCCESS (0 vulnérabilités)
  • Publication: SUCCESS
  • Résumé: SUCCESS
```

---

## 📚 Ressources Utiles

| Ressource | Lien |
|-----------|------|
| .NET 10 Docs | https://learn.microsoft.com/en-us/dotnet/ |
| GitHub Actions | https://docs.github.com/en/actions |
| xUnit | https://xunit.net/ |
| Coverlet | https://github.com/coverlet-coverage/coverlet |
| Moq | https://github.com/moq/moq4 |
| FluentAssertions | https://fluentassertions.com/ |

---

## 📞 Support

Si vous avez des questions sur le pipeline:

1. **Consultez la documentation** : `PIPELINE_DOCUMENTATION.md`
2. **Vérifiez les logs** : GitHub Actions → Workflow details
3. **Testez localement** : `./run-tests.ps1` ou `./run-tests.sh`

---

## ✨ Prochaines Étapes

### Recommandé 🎯
- [ ] Configurer les notifications Slack
- [ ] Ajouter la couverture de code minimale (threshold)
- [ ] Configurer la protection de branche `master`
- [ ] Ajouter des tests d'intégration pour chaque endpoint API

### Optionnel 💡
- [ ] Ajouter SonarQube pour l'analyse de code
- [ ] Ajouter Docker pour la conteneurisation
- [ ] Ajouter le déploiement automatique (CD)
- [ ] Ajouter les notifications Email

---

**Version** : 1.0
**Dernière mise à jour** : 9 février 2026
**Framework** : .NET 10
**CI/CD Platform** : GitHub Actions

