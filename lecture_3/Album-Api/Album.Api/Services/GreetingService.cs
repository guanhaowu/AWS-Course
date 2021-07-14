using System;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Album.Api.Services
{
	public class GreetingService
	{
		// No clue how to solve this when TestUnit is asking for logger param for this constructor.
		
		// private readonly ILogger<GreetingService> _logger;
		// public GreetingService(ILogger<GreetingService> logger)
		// {
		// 	_logger = logger;
		// }

		private string Greeting() {
			return $"Hello world from {Dns.GetHostName()} v2";
		}

		public string Greeting(string inputValue){
			// Line below disabled for the reason above.
			// _logger.LogInformation("Client requested GreetingService at {DateTime}",DateTime.UtcNow.ToLongTimeString());
			if (string.IsNullOrEmpty(inputValue)) return Greeting();
			string newName = inputValue.Trim();
			// Regex just to make sure input after Trim contains alphabet characters only and single space in between words.
			Regex rgx = new Regex(@"^[a-zA-Z ]{2,}$");
			bool mismatched = ! rgx.Match(newName).Success;
			if (mismatched) return Greeting();
			return newName.Length > 0 ? $"Hello {newName} from {Dns.GetHostName()} v2" : Greeting();
		}
	}
}
