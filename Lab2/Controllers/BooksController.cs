using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly BookService _bookService;

    public BooksController(BookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public ActionResult<List<Book>> Get() => _bookService.Get();

    [HttpGet("{id:length(24)}", Name = "GetBook")]
    public ActionResult<Book> Get(string id)
    {
        var book = _bookService.Get(id);
        if (book == null) return NotFound();
        return book;
    }

    [HttpPost]
    public ActionResult<Book> Create(Book book)
    {
        _bookService.Create(book);
        return CreatedAtRoute("GetBook", new { id = book.Id.ToString() }, book);
    }

    [HttpPut("{id:length(24)}")]
    public IActionResult Update(string id, Book bookIn)
    {
        var book = _bookService.Get(id);
        if (book == null) return NotFound();
        _bookService.Update(id, bookIn);
        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public IActionResult Delete(string id)
    {
        var book = _bookService.Get(id);
        if (book == null) return NotFound();
        _bookService.Remove(id);
        return NoContent();
    }
    [HttpGet("paged")]
    public ActionResult<List<Book>> GetPagedBooks(int page = 1, int pageSize = 10, string? sortBy = "Title", bool descending = false)
    {
        var books = _bookService.Get();

        books = sortBy?.ToLower() switch
        {
            "title" => descending ? books.OrderByDescending(b => b.Title).ToList() : books.OrderBy(b => b.Title).ToList(),
            "authorid" => descending ? books.OrderByDescending(b => b.AuthorId).ToList() : books.OrderBy(b => b.AuthorId).ToList(),
            "genreid" => descending ? books.OrderByDescending(b => b.GenreId).ToList() : books.OrderBy(b => b.GenreId).ToList(),
            _ => books.ToList()  
        };


        return books
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

}
