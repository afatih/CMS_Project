﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EddarsCms.Entity.Entities
{
    public class Certificate : EntityBase
    {
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}