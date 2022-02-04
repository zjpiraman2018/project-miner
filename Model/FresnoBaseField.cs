/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 9/7/2015
 * Time: 7:30 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ProjMiner.Model
{
	/// <summary>
	/// Description of FresnoBaseField.
	/// </summary>
	public class FresnoBaseField
	{
	
		public string CourtDuns { get; set; }
		public string FilingNo { get; set; }            
		public string FilingDate   { get; set; }      
		public string FilingType   { get; set; }       
		public string DescCode    { get; set; }       
		public string LienType { get; set; }
		public string GovernmentType { get; set; }
		public string Status  { get; set; }
		public string StatusDate     { get; set; }   
		public string Amount { get; set; }
		public string NoOfDefendants { get; set; }
		public string NoOfPlaintiffs     { get; set; }         
		public string CauseOfAction1    { get; set; }       
		public string CauseOfAction2 { get; set; }
		public string CauseOfAction3 { get; set; }
		public string CauseOfActionAmount1 { get; set; }
		public string CauseOfActionAmount2{ get; set; }
		public string CauseOfActionAmount3 { get; set; }
		public string CaseRemedy1 { get; set; }
		public string CaseRemedy2 { get; set; }
		public string CaseRemedy3  { get; set; }
		public string CaseRemedyAmount1 { get; set; }
		public string CaseRemedyAmount2 { get; set; }
		public string CaseRemedyAmount3{ get; set; }
		public string CauseOfActionDescription{ get; set; }
		public string CollectedDate { get; set; }
		public string ReasonCode1 { get; set; }
		public string ReasonCode2 { get; set; }
		public string ReasonCode3{ get; set; }
		public string JudgmentType { get; set; }
		public string RecordNo { get; set; }


        public string Court { get; set; }

        // Image property
        public string Title { get; set; }
		public string partyRawHTML { get; set; }
		public string FinancialInformation { get; set; }
		public string EventInformation { get; set; }
        public string DispositionEventInfo { get; set; }
        public string DocumentsInfo { get; set; }

        public string CauseOfActionInfo { get; set; }
		public List<FresnoPartyField> Parties { get; set; }

		public FresnoBaseField()
		{
			Parties = new List<FresnoPartyField>();
            DispositionEventInfo = "";
            CauseOfActionInfo = "";
            DocumentsInfo = "";
			EventInformation = "";
			FinancialInformation = "";
			CourtDuns= "";
			FilingNo = "";            
			FilingDate   = "";      
			FilingType   = "";       
			DescCode    = "";       
			LienType = "";
			GovernmentType = "";
			Status  = "";
			StatusDate     = "";   
			Amount = "";
			NoOfDefendants = "";
			NoOfPlaintiffs     = "";         
			CauseOfAction1    = "";       
			CauseOfAction2 = "";
			CauseOfAction3 = "";
			CauseOfActionAmount1 = "";
			CauseOfActionAmount2= "";
			CauseOfActionAmount3 = "";
			CaseRemedy1 = "";
			CaseRemedy2 = "";
			CaseRemedy3  = "";
			CaseRemedyAmount1 = "";
			CaseRemedyAmount2 = "";
			CaseRemedyAmount3= "";
			CauseOfActionDescription= "";
			CollectedDate = "";
			ReasonCode1 = "";
			ReasonCode2 = "";
			ReasonCode3= "";
			JudgmentType = "";
			RecordNo = "";
			
			
			// image property
			Title = "";
            Court = "";

        }
	}
}
