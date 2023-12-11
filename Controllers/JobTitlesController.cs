using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using e_commerce_backend.Models;
using Microsoft.DotNet.Scaffolding.Shared;

namespace e_commerce_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTitlesController : ControllerBase
    {
        private readonly EcommerceContext _context;

        public JobTitlesController(EcommerceContext context)
        {
            _context = context;
        }

        // GET: api/JobTitles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobTitle>>> GetJobTitles(bool? isDeleted = null)
        {
            if (_context.JobTitles == null)
            {
                return NotFound();
            }
            IQueryable<JobTitle> jobTitlesQuery = _context.JobTitles;
            if (isDeleted.HasValue)
            {
                // Filter by IsDeleted if the parameter is provided
                jobTitlesQuery = jobTitlesQuery.Where(c => c.IsDeleted == isDeleted.Value);
            }
            var jobTitles = await jobTitlesQuery.ToListAsync();
            return jobTitles;
        }

        // GET: api/JobTitles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobTitle>> GetJobTitle(int id)
        {
          if (_context.JobTitles == null)
          {
              return NotFound();
          }
            var jobTitle = await _context.JobTitles.FindAsync(id);

            if (jobTitle == null)
            {
                return NotFound();
            }

            return jobTitle;
        }

        // PUT: api/JobTitles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJobTitle(int id, JobTitle jobTitle)
        {
            if (id != jobTitle.Id)
            {
                return BadRequest();
            }

            _context.Entry(jobTitle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobTitleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/JobTitles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<JobTitle>> PostJobTitle(JobTitle jobTitle)
        {
          if (_context.JobTitles == null)
          {
              return Problem("Entity set 'EcommerceContext.JobTitles'  is null.");
          }
            _context.JobTitles.Add(jobTitle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJobTitle", new { id = jobTitle.Id }, jobTitle);
        }

        // DELETE: api/JobTitles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobTitle(int id)
        {
            if (_context.JobTitles == null)
            {
                return NotFound();
            }
            var jobTitle = await _context.JobTitles.FindAsync(id);
            if (jobTitle == null)
            {
                return NotFound();
            }

            jobTitle.IsDeleted = true;
            //_context.JobTitles.Remove(jobTitle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/JobTitles/Restore/5
        [HttpPut("Restore/{id}")]
        public async Task<IActionResult> RestoreJobTitle(int id)
        {
            if (_context.JobTitles == null)
            {
                return NotFound();
            }

            var jobTitle = await _context.JobTitles.FindAsync(id);

            if (jobTitle == null)
            {
                return NotFound();
            }

            // Restore the jobTitle by setting IsDeleted to false
            jobTitle.IsDeleted = false;

            await _context.SaveChangesAsync();

            return Ok(jobTitle);
        }

        private bool JobTitleExists(int id)
        {
            return (_context.JobTitles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
