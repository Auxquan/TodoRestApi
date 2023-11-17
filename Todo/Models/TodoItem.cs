﻿namespace Todo.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Time { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }
        
    }
}
