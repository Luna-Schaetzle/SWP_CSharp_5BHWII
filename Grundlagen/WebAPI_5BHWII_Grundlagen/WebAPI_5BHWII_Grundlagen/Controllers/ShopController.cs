using Klassenbiliothek_Article;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_5BHWII_Grundlagen.Models.DB;

namespace WebAPI_5BHWII_Grundlagen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly DBManager _dbManager;

        public ShopController(DBManager dbManager)
        {
            _dbManager = dbManager;
        }

        private async Task<bool> CheckApiKey(string apiKey)
        {
            var user = await _dbManager.Users.FirstOrDefaultAsync(u => u.ApiKey == apiKey);
            return user != null;
        }

        //###################
        // Article Funktionen
        //###################

        [HttpGet("articles")]
        public async Task<IActionResult> GetArticles(string apiKey)
        {
            if (!await CheckApiKey(apiKey))
            {
                return BadRequest("API Key not valid");
            }

            var articles = await _dbManager.Articles.ToListAsync();
            return Ok(articles);
        }

        [HttpGet("article/{id:int}")]
        public async Task<IActionResult> GetArticle(int id, string apiKey)
        {
            if (!await CheckApiKey(apiKey))
            {
                return BadRequest("API Key not valid");
            }

            var article = await _dbManager.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound("Article not found.");
            }

            return Ok(article);
        }

        [HttpPost("article")]
        public async Task<IActionResult> AddArticle(Article article, string apiKey)
        {
            if (!await CheckApiKey(apiKey))
            {
                return BadRequest("API Key not valid");
            }

            if (article == null)
            {
                return BadRequest("Invalid article data.");
            }

            await _dbManager.Articles.AddAsync(article);
            await _dbManager.SaveChangesAsync();
            return Ok("Article added successfully.");
        }

        [HttpPut("article")]
        public async Task<IActionResult> UpdateArticle(Article article, string apiKey)
        {
            if (!await CheckApiKey(apiKey))
            {
                return BadRequest("API Key not valid");
            }

            var existingArticle = await _dbManager.Articles.FindAsync(article.ArticleId);
            if (existingArticle == null)
            {
                return NotFound("Article not found.");
            }

            existingArticle.Name = article.Name;
            existingArticle.Price = article.Price;
            existingArticle.ReleaseDate = article.ReleaseDate;

            _dbManager.Articles.Update(existingArticle);
            await _dbManager.SaveChangesAsync();
            return Ok("Article updated successfully.");
        }

        [HttpDelete("article/{id:int}")]
        public async Task<IActionResult> DelArticle(int id, string apiKey)
        {
            if (!await CheckApiKey(apiKey))
            {
                return BadRequest("API Key not valid");
            }

            var article = await _dbManager.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound("Article not found.");
            }

            _dbManager.Articles.Remove(article);
            await _dbManager.SaveChangesAsync();
            return Ok("Article deleted successfully.");
        }

        //###################
        // Basket Funktionen
        //###################

        [HttpGet("basket/{userId:int}")]
        public async Task<IActionResult> GetBasket(int userId, string apiKey)
        {
            if (!await CheckApiKey(apiKey))
            {
                return BadRequest("API Key not valid");
            }

            var baskets = await _dbManager.Baskets
                .Include(b => b.article)
                .Where(b => b.user.Id == userId)
                .ToListAsync();

            if (!baskets.Any())
            {
                return NotFound("No articles found in the basket for the given user.");
            }

            return Ok(baskets);
        }

        [HttpPost("basket")]
        public async Task<IActionResult> AddToBasket([FromBody] Basket basket, string apiKey)
        {
            if (!await CheckApiKey(apiKey))
            {
                return BadRequest("API Key not valid");
            }

            if (basket == null || basket.user == null || basket.article == null)
            {
                return BadRequest("Invalid basket data provided.");
            }

            var existingBasket = await _dbManager.Baskets
                .FirstOrDefaultAsync(b => b.user.Id == basket.user.Id && b.article.ArticleId == basket.article.ArticleId);

            if (existingBasket != null)
            {
                return Conflict("The article is already in the basket.");
            }

            await _dbManager.Baskets.AddAsync(basket);
            await _dbManager.SaveChangesAsync();
            return Ok("Article added to the basket successfully.");
        }

        [HttpDelete("basket/{userId:int}/{articleId:int}")]
        public async Task<IActionResult> DelFromBasket(int userId, int articleId, string apiKey)
        {
            if (!await CheckApiKey(apiKey))
            {
                return BadRequest("API Key not valid");
            }

            var basket = await _dbManager.Baskets
                .FirstOrDefaultAsync(b => b.user.Id == userId && b.article.ArticleId == articleId);

            if (basket == null)
            {
                return NotFound("Article not found in the basket for the given user.");
            }

            _dbManager.Baskets.Remove(basket);
            await _dbManager.SaveChangesAsync();
            return Ok("Article removed from the basket successfully.");
        }

        [HttpPost("basket/buy/{userId:int}")]
        public async Task<IActionResult> BuyBasket(int userId, string apiKey)
        {
            if (!await CheckApiKey(apiKey))
            {
                return BadRequest("API Key not valid");
            }

            var baskets = await _dbManager.Baskets
                .Where(b => b.user.Id == userId)
                .ToListAsync();

            if (!baskets.Any())
            {
                return NotFound("No articles found in the basket to purchase.");
            }

            _dbManager.Baskets.RemoveRange(baskets);
            await _dbManager.SaveChangesAsync();
            return Ok("Basket purchased successfully.");
        }
    }
}
