﻿using System.ComponentModel.DataAnnotations;

namespace Backend.Domain
{
	public abstract class BaseEntity
	{
        [Key]
        public int Id { get; set; }
    }
}

