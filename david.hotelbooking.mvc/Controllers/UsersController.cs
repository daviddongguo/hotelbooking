using david.hotelbooking.domain.Entities.RBAC;
using david.hotelbooking.domain.Services;
using david.hotelbooking.mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace david.hotelbooking.mvc.Controllers
{
    public class UsersController : Controller
    {
        //private readonly UserDbContext _context;
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllUsers();
            return View(data.ToList());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _service.GetSingleUser(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Id = 0;
                await _service.AddOrUpdateUser(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _service.GetSingleUser(id);
            if (user == null)
            {
                return NotFound();
            }
            var roles = await _service.GetAllRoles();
            return View(new UserRolesViewModel
            {
                UserId = user.Id,
                UserEmail = user.Email,
                UserRoleIds = user.UserRoles.Select(u => u.RoleId).ToList(),
                Roles = roles.ToList()
            });
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId, UserRoleIds")] UserRolesViewModel model)
        {
            if (id != model.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateUserRoles(id, model.UserRoleIds);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(model.UserId))
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

            var user = await _service.GetSingleUser(id);
            var roles = await _service.GetAllRoles();
            return View(new UserRolesViewModel
            {
                UserId = user.Id,
                UserEmail = user.Email,
                UserRoleIds = user.UserRoles.Select(u => u.RoleId).ToList(),
                Roles = roles.ToList()
            });
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _service.GetSingleUser(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteUser(id);
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _service.GetSingleUser(id) != null;
        }
    }
}
