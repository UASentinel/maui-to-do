﻿using System.ComponentModel.DataAnnotations;

namespace ToDoApi.Models;

public class ToDo
{
    [Key]
    public int Id { get; set; }
    public string? Text { get; set; }
}
