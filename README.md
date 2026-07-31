# Construction Material Calculator

A WPF desktop application for calculating and managing construction material quantities, with a material catalog and order tracking system.

## Features

- **Four domain-specific calculators**: concrete volume, steel bar weight, paint coverage, and tile quantity, each with waste-factor logic and real-time input validation
- **Material catalog management**: add, browse, and delete materials with category, unit, and price-per-unit data
- **Order tracking system**: status workflow (Pending / Delivered), dynamic filtering by status, category, and material
- **CSV export** for order data
- **JSON-based data persistence** using data binding and `ObservableCollection`

## Screenshots

See the `screenshots` folder:
- `dashboard.png` — main window with catalog stats and material list
- `calculator-form.png` — concrete/steel/paint/tile calculator
- `orders-management.png` — order tracking table with filters

## Tech Stack

- C#
- WPF / XAML
- .NET Framework
- Newtonsoft.Json

## Project Structure

```
src/
├── FinalProject.sln
└── FinalProject/
    ├── App.xaml / App.xaml.cs
    ├── MainWindow.xaml / MainWindow.xaml.cs
    ├── Widows/            # Calculator, Add Material, and Orders windows
    ├── Models/             # Material, Orders, AppData
    ├── Enums/              # Bars, Elements, Status, Surface, Tile types
    └── Helper/             # Shared helper logic
```

## Author

Esraa Omran — AEC Software Developer | BIM Developer
[LinkedIn](https://www.linkedin.com/in/esraa-omran/) · [Portfolio](https://esraaomran0.github.io/)
