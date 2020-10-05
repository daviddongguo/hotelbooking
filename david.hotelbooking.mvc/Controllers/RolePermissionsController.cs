using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using david.hotelbooking.domain.Concretes;
using david.hotelbooking.domain.Entities.RBAC;

namespace david.hotelbooking.mvc.Controllers
{
    public class RolePermissionsController : Controller
    {
        private readonly EFDbContext _context;

        public RolePermissionsController(EFDbContext context)
        {
            _context = context;
        }

        // GET: RolePermissions
        public async Task<IActionResult> Index()
        {
            var eFDbContext = _context.RolePermissions.Include(r => r.Permission).Include(r => r.Role);
            return View(await eFDbContext.ToListAsync());
        }

        // GET: RolePermissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rolePermission = await _context.RolePermissions
                .Include(r => r.Permission)
                .Include(r => r.Role)
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (rolePermission == null)
            {
                return NotFound();
            }

            return View(rolePermission);
        }

        // GET: RolePermissions/Create
        public IActionResult Create()
        {
            ViewData["PermissionId"] = new SelectList(_context.Permissions, "Id", "Id");
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id");
            return View();
        }

        // POST: RolePermissions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoleId,PermissionId")] RolePermission rolePermission)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rolePermission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PermissionId"] = new SelectList(_context.Permissions, "Id", "Id", rolePermission.PermissionId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", rolePermission.RoleId);
            return View(rolePermission);
        }

        // GET: RolePermissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rolePermission = await _context.RolePermissions.FindAsync(id);
            if (rolePermission == null)
            {
                return NotFound();
            }
            ViewData["PermissionId"] = new SelectList(_context.Permissions, "Id", "Id", rolePermission.PermissionId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", rolePermission.RoleId);
            return View(rolePermission);
        }

        // POST: RolePermissions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoleId,PermissionId")] RolePermission rolePermission)
        {
            if (id != rolePermission.RoleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rolePermission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RolePermissionExists(rolePermission.RoleId))
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
            ViewData["PermissionId"] = new SelectList(_context.Permissions, "Id", "Id", rolePermission.PermissionId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", rolePermission.RoleId);
            return View(rolePermission);
        }

        // GET: RolePermissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rolePermission = await _context.RolePermissions
                .Include(r => r.Permission)
                .Include(r => r.Role)
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (rolePermission == null)
            {
                return NotFound();
            }

            return View(rolePermission);
        }

        // POST: RolePermissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rolePermission = await _context.RolePermissions.FindAsync(id);
            _context.RolePermissions.Remove(rolePermission);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RolePermissionExists(int id)
        {
            return _context.RolePermissions.Any(e => e.RoleId == id);
        }
    }
}
