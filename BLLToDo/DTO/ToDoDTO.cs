using System.ComponentModel.DataAnnotations;

namespace BLLToDo.DTO
{
    public class ToDoListModelDTO
    {
        [Key]
        public int Id { get; set; }
        public string Task { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Owner { get; set; }
    }
}
