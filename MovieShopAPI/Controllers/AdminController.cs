using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost]
        [Route("movie")]
        public async Task<IActionResult> CreateMovie(MovieRequestModel model)
        {
            var createMovie = await _adminService.CreateMovie(model);
            return Ok(createMovie);
        }

        [HttpPut]
        [Route("movie")]
        public async Task<IActionResult> UpdateMovie(MovieRequestModel model, int id)
        {
            var updateMovie = await _adminService.UpdateMovie(model, id);
            return Ok(updateMovie);
        }

        [HttpGet]
        [Route("top-purchased-movies")]
        public async Task<IActionResult> TopPurchasedMovies(DateTime? start = null, DateTime? end = null)
        {
            var TopPurchasedMovies = await _adminService.TopPurchasedMovie(start, end);
            if (TopPurchasedMovies == null)
            {
                return NotFound(new {error = "not found"});
            }

            return Ok(TopPurchasedMovies);
        }
    }
}