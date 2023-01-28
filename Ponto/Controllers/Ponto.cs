using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Ponto.DAL;
using Ponto.DAL.DAO;
using Ponto.DAL.Objects;
using Ponto.DTO;
using System.Globalization;
using System.Text.Json;

namespace Ponto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Ponto : ControllerBase
    {
        private readonly ILogger<Ponto> _logger;
        private BaseDAO<Register> _registerDAO;
        private BaseDAO<Entry> _entryDAO;

        public Ponto(ILogger<Ponto> logger, BaseDAO<Entry> entryDAO, BaseDAO<Register> registerDAO)
        {
            _logger = logger;
            _entryDAO = entryDAO;
            _registerDAO = registerDAO;
        }

        [HttpGet]
        [Route("get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult EntryGet(string name) 
        {
            List<EntryGetDTO> result = new List<EntryGetDTO>();

            var registers = _registerDAO.GetAll().Where(element =>
            element.EmployeeName!.ToLower().Trim().Equals(name.ToLower().Trim())
            );
            var entries = _entryDAO.GetAll();

            if(registers != null && registers.Any()) 
            {
                foreach(var register in registers)
                {
                    var entryGetEntry = new EntryGetDTO
                    { 
                        employeeName= register.EmployeeName,
                        punchDate = register.PunchDate,
                        Entries = new List<EntryDTO>()
                    };
                    var eventEntries = entries.Where(entry => entry.RegisterId == register.Id);
                    if(eventEntries == null || !eventEntries.Any()) 
                    { 
                        entryGetEntry.amountOfHoursWorked = "0.0";
                    }
                    else
                    {
                        double hoursWorked = 0;
                        DateTime? lastItem = null;
                        foreach(var entry in eventEntries)
                        {
                            string[] data = entry.PunchDateTime!.Split(' ');
                            string[] day = data[0].Split("/");
                            string[] hour= data[1].Split(":");
                            DateTime dateTime = new DateTime(
                                Convert.ToInt32(day[2]),
                                Convert.ToInt32(day[1]),
                                Convert.ToInt32(day[0]),
                                Convert.ToInt32(hour[0]),
                                Convert.ToInt32(hour[1]),
                                Convert.ToInt32(hour[2])
                                );
                            if(lastItem != null) 
                            {
                                hoursWorked = (dateTime - lastItem).Value.TotalHours;
                            }
                            lastItem = dateTime;
                            entryGetEntry.Entries.Add(
                                new EntryDTO
                                {
                                   punchDateTime = entry.PunchDateTime,
                                   punchType = entry.PunchType.Value
                                });
                        }

                        entryGetEntry.amountOfHoursWorked = String.Format("{0:0.0}", hoursWorked); ;
                    }
                    result.Add(entryGetEntry);
                }
            }

            string jsonSerialize = JsonSerializer.Serialize(result);
            return Ok(jsonSerialize);
        }

        [HttpPost]
        [Route("post")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult EntryPost(EntryPostDTO entryDTO)
        {
            string[] data = entryDTO.punchDateTime!.Split(' ');
            var register = _registerDAO.GetAll().LastOrDefault(element =>
            element.EmployeeName!.ToLower().Trim().Equals(entryDTO.employeeName!.ToLower().Trim())
            );
            if (register == null)
            {
                register = new Register
                {
                    EmployeeName = entryDTO.employeeName,
                    PunchDate = data[0]
                };

                _registerDAO.Create(register);
            }

            var entries = _entryDAO.GetAll().Where(entry => entry.RegisterId.Equals(register.Id));

            if (entries != null && entries.Any())
            {
                if (!entries.Last().PunchType.Equals(1) || !entryDTO.punchType.Equals(2))
                {
                    register = new Register
                    {
                        EmployeeName = entryDTO.employeeName,
                        PunchDate = data[0]
                    };

                    _registerDAO.Create(register);
                }
            }

            _entryDAO.Create(
                        new Entry
                        {
                            RegisterId = register.Id,
                            PunchDateTime = entryDTO.punchDateTime,
                            PunchType = entryDTO.punchType
                        });

            return CreatedAtAction(nameof(EntryGet), new {name = register.EmployeeName}, register);
        }
    }
}