using GRC.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GRC.Web.Models
{
    public class StandardViewModel
    {
        public Standard Standard { get; set; }

        public IEnumerable<SelectListItem> StandardCategories { get; set; }
    }
}
