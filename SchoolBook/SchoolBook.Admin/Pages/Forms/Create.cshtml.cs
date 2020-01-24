using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication2.Domain;
using WebApplication2.Services;

namespace WebApplication2
{
    public class CreateModel : PageModel
    {
        private readonly IFormRepository formRepository;
        public CreateModel(IFormRepository formRepository)
        {
            this.formRepository = formRepository;
        }

        public PageResult OnGet()
        {
            return Page();
        }

        public PageResult OnPost(Form form)
        {
            formRepository.Add(form);
            return Page();
        }
    }
}