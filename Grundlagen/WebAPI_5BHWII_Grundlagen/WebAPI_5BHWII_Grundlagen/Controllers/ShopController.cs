﻿using Klassenbiliothek_Article;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using WebAPI_5BHWII_Grundlagen.Models.DB;

namespace WebAPI_5BHWII_Grundlagen.Controllers
{
    /*
        Vorbereitungen

        - EF Core installieren (EF Core, EF Core Tools, Pomelo)
        - Klasse Article (ArticleId, Name, Price, ReleaseDate)
        - DBManager programmieren
        - Migrationen erzeugen 
        - ca. 3 Artikel direkt in MySQL eintragen

        WebAPI
        - 

    */



    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {

        //vorbereitung (DB) - DI (Dependency Injection)
        private DBManager _dbManager;

        //hier wird vom DI-Container die von ihm erzeugte Instanz an den ctor übergeben und wir können auf die 
        //DB zugreifen.
        public ShopController(DBManager dbManager)
        {
            this._dbManager = dbManager;
        }

        public async Task<bool> CheckApiKey(string apiKey)
        {
            var user = await _dbManager.Users.FirstOrDefaultAsync(u => u.ApiKey == apiKey);
            if(user == null){
                return false;
            }
            return true;
        }

        [HttpGet("articles")] 
        public async Task<IActionResult> GetArticles(string apiKey)
        {   
            // 1. API Key überprüfen
            // 2. Wenn der API Key nicht stimmt, dann Fehlermeldung zurückgeben
            // 3. Wenn der API Key stimmt, dann alle Artikel aus der DB holen und zurückgeben
            // 4. Artikel in Json umwandeln und zurückgeben
            if(!await CheckApiKey(apiKey)){
                return BadRequest("API Key not valid");
            }
            return new JsonResult(await _dbManager.Articles.ToListAsync());
        }

        [HttpGet("article/{id:int}")]
        public async Task<IActionResult> GetArticle(int id, string apiKey)
        {
            // 1. Artikel mit der ID aus der DB holen
            // 2. Artikel nach Json umwandeln und zurückliefern
            // 3. API Key überprüfen
            if(!await CheckApiKey(apiKey)){
                return BadRequest("API Key not valid");
            }
            return new JsonResult(await _dbManager.Articles.FindAsync(id));
        }

        //einen artikel löschen per ID
        [HttpDelete("article/{id:int}")]
        public async Task<IActionResult> DelArticle(int id, string apiKey){
            if(!await CheckApiKey(apiKey)){
                return BadRequest("API Key not valid");
            }
            var articleToDel = await _dbManager.Articles.FindAsync(id);
            var erfolg = _dbManager.Articles.Remove(articleToDel);
            await _dbManager.SaveChangesAsync();
            return new JsonResult(erfolg);
        }
        //einen neuen Artikel erzeugen 
        [HttpPost("article")]
        public async Task<IActionResult> AddArticle(Article article, string apiKey){
            if(!await CheckApiKey(apiKey)){
                return BadRequest("API Key not valid");
            }
            var erfolg = await _dbManager.Articles.AddAsync(article);
            await _dbManager.SaveChangesAsync();
            return new JsonResult(erfolg);
        }

        //einen anderen Artikel ändern 
        [HttpPut("article")]
        public async Task<IActionResult> UpdateArticle(Article article, string apiKey){
            if(!await CheckApiKey(apiKey)){
                return BadRequest("API Key not valid");
            }
            //var articleToUpdate = 
            var articleToUpdate = await _dbManager.Articles.FindAsync(article.ArticleId);
            articleToUpdate.Name = article.Name;
            articleToUpdate.Price = article.Price;
            articleToUpdate.ReleaseDate = article.ReleaseDate;
            var erfolg = _dbManager.Articles.Update(articleToUpdate);
            await _dbManager.SaveChangesAsync();
            return new JsonResult(erfolg);
        }


    }
}
