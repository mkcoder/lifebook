using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using WebApplication2.Domain;
using WebApplication2.Services;

namespace WebApplication2
{
    public class IndexModel : PageModel
    {
        private readonly IFormRepository formRepository;
        public List<Form> Forms { get; set; }
        public IndexModel(IFormRepository formRepository)
        {
            this.formRepository = formRepository;
        }

        public void OnGet()
        {
            Forms = formRepository.GetAll();
        }
    }
}