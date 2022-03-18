namespace ApplicationCore.Models;

public class MovieRequestModel
{
    public string Title { get; set; }
    public string Overview { get; set; }
    public string Tagline { get; set; }
    public decimal Budget { get; set; }
    public decimal Revenue { get; set; }
    public string ImdbUrl { get; set; }
    public string TmdbUrl { get; set; }
    public string PosterUrl { get; set; }
    public string BackdropUrl { get; set; }
    public string OriginalLanguage { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int RunTime { get; set; }
    public decimal Price { get; set; }
}