namespace IntegrationService.Models
{
	using IntegrationService.Controllers;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.RazorPages;
	using System.Collections.Generic;

	public class IndexModel : PageModel
	{
		private readonly MyController _controller;

		public IndexModel(MyController controller)
		{
			_controller = controller;
		}

		// Your other properties and methods...

		public IActionResult OnPost(List<TableRow> rows)
		{
			// Pass the rows to the controller's constructor
			var myControllerInstance = new MyController(rows);

			// Use the controller instance to perform further actions
			//myControllerInstance.DoSomething();

			return Page();
		}

		public class TableRow
		{
			public string Title { get; set; }
			public string FirstCell { get; set; }
			public string SecondCell { get; set; }
			public string ThirdCell { get; set; }
			public string FourthCell { get; set; }
			public bool IsHighlight { get; set; }
			public int Order { get; set; }
			public bool IsBold { get; set; }
			public string Note { get; set; }
		}
	}
}
