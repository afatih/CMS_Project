﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EddarsCms.Dto.BasicDtos
{
    public class PageDto:DtoBase
    {
        [Required(ErrorMessage = "Bu alanı doldurmanız zorunludur")]
        public string Caption { get; set; }
        [Required(ErrorMessage = "Bu alanı doldurmanız zorunludur")]
        public string Url { get; set; }


        public string Content { get; set; }

        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }

        public string ImageCover { get; set; }
        public string ImageBig { get; set; }

        public string Description { get; set; }


    }
}
