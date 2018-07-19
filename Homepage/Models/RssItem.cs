using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Rewrite.Internal;

namespace Homepage.Models
{
	public class RssItem
	{
		public string Title { get; set; }

		public string Author { get; set; }

		public DateTime DatePublished { get; set; }

		public string Description { get; set; }

		public string Url { get; set; }
    }
}
