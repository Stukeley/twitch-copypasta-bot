using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchCopypastaBot.Models
{
	internal class Copypasta
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int Id { get; set; }

		[DefaultValue("")]
		public string Title { get; set; }

		public string Content { get; set; }

		public DateTime DateAdded { get; set; }
	}
}