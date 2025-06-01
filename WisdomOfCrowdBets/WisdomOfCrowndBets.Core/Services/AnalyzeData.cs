using WisdomOfCrowndBets.Core.DTO;
using WisdomOfCrowndBets.Core.Interfaces;

namespace WisdomOfCrowndBets.Core.Services
{
    public class AnalyzeData:IAnalyzeData
    {
        public Task GetExelData(ApiOddsConfig apiGetOdds) 
        {
            List<HistoricalDataXlm> matches = new List<HistoricalDataXlm>();

            FileInfo file = new FileInfo(filePath);

            try
            {
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    // Assuming the data is in the first worksheet
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    if (worksheet == null)
                    {
                        Console.WriteLine($"Error: Worksheet not found in file: {filePath}");
                        return matches;
                    }

                    // Determine the number of rows with data (you might need to adjust this)
                    int rowCount = worksheet.Dimension?.Rows ?? 0;
                    if (rowCount <= 1) // Assuming the first row is the header
                    {
                        Console.WriteLine($"Warning: No data found in worksheet: {worksheet.Name}");
                        return matches;
                    }

                    // Assuming the first row contains the headers
                    Dictionary<string, int> headerColumns = new Dictionary<string, int>();
                    for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                    {
                        string header = worksheet.Cells[1, col].Value?.ToString()?.Trim(); // Trim to handle potential extra spaces
                        if (!string.IsNullOrEmpty(header))
                        {
                            headerColumns[header] = col;
                        }
                    }

                    // Try to find the columns based on common variations of the header names
                    int dateColumn = -1;
                    int homeTeamColumn = -1;
                    int awayTeamColumn = -1;
                    int homeOddsColumn = -1;
                    int awayOddsColumn = -1;

                    // Prioritize exact matches, then try variations
                    if (headerColumns.ContainsKey("Date")) dateColumn = headerColumns["Date"];
                    else if (headerColumns.ContainsKey("Kick Off Date (Local)")) dateColumn = headerColumns["Kick Off Date (Local)"];

                    if (headerColumns.ContainsKey("Home Team")) homeTeamColumn = headerColumns["Home Team"];
                    else if (headerColumns.ContainsKey("HomeTeam")) homeTeamColumn = headerColumns["HomeTeam"];
                    else if (headerColumns.ContainsKey("Home")) homeTeamColumn = headerColumns["Home"]; // Based on the image

                    if (headerColumns.ContainsKey("Away Team")) awayTeamColumn = headerColumns["Away Team"];
                    else if (headerColumns.ContainsKey("AwayTeam")) awayTeamColumn = headerColumns["AwayTeam"];
                    else if (headerColumns.ContainsKey("Away")) awayTeamColumn = headerColumns["Away"]; // Based on the image

                    // Look for "Home Odds" variations
                    foreach (var headerPair in headerColumns)
                    {
                        if (headerPair.Key.Contains("Home Odds") || headerPair.Key.StartsWith("Home", StringComparison.OrdinalIgnoreCase) && headerPair.Key.Contains("Odds"))
                        {
                            homeOddsColumn = headerPair.Value;
                            break; // Assuming the first match is the relevant one
                        }
                    }

                    // Look for "Away Odds" variations
                    foreach (var headerPair in headerColumns)
                    {
                        if (headerPair.Key.Contains("Away Odds") || headerPair.Key.StartsWith("Away", StringComparison.OrdinalIgnoreCase) && headerPair.Key.Contains("Odds"))
                        {
                            awayOddsColumn = headerPair.Value;
                            break; // Assuming the first match is the relevant one
                        }
                    }

                    // Check if all required columns were found
                    if (dateColumn == -1 || homeTeamColumn == -1 || awayTeamColumn == -1 || homeOddsColumn == -1 || awayOddsColumn == -1)
                    {
                        Console.WriteLine("Error: Could not find all the required columns (Date, Home Team, Away Team, Home Odds, Away Odds) in the Excel file.");
                        Console.WriteLine($"Found Headers: {string.Join(", ", headerColumns.Keys)}");
                        return matches;
                    }

                    // Read data starting from the second row
                    for (int row = 2; row <= rowCount; row++)
                    {
                        BettingMatch match = new BettingMatch();

                        try
                        {
                            var dateValue = worksheet.Cells[row, dateColumn].Value;
                            if (dateValue != null && DateTime.TryParse(dateValue.ToString(), out DateTime parsedDate))
                            {
                                match.Date = parsedDate;
                            }
                            else
                            {
                                Console.WriteLine($"Warning: Could not parse Date in row {row}. Skipping row.");
                                continue;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error parsing Date in row {row}: {ex.Message}. Skipping row.");
                            continue;
                        }

                        match.HomeTeam = worksheet.Cells[row, homeTeamColumn].Value?.ToString()?.Trim();
                        match.AwayTeam = worksheet.Cells[row, awayTeamColumn].Value?.ToString()?.Trim();

                        try
                        {
                            var homeOddsValue = worksheet.Cells[row, homeOddsColumn].Value;
                            if (homeOddsValue != null && decimal.TryParse(homeOddsValue.ToString(), out decimal parsedHomeOdds))
                            {
                                match.HomeOdds = parsedHomeOdds;
                            }
                            else
                            {
                                Console.WriteLine($"Warning: Could not parse Home Odds in row {row}. Setting to 0.");
                                match.HomeOdds = 0;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error parsing Home Odds in row {row}: {ex.Message}. Setting to 0.");
                            match.HomeOdds = 0;
                        }

                        try
                        {
                            var awayOddsValue = worksheet.Cells[row, awayOddsColumn].Value;
                            if (awayOddsValue != null && decimal.TryParse(awayOddsValue.ToString(), out decimal parsedAwayOdds))
                            {
                                match.AwayOdds = parsedAwayOdds;
                            }
                            else
                            {
                                Console.WriteLine($"Warning: Could not parse Away Odds in row {row}. Setting to 0.");
                                match.AwayOdds = 0;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error parsing Away Odds in row {row}: {ex.Message}. Setting to 0.");
                            match.AwayOdds = 0;
                        }

                        matches.Add(match);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Error: Excel file not found at path: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the Excel file: {ex.Message}");
            }

            return matches;
        }        

    }
}
