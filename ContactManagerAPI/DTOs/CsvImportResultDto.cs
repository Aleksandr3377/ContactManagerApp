namespace ContactManagerAPI.DTOs;

public class CsvImportResultDto
{
    public int Imported { get; set; }
    public List<string> Errors { get; set; } = new();
}