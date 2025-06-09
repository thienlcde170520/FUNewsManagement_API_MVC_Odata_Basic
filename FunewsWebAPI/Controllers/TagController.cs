using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using DataAccessObjects;
using AutoMapper;
using FUnewsDTO;
using Microsoft.AspNetCore.Authorization;
using Repositories.Interfaces;

namespace FunewsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagRepository _tagRepo;
        private readonly IMapper _mapper;

        public TagController(ITagRepository tagRepo, IMapper mapper)
        {
            _tagRepo = tagRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _tagRepo.GetAllTags();
            var result = _mapper.Map<IEnumerable<TagDTO>>(accounts);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(short id)
        {
            var account = await _tagRepo.GetTagById(id);
            if (account == null) return NotFound();
            return Ok(account);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Tag dto)
        {
            await _tagRepo.Add(dto);
            return Content("Insert success!");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Update(short id, Tag dto)
        {
            if (id != dto.TagId) return BadRequest();

            var existing = await _tagRepo.GetTagById(id);
            if (existing == null) return NotFound();

            await _tagRepo.Update(dto);
            return Content("Update success!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(short id)
        {
            var account = await _tagRepo.GetTagById(id);
            if (account == null) return NotFound();

            

            await _tagRepo.Delete(id);
            return Content("Delete success!");
        }
    }
}
