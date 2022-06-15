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
    public class IndexModel : PageModel
    {
        private readonly PgJobs.Data.JobDbContext _context;

        public IndexModel(PgJobs.Data.JobDbContext context)
        {
            _context = context;
        }

        public IList<Job> Job { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Jobs != null)
            {
                Job = await _context.Jobs.ToListAsync();
            }
        }
    }
}
