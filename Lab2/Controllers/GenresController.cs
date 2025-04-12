using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class GenresController : ControllerBase
{
    private readonly GenreService _genreService;
    private readonly BookService _bookService;

    public GenresController(GenreService genreService, BookService bookService)
    {
        _genreService = genreService;
        _bookService = bookService;
    }

    [HttpGet]
    public ActionResult<List<Genre>> Get() => _genreService.Get();

    [HttpGet("{id:length(24)}", Name = "GetGenre")]
    public ActionResult<Genre> Get(string id)
    {
        var genre = _genreService.Get(id);
        if (genre == null) return NotFound();
        return genre;
    }

    [HttpPost]
    public ActionResult<Genre> Create(Genre genre)
    {
        _genreService.Create(genre);
        return CreatedAtRoute("GetGenre", new { id = genre.Id }, genre);
    }

    [HttpPut("{id:length(24)}")]
    public IActionResult Update(string id, Genre genreIn)
    {
        var genre = _genreService.Get(id);
        if (genre == null) return NotFound();
        _genreService.Update(id, genreIn);
        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public IActionResult Delete(string id)
    {
        var genre = _genreService.Get(id);
        if (genre == null) return NotFound();
        _genreService.Remove(id);
        return NoContent();
    }

    // 🔗 Вкладений запит
    [HttpGet("{id:length(24)}/books")]
    public ActionResult<List<Book>> GetBooksByGenre(string id)
    {
        var genre = _genreService.Get(id);
        if (genre == null) return NotFound();

        var books = _bookService.Get().Where(b => b.GenreId == id).ToList();
        return books;
    }
    [HttpGet("paged")]
    public ActionResult<List<Genre>> GetPagedGenres(int page = 1, int pageSize = 10, string? sortBy = "Name", bool descending = false)
    {
        var genres = _genreService.Get();

        genres = sortBy?.ToLower() switch
        {
            "name" => descending ? genres.OrderByDescending(g => g.Name).ToList() : genres.OrderBy(g => g.Name).ToList(),
            _ => genres.ToList()  
        };

        return genres
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

}
