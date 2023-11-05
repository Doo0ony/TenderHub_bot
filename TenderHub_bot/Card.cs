using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenderHub_bot
{
	internal class Card
	{
		public int ID { get; set; }
		public string OrganizationName { get; set; }
		public string Description { get; set; }
		public string ProcurementType { get; set; }
		public string PurchaseName { get; set; }
		public decimal Summa { get; set; }
		public string PublicationDate { get; set; }
		public string Deadline { get; set; }
	}
}
