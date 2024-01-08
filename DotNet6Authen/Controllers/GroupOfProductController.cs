using DemoWebApiDotNet6.DTO.Auth;
using DotNet6Authen.DTO;
using DotNet6Authen.Entities;
using DotNet6Authen.Service.GroupOfPorducts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace DotNet6Authen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupOfProductController : ControllerBase
    {
        private readonly IGroupOfProductService _gopService;
        private readonly IConfiguration _configuration;

        public GroupOfProductController(IGroupOfProductService gopService, IConfiguration configuration)
        {
            _gopService = gopService;
            _configuration = configuration;
        }

        [HttpGet("GroupOfProduct_GetAll"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GroupOfProduct_GetAll()
        {
            try
            {
                return Ok(await _gopService.GroupOfProduct_GetAll());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GroupOfProduct_Get"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GroupOfProduct_Get(dynamic dynParam)
        {
            var obj = JsonConvert.DeserializeObject<object>(dynParam.ToString());
            int id = (int)obj.id;
            var gop = await _gopService.GroupOfProduct_Get(id);
            return gop != null ? Ok(gop) : BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> GroupOfProduct_Add(GroupOfProductDTO item)
        {
            try
            {
                await _gopService.GroupOfProduct_Add(item);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> GroupOfProduct_Update(int id, GroupOfProductDTO item)
        {
            try
            {
                await _gopService.GroupOfProduct_Update(id, item);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> GroupOfProduct_Delete(int id)
        {
            try
            {
                await _gopService.GroupOfProduct_Delete(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
