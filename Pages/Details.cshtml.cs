using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PgJobs.Data;
using PgJobs.Models;

namespace PgJobs.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly PgJobs.Data.JobDbContext _context;

        public DetailsModel(PgJobs.Data.JobDbContext context)
        {
            _context = context;
        }

      public Job Job { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Jobs == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs.FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }
            else 
            {
                Job = job;
            }
            return Page();
        }
    }
}
