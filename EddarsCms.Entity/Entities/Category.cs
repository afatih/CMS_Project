﻿using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EddarsCms.Entity.Entities
{
    public class Category:EntityBase
    {
        public string Name { get; set; }
        public int MainCatId { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string ImageSmall { get; set; }
        public string ImageBig { get; set; }
        public string Video1 { get; set; }
        public string Video2 { get; set; }
        public string Video3 { get; set; }

        public string Description { get; set; }
    }
}
