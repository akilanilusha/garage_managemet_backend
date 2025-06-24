using System;
using Microsoft.AspNetCore.Mvc;
using garage_managemet_backend_api.Data;
using garage_managemet_backend_api.Services;
using garage_managemet_backend_api.Models;
using Microsoft.EntityFrameworkCore;
using garage_managemet_backend_api.controller.Services.Auth;

namespace garage_managemet_backend_api.controller;
{
	public class AppointmentController : ControllerBase 
	{
		[Route("api/[controller]")]
		[ApiController]
		[Authorize]

		private readonly AppDbContext _context;
	
		public AppointmentController(AppDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<AppointmentController>>> GetAppointment()
		{
				var appointments = await _context.Appointments.ToListAsync();
				return (appointments);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Appointment>> GetAppointment(int id)
		{
			var appointment = await _context.Appointments.FindAsync(id);
			if (appointment == null)
			{
				return NotFound();
			}
			return appointment;
		}

		[HttpPost]
		public async Task<ActionResult<Appointment>> CreateAppointment(Appointment appointment)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			_context.Appointments.Add(appointment);
			await _context.SaveChangesAsync();
			return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateAppointment(int id, Appointment appointment)
		{
			if (id != appointment.Id || !ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			_context.Entry(appointment).State = EntityState.Modified;
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!AppointmentExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}
			return NoContent();
    }
	[HttpDelete("{id}")]
	public async  Task<IActionResult> DeleteAppointment(int id)
	{
		var appointmnet = awit _context.Appointments.FindAsync(id);
		if(appointmnet == null)
		{
			return NotFound();
        }
		_context.Appoinmnets.Remove(appointmnet);
		await _context.SaveChangesAsync();
		return Ok(new { deleteAppointmentId = AppointmentController.Id });
    }
}
}

