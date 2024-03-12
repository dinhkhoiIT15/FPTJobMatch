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
    public class ProJobsController : Controller
    {
        private readonly DB1670Context _context;
        public ProJobsController(DB1670Context context)
        {
            _context = context;
        }
        // GET: ProJobs
        public async Task<IActionResult> Index(int id)
        {
            var dB1670Context = _context.ProJob.Include(p => p.ObjJob).Include(p => p.ObjProfile).Where(p => p.JobId == id);
            return View(await dB1670Context.ToListAsync());
        }
        // GET: ProJobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proJob = await _context.ProJob
                .Include(p => p.ObjJob)
                .Include(p => p.ObjProfile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proJob == null)
            {
                return NotFound();
            }

            return View(proJob);
        }
        // GET: ProJobs/Create
        public IActionResult Create(int id)
        {
            ProJob pj = new ProJob();
            pj.JobId = id;
            pj.ReDate = DateTime.Now;
            pj.ProfileId = _context.Profile.Where(p => p.UserId == User.Identity.Name).FirstOrDefault().Id;
                _context.Add(pj);
                _context.SaveChanges();

            return RedirectToAction("ListJob", "Jobs");
        }
        // POST: ProJobs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReDate,ProfileId,JobId")] ProJob proJob)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proJob);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "Id", proJob.JobId);
            ViewData["ProfileId"] = new SelectList(_context.Profile, "Id", "Id", proJob.ProfileId);
            return View(proJob);
        }
        // GET: ProJobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proJob = await _context.ProJob.FindAsync(id);
            if (proJob == null)
            {
                return NotFound();
            }
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "Id", proJob.JobId);
            ViewData["ProfileId"] = new SelectList(_context.Profile, "Id", "Id", proJob.ProfileId);
            return View(proJob);
        }
        // POST: ProJobs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReDate,ProfileId,JobId")] ProJob proJob)
        {
            if (id != proJob.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proJob);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProJobExists(proJob.Id))
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
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "Id", proJob.JobId);
            ViewData["ProfileId"] = new SelectList(_context.Profile, "Id", "Id", proJob.ProfileId);
            return View(proJob);
        }
        // GET: ProJobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proJob = await _context.ProJob
                .Include(p => p.ObjJob)
                .Include(p => p.ObjProfile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proJob == null)
            {
                return NotFound();
            }

            return View(proJob);
        }
        // POST: ProJobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proJob = await _context.ProJob.FindAsync(id);
            if (proJob != null)
            {
                _context.ProJob.Remove(proJob);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ProJobExists(int id)
        {
            return _context.ProJob.Any(e => e.Id == id);
        }
    }
}
