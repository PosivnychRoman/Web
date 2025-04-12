using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly AuthorService _authorService;
    private readonly BookService _bookService;

    public AuthorsController(AuthorService authorService, BookService bookService)
    {
        _authorService = authorService;
        _bookService = bookService;
    }

    [HttpGet]
    public ActionResult<List<Author>> Get() => _authorService.Get();

    [HttpGet("{id:length(24)}", Name = "GetAuthor")]
    public ActionResult<Author> Get(string id)
    {
        var author = _authorService.Get(id);
        if (author == null) return NotFound();
        return author;
    }

    [HttpPost]
    public ActionResult<Author> Create(Author author)
    {
        _authorService.Create(author);
        return CreatedAtRoute("GetAuthor", new { id = author.Id }, author);
    }

    [HttpPut("{id:length(24)}")]
    public IActionResult Update(string id, Author authorIn)
    {
        var author = _authorService.Get(id);
        if (author == null) return NotFound();
        _authorService.Update(id, authorIn);
        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public IActionResult Delete(string id)
    {
        var author = _authorService.Get(id);
        if (author == null) return NotFound();
        _authorService.Remove(id);
        return NoContent();
    }

    // 🔗 Вкладений запит
    [HttpGet("{id:length(24)}/books")]
    public ActionResult<List<Book>> GetBooksByAuthor(string id)
    {
        var author = _authorService.Get(id);
        if (author == null) return NotFound();

        var books = _bookService.Get().Where(b => b.AuthorId == id).ToList();
        return books;
    }
    [HttpGet("paged")]
    public ActionResult<List<Author>> GetPagedAuthors(int page = 1, int pageSize = 10, string? sortBy = "Name", bool descending = false)
    {
        var authors = _authorService.Get();

        authors = sortBy?.ToLower() switch
        {
            "name" => descending ? authors.OrderByDescending(a => a.Name).ToList() : authors.OrderBy(a => a.Name).ToList(),
            _ => authors.ToList()  
        };

        return authors
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

}
