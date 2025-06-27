using System;

namespace garage_managemet_backend_api.Models;


public class Appointment
	{
		
		public int Id { get; set; }
		public string CustomerName { get; set; }
		public string Vehicle { get; set; }
		public DateTime Date { get; set; }
		public string Mechanic { get; set; }
		public string Status { get; set; }
	
	}

	//public enum AppointmentStatus
	//{
	//	Pending=1,
	//	Confirmed=2,
	//	Completed=3,
	//	Cancelled=4
	//}

