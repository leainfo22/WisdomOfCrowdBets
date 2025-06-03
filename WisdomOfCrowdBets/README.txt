# WisdomOfCrowdBets

## Overview

**WisdomOfCrowdBets** is a .NET 8 solution designed to analyze sports betting odds and historical results. It aggregates odds from multiple bookmakers, calculates average odds and implied probabilities, compares them to historical outcomes, and provides statistical insights for teams and matches. The solution also supports email notifications and data normalization for team names.

---

## Features

- **Odds Aggregation:** Collects and averages odds from various bookmakers for each event.
- **Implied Probability Calculation:** Converts average odds into implied probabilities.
- **Historical Analysis:** Compares bookmaker probabilities to actual historical win rates.
- **Team Statistics:** Calculates wins, losses, home/away performance, and win rates for each team.
- **Name Normalization:** Standardizes team names using a mapping file (`AFL_teams.txt`).
- **Email Notifications:** Sends bet notifications via SMTP.
- **Extensible Data Sources:** Supports importing historical data from Excel files.

---

## Project Structure

- `WisdomOfCrowdBets/`  
  Main application, background worker, configuration, and entry point.
- `WisdomOfCrowndBets.Core/`  
  Core DTOs, interfaces, and services for data, analysis, and team statistics.
- `WisdomOfCrowndBets.Infrastructure/`  
  Infrastructure services (e.g., email, odds fetching).
- `WisdomOfCrowdBets/AFL_teams.txt`  
  Mapping file for normalizing team names.
- `appsettings.json`  
  Configuration for API keys, endpoints, and other settings.

---

## Key Components

- **Worker.cs:** Orchestrates data fetching, normalization, analysis, and notification.
- **EventAnalysis.cs:** Calculates average odds, implied probabilities, and win rates.
- **HistoricalTeamStatistics.cs:** Computes team-level statistics from historical data.
- **SendEmail.cs:** Handles sending email notifications.
- **DTOs:** Define data structures for events, teams, odds, and historical records.

---

## Setup & Usage

1. **Clone the repository.**
2. **Configure `appsettings.json`** with your API keys, endpoints, and SMTP settings.
3. **Ensure `AFL_teams.txt`** contains the correct team name mappings.
4. **Build the solution** using Visual Studio 2022 or `dotnet build`.
5. **Run the application**. The background worker will fetch odds, process data, and perform analysis automatically.

---

## Dependencies

- .NET 8
- [NPOI](https://github.com/nissl-lab/npoi) (for Excel file handling)
- [EPPlus](https://github.com/EPPlusSoftware/EPPlus) (if used for Excel)
- System.Net.Mail (for email notifications)

---

## Customization

- **Add new bookmakers:** Extend the odds fetching logic in the infrastructure layer.
- **Change team mappings:** Edit `AFL_teams.txt`.
- **Mod