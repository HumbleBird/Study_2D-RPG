using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AccountServer.DB
{
	public enum ProviderType
	{
		Facebook = 1,
		Google = 2,
	}

	[Table("Account")]
	public class AccountDb
	{
		public int AccountDbId { get; set; }

		[Required]
		public string LoginProviderUserId { get; set; }
		[Required]
		public ProviderType LoginProviderType { get; set; }
	}
}
