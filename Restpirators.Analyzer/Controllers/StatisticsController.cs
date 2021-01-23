using Restpirators.Analyzer.DataAccess;
using Restpirators.Analyzer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restpirators.Analyzer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IEmergenciesRepository _emergenciesRepository;
        public StatisticsController(IEmergenciesRepository emergenciesRepository)
        {
            _emergenciesRepository = emergenciesRepository;
        }
        [HttpGet("{year}/{month}"), Description("Get quantity statistics")]
        public IActionResult CreateRoom(int year, int month)
        {
            try
            {
                return Ok(_emergenciesRepository.GetEmergencyQuantityStatistics(year == 0 ? (int?)null : year, month == 0 ? (int?)null : month));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("getTimeStatistics/{year}/{month}"), Description("Get time statistics")]
        public IActionResult GetTimeStatistics(int year, int month)
        {
            try
            {
                return Ok(_emergenciesRepository.GetEmergencyTimeStatistics(year == 0 ? (int?)null : year, month == 0 ? (int?)null : month));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet, Description("Get all history emergencies")]
        public IActionResult GetAllEmergencies()
        {
            try
            {
                return Ok(_emergenciesRepository.GetAllEmergencies());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
