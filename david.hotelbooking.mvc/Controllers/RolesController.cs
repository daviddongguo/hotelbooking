using david.hotelbooking.domain.Concretes;
using david.hotelbooking.domain.Entities.RBAC;
using david.hotelbooking.domain.Services;
using david.hotelbooking.mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace david.hotelbooking.mvc.Controllers
{
    public class RolesController : Controller
    {
        private readonly UserDbContext _context;
        private readonly IUserService _service;

        public RolesController(UserDbContext context, IUserService service)
        {
            _context = context;
            _service = service;
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Roles.ToListAsync());
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _service.GetSingleRole(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Role role)
        {
            if (ModelState.IsValid)
            {
                _context.Add(role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _service.GetSingleRole(id);
            if (role == null)
            {
                return NotFound();
            }

            var permissions = (await _service.GetAllPermissions()).ToList();
            return View(new RolePermissionViewModel
            {
                Role = role,
                RoleId = role.Id,
                RoleName = role.Name,
                RoleDescription = role.Description,
                RolePermissionIds = role.RolePermissions.Select(r => r.Permission.Id).ToList(),
                Permissions = permissions
            });
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoleId,RoleName,RoleDescription,RolePermissionIds")] RolePermissionViewModel model)
        {
            if (id != model.RoleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateRole(new Role
                    {
                        Id = model.RoleId,
                        Name = model.RoleName,
                        Description = model.RoleDescription
                    });
                    await _service.UpdateRolePermissions(model.RoleId, model.RolePermissionIds);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(model.RoleId))
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

            var role = await _service.GetSingleRole(model.RoleId);
            return View(new RolePermissionViewModel
            {
                Role = role,
                RoleId = role.Id,
                RoleName = role.Name,
                RoleDescription = role.Description,
                RolePermissionIds = role.RolePermissions.Select(r => r.Permission.Id).ToList(),
                Permissions = (await _service.GetAllPermissions()).ToList()
            }); ;
        }

        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
