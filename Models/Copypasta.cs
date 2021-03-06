﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitchCopypastaBot.Models
{
	public class Copypasta
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int Id { get; set; }

		[DefaultValue("")]
		public string Title { get; set; }

		public string Content { get; set; }

		public DateTime DateAdded { get; set; }

		// Where the pasta originally came from
		public string ChannelFrom { get; set; }

		// For easier filtering
		[DefaultValue("False")]
		public bool IsFavourite { get; set; }
	}
}