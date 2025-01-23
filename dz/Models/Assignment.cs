namespace dz;

public class Assignment
{
    public int Id { get; set; }
    public int ClassId { get; set; }
    public string Subject { get; set; } = ""; 
    public string Message { get; set; } = "";
    public DateTime Datetime { get; set; } = DateTime.UtcNow;
    public string PathOfFile { get; set; } = "";
}