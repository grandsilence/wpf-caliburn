using System;

namespace WpfCaliburn.Models
{
    public class TodoTask
    {
        public string Title { get; set; }
        public DateTime CreatedAt { get; } = DateTime.Now;

        public override string ToString()
        {
            return $"{Title} at {CreatedAt}";
        }
    }
}