using Library.Core.DTO.Author;
using Library.Core.ServiceContracts;
using Library.Core.Services;
using Library.Infrastructure.DbContext;
using Microsoft.AspNetCore.Mvc;

namespace Library.UI.Controllers
{
    [Route("authors")]
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;
        private readonly IAuthorImageService _authorImageService;

        public AuthorController(IAuthorService authorService, IAuthorImageService authorImageService)
        {
            _authorService = authorService;
            _authorImageService = authorImageService;
        }

        [HttpGet]
        public async Task<IActionResult> AuthorCatalog()
        {
            var authors = await _authorService.GetAllAuthorsAsync();
            return View(authors);
        }

        [Route("add")]
        [HttpGet]
        public IActionResult AddAuthor()
        {
            return View();
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> AddAuthor(AuthorAddRequest? authorAddRequest)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View(authorAddRequest);
            }

            var author = await _authorService.AddAuthorAsync(authorAddRequest);

            Guid authorId = author.AuthorID;
            var newImage = authorAddRequest.Image;

            if (newImage != null)
            {
                var addImage = await _authorImageService.AddImageAsync(newImage, authorId);
                if (addImage == null)
                    return StatusCode(500);
            }



            return Redirect($"{author.AuthorID}");
        }

        [Route("{authorID}")]
        public async Task<IActionResult> AuthorDetails(Guid? authorID)
        {
            if (authorID == null)
                return BadRequest();

            var author = await _authorService.GetAuthorByIdAsync(authorID.Value);
            if (author == null)
                return NotFound();

            return View(author);
        }

        [Route("{authorID}/update")]
        [HttpGet]
        public async Task<IActionResult> UpdateAuthor(Guid? authorID)
        {
            if (authorID == null)
                return BadRequest();

            var author = await _authorService.GetAuthorByIdAsync(authorID.Value);
            if (author == null)
                return NotFound();

            ViewBag.Biography = author.Biography;
            ViewBag.Image = "authorImages/" + author?.AuthorImages?.First().ImageName;

            return View();
        }

        [Route("{authorID}/update")]
        [HttpPost]
        public async Task<IActionResult> UpdateAuthor(AuthorUpdateRequest? authorUpdateRequest, Guid? authorId)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                ViewBag.Biography = authorUpdateRequest?.Biography;
                return View();
            }

            if (authorId == null)
                return BadRequest();

            bool sassesSaveImage = false;
            var newImage = authorUpdateRequest.Image;
            if (newImage == null)
            {
                sassesSaveImage = await _authorImageService.DeleteImagesByUserIdAsync(authorId);
            }
            else
            {
                var image = await _authorImageService.GetImagesByAuthorIdAsync(authorId);
                if (image == null || image.Count() == 0)
                {
                    var updatedImage = await _authorImageService.AddImageAsync(newImage, authorId);
                    if (updatedImage != null)
                        sassesSaveImage = true;
                }
                else
                {
                    sassesSaveImage = await _authorImageService.UpdateImageAsync(authorId, newImage);
                }
            }

            var author = await _authorService.UpdateAuthorAsync(authorUpdateRequest, authorId.Value);
            if (sassesSaveImage == false || author == null)
            {
                return StatusCode(500);
            }

            return Redirect($"/authors/{author.AuthorID}");
        }

        [Route("{authorID}/delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? authorID)
        {
            if (authorID == null)
                return BadRequest();

            if (await _authorService.DeleteAuthorAsync(authorID.Value) == false)
                return Conflict("This author have books, please delete books before delete author");

            return Redirect("/");
        }
    }
}
