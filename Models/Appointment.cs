using System;

namespace garage_managemet_backend_api.Models;
{

public class Appointment
	{
		
		[Required]
		public int Id { get; set; }
		
		[Required]
		public DateTime Date { get; set; }
		public string Description { get; set; }
	
		[Required]
		public string Service { get; set; }
		public string Status { get; set; }
	
	}

	public enum AppointmentStatus
	{
		Pending=1,
		Confirmed=2,
		Completed=3,
		Cancelled=4
	}
}
