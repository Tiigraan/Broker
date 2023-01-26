using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPIPracticeProject.Data.Model;

public class Package
{
    public int Id { get; set; }

    [Required]
    public string Content { get; set; }

    public Status Sent { get; set; }

    public int FileId { get; set; }
}

