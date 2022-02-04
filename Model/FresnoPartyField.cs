
/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 9/7/2015
 * Time: 7:33 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace ProjMiner.Model
{
	/// <summary>
	/// Description of FresnoPartyField.
	/// </summary>
	public class FresnoPartyField
	{
	
		public string CourtDuns { get; set; }
		public string FileNumber { get; set; }
		public string FilingDate { get; set; }
		public string FilingType { get; set; }
		public string PartyType { get; set; }
		public string PartyName { get; set; }
		public string Fein { get; set; }
		public string DebtorAddress { get; set; }
		public string DebtorCity { get; set; }
		public string DebtorState { get; set; }
		public string DebtorZipCode { get; set; }
		public string RemedyAmount { get; set; }
		public string PartyCoa { get; set; }
		public string PartyRemedy { get; set; }
		
		
		public string RecordNo { get; set; }
		public string ActiveAttorney { get; set; }
		
		public FresnoPartyField()
		{
			
			ActiveAttorney = "";
			CourtDuns = "";
			FileNumber = "";
			FilingDate = "";
			FilingType = "";
			PartyType = "";
			PartyName = "";
			
			Fein = "";
			DebtorAddress = "";
			DebtorCity = "";
			DebtorState = "";
			DebtorZipCode = "";
			RemedyAmount = "";
			PartyCoa = "";
			PartyRemedy = "";
			
			RecordNo = "";
		}
	}
}
