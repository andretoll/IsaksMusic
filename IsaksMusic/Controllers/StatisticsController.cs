using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsaksMusic.Data;
using IsaksMusic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IsaksMusic.Controllers
{
    [Produces("application/json")]
    [Route("api/Statistics")]
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public StatisticsController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpPost]
        public async Task AddTrackCountAsync(int? id)
        {
            if (id != null)
            {
                int songId = (int)id;
                Statistics statistics = new Statistics()
                {
                    SongId = songId,
                    PlayedDate = DateTime.Now.Date
                };

                await _applicationDbContext.Statistics.AddAsync(statistics);
                await _applicationDbContext.SaveChangesAsync();
            }
        }
    }
}