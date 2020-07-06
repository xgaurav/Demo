using System;
using System.Collections.Generic;
using System.Text;

namespace EntiryModel
{
	public class Transactions
		{
			public decimal TransactionUID { get; set; }
			public string ReinsurerCode { get; set; }
			public string ContractCode { get; set; }
			public int SourceCode { get; set; }
			public string CompanyCode { get; set; }
			public string CededAssumedFlag { get; set; }
			public string DisputedFlag { get; set; }
			public string Pre1984Flag { get; set; }
			public decimal PaidLossCurrentAmt { get; set; }
			public decimal PaidLoss1to29Amt { get; set; }
			public decimal PaidLoss30to90Amt { get; set; }
			public decimal PaidLoss91to120Amt { get; set; }
			public decimal PaidLossOver120Amt { get; set; }
			public decimal PaidLAECurrAmt { get; set; }
			public decimal PaidLAE1to29Amt { get; set; }
			public decimal PaidLAE30to90Amt { get; set; }
			public decimal PaidLAE91to120Amt { get; set; }
			public decimal PaidLAEOver120Amt { get; set; }
			public decimal WrittenPremiumAmt { get; set; }
			public decimal UnearnedPremiumAmt { get; set; }
			public decimal CaseReserveAmt { get; set; }
			public decimal CaseLAEReserveAmt { get; set; }
			public decimal StatPaidALAEAmt { get; set; }
			public decimal StatPaidLossAmt { get; set; }
			public decimal CommissionAmt { get; set; }
			public decimal PremiumPayableAmt { get; set; }
			public decimal Prior90DayCashAmt { get; set; }
			public decimal IBNRLossReserveAmt { get; set; }
			public decimal IBNRSuppReserveAmt { get; set; }
			public decimal IBNRLAEReserveAmt { get; set; }
			public decimal FundsOnDepositWReinsAmt { get; set; }
			public decimal AssetsPledgedForLOCAmt { get; set; }
			public decimal LetterOfCreditAmt { get; set; }
			public decimal OtherAllowedOffsetAmt { get; set; }
			public decimal MiscBalanceAmt { get; set; }
			public DateTime CreationDate { get; set; }
			public string CreationLogonID { get; set; }
			public decimal PendingTransactionUID { get; set; }
			public string ProductCode { get; set; }
			public string PsUID { get; set; }
			public int ClaimTransactionID { get; set; }
			public string PolicyNumber { get; set; }
			public int ClaimMasterUID { get; set; }
			public int TableSourceRecID { get; set; }
			public string PeoplesoftSource { get; set; }
			public int TableSourceCode { get; set; }
			public string Coverage { get; set; }
			public string ManLdInd { get; set; }
			public decimal StageUID { get; set; }
			public string StageType { get; set; }
			public string APFlag { get; set; }
			public string NICOOffsetFlag { get; set; }
			public string OriginalReinsurerCode { get; set; }
			public string ContractCodeFAC { get; set; }
			public decimal ContractUnderwritingYear { get; set; }
			public DateTime ContractLayerEffectiveDate { get; set; }
			public decimal ContractLayerNumber { get; set; }
			public string CatastropheCode { get; set; }
			public string LocationCodePart6 { get; set; }
			public DateTime CollateralDeferralEndDate { get; set; }
			public DateTime CertifiedRatingEffDt { get; set; }
			public string CertifiedRecoRatingCd { get; set; }
			public decimal CertifiedPercent { get; set; }
			public DateTime OrigReinsCollateDeferralEndDt { get; set; }
			public DateTime OrigReinsCertifiedRatingEffDt { get; set; }
			public string OrigReinsCertifiedRecoRatingCd { get; set; }
			public decimal OrigReinsCertifiedPercent { get; set; }
			public decimal MultipleBeneficiaryAmt { get; set; }
		}
	}


