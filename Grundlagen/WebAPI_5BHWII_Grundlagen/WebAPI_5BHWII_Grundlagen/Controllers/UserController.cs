using Klassenbiliothek_Article;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using WebAPI_5BHWII_Grundlagen.Models.DB;



namespace WebAPI_5BHWII_Grundlagen.Controllers
{
    


    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {

        //vorbereitung (DB) - DI (Dependency Injection)
        private DBManager _dbManager;

        //hier wird vom DI-Container die von ihm erzeugte Instanz an den ctor übergeben und wir können auf die 
        //DB zugreifen.
        public UserController(DBManager dbManager)
        {
            this._dbManager = dbManager;
        }

        // Wir schicken die Regestrierungsdaten an die API und bekommen den API Key zurück
        [HttpPost("Register")]
        public IActionResult Register(User user)
        {
            //Überprüfen ob der User schon existiert
            if (_dbManager.Users.Any(u => u.Email == user.Email))
            {
                return BadRequest("User already exists");
            }

            //API Key generieren
            user.ApiKey = Guid.NewGuid().ToString();

            //User in DB speichern
            _dbManager.Users.Add(user);
            _dbManager.SaveChanges();

            return Ok(user.ApiKey);
        }


        // Überprüfen ob der User existiert und den API Key zurückgeben
        //Wird nicht Verwendet 
        [HttpGet("CheckAPIKey")]
        public IActionResult CheckAPIKey(string email, string password)
        {
            User user = _dbManager.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                return BadRequest("Email or Password is wrong");
            }

            return Ok(user.ApiKey);
        }

       

    }
}
