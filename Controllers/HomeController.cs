using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsORM.Models;


namespace SportsORM.Controllers
{
    public class HomeController : Controller
    {

        private static Context _context;

        public HomeController(Context DBContext)
        {
            _context = DBContext;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.BaseballLeagues = _context.Leagues
                .Where(l => l.Sport.Contains("Baseball"))
                .ToList();
            return View();
        }

        [HttpGet("level_1")]
        public IActionResult Level1()
        {
            return View();
        }

        [HttpGet("level_2")]
        public IActionResult Level2()
        {
            ViewBag.AtlanticTeams = _context.Teams
                .Include(t => t.CurrLeague)
                .Where(t => t.CurrLeague.Sport.Contains("Soccer"))
                .ToList();

            ViewBag.BostonPenguinsPlayers = _context.Players
                .Include(p => p.CurrentTeam)
                .Where(p => p.CurrentTeam.Location.Contains("Boston"))
                .Where(p => p.CurrentTeam.TeamName.Contains("Penguins"))
                .ToList();

            ViewBag.CurrPlayersCollegiateBaseball = _context.Players
                .Include(p => p.CurrentTeam)
                .ThenInclude(t => t.CurrLeague)
                .Where(p => p.CurrentTeam.CurrLeague.Name.Contains("International Collegiate Baseball Conference"))
                .ToList();

            ViewBag.LopezPlayersAmateurFootball = _context.Players
                .Include(p => p.CurrentTeam.CurrLeague)
                .Where(p => p.LastName.Contains("Lopez"))
                .Where(p => p.CurrentTeam.CurrLeague.Name.Contains("American Conference of Amateur Football"))
                .ToList();

            ViewBag.AllfootballPlayers = _context.Players
                .Include(p => p.CurrentTeam.CurrLeague)
                .Where(p => p.CurrentTeam.CurrLeague.Sport.Contains("Football"))
                .ToList();

            ViewBag.SophiaTeams = _context.Teams
                .Where(t => t.CurrentPlayers.Any(p => p.FirstName.Contains("Sophia")))
                .ToList();

            ViewBag.SophiaLeagues = _context.Leagues
                .Where(l => l.Teams.Any(t => t.CurrentPlayers.Any(p => p.FirstName.Contains("Sophia"))))
                .ToList();

            ViewBag.FloresPlayersNotRoughRider = _context.Players
                .Include(p => p.CurrentTeam)
                .Where(p => p.LastName.Contains("Flores"))
                .Where(p => !p.CurrentTeam.TeamName.Contains("Roughriders"))
                .Where(p => !p.CurrentTeam.Location.Contains("Wishington"))
                .ToList();
            return View();
        }

        [HttpGet("level_3")]
        public IActionResult Level3()
        {
            ViewBag.AllSamEvansTeams = _context.Teams
                .Include(t => t.AllPlayers)
                .Where(t => t.AllPlayers.Any(p => p.PlayerOnTeam.FirstName.Contains("Samuel")))
                .Where(t => t.AllPlayers.Any(p => p.PlayerOnTeam.LastName.Contains("Evans")))
                .ToList();
                
            ViewBag.ManitobaTigerCats = _context.Players
                .Include(p => p.AllTeams)
                .Where(p => p.AllTeams.Any(t => t.TeamOfPlayer.TeamName.Contains("Tiger-Cats")))
                .Where(p => p.AllTeams.Any(t => t.TeamOfPlayer.Location.Contains("Manitoba")))
                .ToList();

            ViewBag.WichitaVikingsFormerPlayers = _context.Players
                .Include(p => p.AllTeams)
                    .ThenInclude(t => t.TeamOfPlayer)
                        .ThenInclude(t => t.AllPlayers)
                .Include(p => p.CurrentTeam)
                .Where(p => p.AllTeams.Any(t => t.TeamOfPlayer.TeamName.Contains("Vikings")))
                .Where(p => !p.CurrentTeam.TeamName.Contains("Vikings"))
                .ToList();
            
            ViewBag.JacobGrayFormerColtsTeams = _context.Teams
                .Include(t => t.AllPlayers)
                    .ThenInclude(p => p.PlayerOnTeam)
                .Where(t => t.AllPlayers.Any(p => p.PlayerOnTeam.FirstName.Contains("Jacob")))
                .Where(t => t.AllPlayers.Any(p => p.PlayerOnTeam.LastName.Contains("Gray")))
                .Where(t => t.CurrentPlayers.Any(p => !p.FirstName.Contains("Jacob")))
                .Where(t => t.CurrentPlayers.Any(p => !p.LastName.Contains("Gray")))
                .Where(t => !t.TeamName.Contains("Colts"))
                .ToList();

            ViewBag.JoshuaAtlantic = _context.Players
                .Include(p => p.AllTeams)
                    .ThenInclude(t => t.TeamOfPlayer)
                        .ThenInclude(tp => tp.CurrLeague)
                .Where(p => p.FirstName.Contains("Joshua"))
                .Where(p => p.AllTeams.Any(t => t.TeamOfPlayer.CurrLeague.Name.Contains("Atlantic Federation of Amateur")))
                .ToList();

            ViewBag.TeamsOfTwelveOrMorePlayers = _context.Teams
                .Where(t => t.AllPlayers.Count >= 12)
                .ToList();

            ViewBag.PlayersSortedByTeams = _context.Players
                .Include(p => p.AllTeams)
                .OrderBy(p => p.AllTeams.Count)
                .ToList();
            return View();
        }

    }
}