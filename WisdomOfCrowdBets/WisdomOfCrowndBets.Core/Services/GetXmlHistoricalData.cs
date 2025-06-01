using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using WisdomOfCrowndBets.Core.DTO;
using WisdomOfCrowndBets.Core.Interfaces;

namespace WisdomOfCrowndBets.Core.Services
{
    public class GetXmlHistoricalData : IGetXlsxHistoricalData
    {
        public async Task<List<HistoricalDataXlsx>> GetExelData(Xlsx xlsx)
        {
            var data = new List<HistoricalDataXlsx>();
            if (string.IsNullOrWhiteSpace(xlsx?.XlsxPath))
            {
                Console.WriteLine("Error: XlsxPath is null or empty.");
                return data;
            }
            try
            {
                using (var fs = new FileStream(xlsx.XlsxPath, FileMode.Open, FileAccess.Read))
                {
                    var workbook = new XSSFWorkbook(fs);
                    var sheet = workbook.GetSheetAt(0);

                    if (sheet != null)
                    {
                        int[] columnIndices = { 0,2,3,5,6,12,13 };

                        var headerRow = sheet.GetRow(sheet.FirstRowNum+1);
                        string[] headers = columnIndices.Select(i => headerRow?.GetCell(i)?.ToString() ?? $"Column_{i + 1}").ToArray();

                        // Iterate rows, skipping header if present
                        for (int i = sheet.FirstRowNum + 2; i <= sheet.LastRowNum; i++)
                        {
                            var row = sheet.GetRow(i);
                            if (row == null) continue;

                            // Parse values
                            string? dateStr = row.GetCell(columnIndices[0])?.ToString();
                            string? homeTeam = row.GetCell(columnIndices[1])?.ToString();
                            string? awayTeam = row.GetCell(columnIndices[2])?.ToString();
                            string? homeScoreStr = row.GetCell(columnIndices[3])?.ToString();
                            string? awayScoreStr = row.GetCell(columnIndices[4])?.ToString();
                            string? homeOddsStr = row.GetCell(columnIndices[5])?.ToString();
                            string? awayOddsStr = row.GetCell(columnIndices[6])?.ToString();


                            if (!DateTime.TryParse(dateStr, out var date))
                                continue;
                            if (!decimal.TryParse(homeOddsStr, out var homeOdds))
                                homeOdds = 0;
                            if (!decimal.TryParse(awayOddsStr, out var awayOdds))
                                awayOdds = 0;
                            if (!int.TryParse(homeScoreStr, out var homeScore))
                                homeScore = 0;
                            if (!int.TryParse(awayScoreStr, out var awayScore))
                                awayScore = 0;
                            string winner = string.Empty;
                            string losser = string.Empty;

                            //no draw
                            if (homeScore < awayScore)
                            {
                                winner = "Home";
                                losser = "Away";
                            }
                            else 
                            {
                                winner = "Away";
                                losser = "Home";    
                            }

                            data.Add(new HistoricalDataXlsx
                                {
                                    Date = date,
                                    HomeTeam = homeTeam,
                                    AwayTeam = awayTeam,
                                    Winner = winner,
                                    Losser = losser,
                                    HomeOdds = homeOdds,
                                    AwayOdds = awayOdds
                                });
                        }
                    }
                    else
                    {
                        Console.WriteLine("Sheet not found.");
                    }
                }
                Console.WriteLine("XLSX file loaded and processed successfully using NPOI.");
                return data;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Error: File not found at {xlsx.XlsxPath}");
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return data;
            }
        }

    }
}
