﻿using Library.Core.DTO.Author;
using Library.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace Library.UI.Controllers
{
    [Route("authors")]
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        public IActionResult Authors()
        {
            return View();
        }

        [Route("add")]
        [HttpGet]
        public IActionResult AddAuthor()
        {

            return View();
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> AddAuthor(AuthorAddRequest authorAddRequest)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View(authorAddRequest);
            }

            var author = await _authorService.AddAuthorAsync(authorAddRequest);

            return Redirect($"{author.AuthorID}");
        }

        [Route("{authorID}")]
        public async Task<IActionResult> AuthorDetails(Guid? authorID)
        {
            if (authorID == null)
                return BadRequest();

            var author = await _authorService.GetAuthorByIdAsync(authorID.Value);
            if(author == null)
                return NotFound();
            
            return View(author);
        }

        [Route("{authorID}/delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? authorID)
        {
            if(authorID == null)
                return BadRequest();

            if(await _authorService.DeleteAuthorAsync(authorID.Value) == false)
                return Conflict("This author have books, please delete books before delete author");

            return Redirect("/");
        }
    }
}