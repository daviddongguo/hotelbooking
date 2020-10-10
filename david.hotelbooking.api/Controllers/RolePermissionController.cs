using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using david.hotelbooking.domain.Entities;
using david.hotelbooking.domain.Entities.RBAC;
using david.hotelbooking.domain.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace david.hotelbooking.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionController : ControllerBase
    {
        private readonly IUserService _service;

        public RolePermissionController(IUserService service)
        {
            _service = service;
        }
        // GET: api/<RolePermissionController>
        [HttpGet]
        public async Task<ServiceResponse<List<RolePermission>>> GetAll()
        {
            var response = new ServiceResponse<List<RolePermission>>();
            try
            {
                response.Data = (await _service.GetAllRolePermissions()).ToList();

            }
            catch (System.Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        // GET api/<RolePermissionController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RolePermissionController>
        [HttpPost]
        public async Task<IActionResult> AddRolePermission([FromBody] RolePermission newRolePermission)
        {
            ServiceResponse<RolePermission> response = new ServiceResponse<RolePermission>();
            try
            {
                var role = await _service.GetSingleRole(newRolePermission.RoleId);
                if (role == null)
                {
                    response.Success = false;
                    response.Message = $"Role(id={newRolePermission.RoleId}) not found.";
                    NotFound(response);
                }
                var permission = await _service.GetSinglePermission(newRolePermission.PermissionId);
                if (permission == null)
                {
                    response.Success = false;
                    response.Message = $"Permission(id={newRolePermission.PermissionId}) not found.";
                    NotFound(response);
                }
                var rolePermission = new RolePermission
                {
                    Role = role,
                    Permission = permission
                };
                await _service.AddRolePermission(rolePermission);
                response.Data = rolePermission;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        // PUT api/<RolePermissionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RolePermissionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
