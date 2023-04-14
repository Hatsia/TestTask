﻿using System.Collections.Generic;

namespace TestTask.Models
{
    public class Team
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
