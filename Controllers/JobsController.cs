﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPTJobMatch.Models;
using Microsoft.AspNetCore.Authorization;

namespace FPTJobMatch.Controllers
{
    public class JobsController : Controller
    {
        private readonly DB1670Context _context;

        public JobsController(DB1670Context context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin, Employer")]
        // GET: Jobs
        public async Task<IActionResult> Index()
        {
            var dB1670Context = _context.Jobs.Include(j => j.ObjCategory);
            return View(await dB1670Context.ToListAsync());
        }
        [Authorize(Roles = "Admin, Employer, Job seeker")]
		public async Task<IActionResult> ListJob()
		{
			var dB1670Context = _context.Jobs.Include(j => j.ObjCategory).Where(j=>j.Deadline >= DateTime.Now);
			return View(await dB1670Context.ToListAsync());
		}
        [Authorize(Roles = "Admin, Employer, Job seeker")]
        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Jobs == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs.Include(j => j.ObjCategory).FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            var proJob = _context.ProJob.Include(p => p.ObjProfile).Where(p => p.JobId == id);

            var profile = _context.Profile.Where(p => p.UserId == User.Identity.Name).FirstOrDefault();

            if (proJob.Where(p => p.ProfileId == profile.Id).Count() > 0 && proJob.Count() > 0)
            {
                ViewBag.Apply = true;
            }
            else
            {
                ViewBag.Apply = false;
            }

            return View(job);
        }
        [Authorize(Roles = "Admin, Employer")]
        // GET: Jobs/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }
        // POST: Jobs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Qualification,Location,Industry,Deadline,CategoryId")] Job job)
        {
            if (ModelState.IsValid)
            {
                _context.Add(job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", job.CategoryId);
            return View(job);
        }
        [Authorize(Roles = "Admin, Employer")]
        // GET: Jobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", job.CategoryId);
            return View(job);
        }
        // POST: Jobs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Qualification,Location,Industry,Deadline,CategoryId")] Job job)
        {
            if (id != job.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(job.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", job.CategoryId);
            return View(job);
        }
        [Authorize(Roles = "Admin, Employer")]
        // GET: Jobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs
                .Include(j => j.ObjCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }
        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job != null)
            {
                _context.Jobs.Remove(job);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool JobExists(int id)
        {
            return _context.Jobs.Any(e => e.Id == id);
        }
    }
}
