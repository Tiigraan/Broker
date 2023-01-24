using System.ComponentModel.DataAnnotations;

namespace WebAPIPracticeProject.Data.Model;

public class File
{
    public int Id { get; set; }
    
    public byte[] Content { get; set; }
}