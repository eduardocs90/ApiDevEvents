﻿
using AutoMapper;
using AwersomeDevEvents.API.Entities;
using AwersomeDevEvents.API.Models;
using AwersomeDevEvents.API.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AwersomeDevEvents.API.Controllers
{
    [Route("api/dev-events")]
    [ApiController]
    public class DevEventsController : ControllerBase
    {
        private readonly DevEventsDbContext _context;
        private readonly IMapper _mapper;
        public DevEventsController(
            DevEventsDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        /// <summary>
        /// Obter todos os eventos
        /// </summary>
        /// <returns>Coleção de eventos</returns>
        /// <response code="200">Sucesso</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var devEvents = _context.DevEvents.Include(de => de.Speakers).Where(d => !d.IsDeletd).ToList();

            var viewModel = _mapper.Map<List<DevEventViewModel>>(devEvents);
            return Ok(viewModel);
        }
        /// <summary>
        /// Obter um evento
        /// </summary>
        /// <param name="id">Identificador do evento</param>
        /// <returns>Dado do evento</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(Guid id)
        {
            var devEvent = _context.DevEvents
                .Include(de => de.Speakers)
                .SingleOrDefault(d => d.Id == id);
            if (devEvent == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<DevEventViewModel>(devEvent);


            return Ok(viewModel);
        }

        /// <summary>
        /// Cadastrar um evento
        /// </summary>
        /// <remarks>
        /// {"title": "string","description": "string","startDate": "2024-03-19T19:13:02.997Z","endDate": "2024-03-19T19:13:02.997Z"}
        /// </remarks>
        /// <param name="input">Dados do evento</param>
        /// <returns>Objeto recém-criado</returns>
        /// <response code="201">Sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Post(DevEventInputModel input)
        {
            var devEvent = _mapper.Map<DevEvent>(input);

            _context.DevEvents.Add(devEvent);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = devEvent.Id }, devEvent);

        }
        /// <summary>
        /// Atualizar um evento
        /// </summary>
        /// <remarks>
        /// {"title": "string","description": "string","startDate": "2024-03-19T19:13:02.997Z","endDate": "2024-03-19T19:13:02.997Z"}
        /// </remarks>
        /// <param name="id">Identificador de evento</param>
        /// <param name="input">Dados do evento</param>
        /// <returns>Nada</returns>
        /// <response code="404">Não encontrado</response>
        /// <response code="204">Sucesso</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Update(Guid id, DevEventInputModel input)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(d => d.Id == id);
            if (devEvent == null)
            {
                return NotFound();
            }
            devEvent.Update(input.Title, input.Description, input.StartDate, input.EndDate);
            _context.DevEvents.Update(devEvent);
            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Deletar um evento
        /// </summary>
        /// <param name="id">Identificador de evento</param>
        /// <returns>Nada</returns>
        /// <response code="404">Não encontrado</response>
        /// <response code="204">Sucesso</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public IActionResult Delete(Guid id)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(d => d.Id == id);
            if (devEvent == null)
            {
                return NotFound();
            }
            devEvent.Delete();
            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Cadastrar palestrante
        /// </summary>
        /// <remarks>
        /// {"name": "string", "talkTitle": "string", "talkDescription": "string", "linkedinProfile": "string"}
        /// </remarks>
        /// <param name="id">Identificador do evento</param>
        /// <param name="input">Dados do palestrante</param>
        /// <returns>Nada</returns>
        /// <response code="204">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        [HttpPost("{id}/speakers")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult PostSpeaker(Guid id, DevEventSpeakerInputModel input)
        {
            var speaker = _mapper.Map<DevEventSpeaker>(input);

            speaker.DevEventId = id;
            var devEvent = _context.DevEvents.Any(d => d.Id == id);
            if (!devEvent)
            {
                return NotFound();
            }

            _context.DevEventSpeakers.Add(speaker);
            _context.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// Obter todos os speakers
        /// </summary>
        /// <returns>Coleção de speakers</returns>
        /// <response code="200">Sucesso</response>
        [HttpGet("speakers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllSpeakers()
        {
            var speakers = _context.DevEventSpeakers;

            var viewModel = _mapper.Map<List<DevEventSpeakerViewModel>>(speakers);
            return Ok(viewModel);
        }


    }

}