﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EddarsCms.Entity.Entities
{
    public class Slider : EntityBase
    {
        public string Caption { get; set; }
        public string Url { get; set; }
        public string Image { get; set; }
        public int Number { get; set; }

    }
}