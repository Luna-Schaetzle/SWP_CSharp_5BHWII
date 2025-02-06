using Klassenbiliothek_Article;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI_5BHWII_Grundlagen.Models.DB;

namespace WebAPI_5BHWII_Grundlagen.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // Vorbereitung (DB) - DI (Dependency Injection)
        private readonly DBManager _dbManager;

        // Hier wird vom DI-Container die von ihm erzeugte Instanz an den ctor übergeben und wir können auf die DB zugreifen.
        public UserController(DBManager dbManager)
        {
            _dbManager = dbManager;
        }

        // Wir schicken die Registrierungsdaten an die API und bekommen den API Key zurück
        [HttpPost("Register")]
        public IActionResult Register([FromBody] User user)
        {
            // Überprüfen, ob der User schon existiert
            if (_dbManager.Users.Any(u => u.Email == user.Email))
            {
                return BadRequest("User already exists");
            }

            // API Key generieren
            user.ApiKey = Guid.NewGuid().ToString();

            // User in DB speichern
            _dbManager.Users.Add(user);
            _dbManager.SaveChanges();

            return Ok(user.ApiKey);
        }

        // Überprüfen, ob der User existiert und den API Key zurückgeben
        // Wird nicht verwendet 
        [HttpGet("CheckAPIKey")]
        public IActionResult CheckAPIKey(string email, string password)
        {
            var user = _dbManager.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                return BadRequest("Email or Password is wrong");
            }

            return Ok(user.ApiKey);
        }

        // Neuer Endpunkt, um Benutzerdaten zu aktualisieren
        [HttpPut("UpdateUser/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            var user = _dbManager.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Aktualisieren der Benutzerdaten
            user.Name = updatedUser.Name;
            user.Password = updatedUser.Password;
            user.Birthdate = updatedUser.Birthdate;
            user.Street = updatedUser.Street;
            user.HouseNumber = updatedUser.HouseNumber;
            user.PostalCode = updatedUser.PostalCode;
            user.City = updatedUser.City;

            _dbManager.SaveChanges();

            return Ok("User updated successfully");
        }

        // Neuer Endpunkt, um Benutzerdaten zu löschen
        [HttpDelete("DeleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _dbManager.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            _dbManager.Users.Remove(user);
            _dbManager.SaveChanges();

            return Ok("User deleted successfully");
        }

        //User abfragen für den Login
        [HttpGet("Login")]
        public IActionResult Login(string name, string password)
        {
            var user = _dbManager.Users.FirstOrDefault(u => u.Name == name && u.Password == password);

            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            return Ok(user);
        }
    }
}