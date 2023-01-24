using System.ComponentModel.DataAnnotations;

namespace WebAPIPracticeProject.Data.Model;

public class Package
{
    public int Id { get; set; }

    public string Content { get; set; }

    public bool Sent { get; set; }
    
    public int FileId { get; set; }
}

