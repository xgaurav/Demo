using System;
using System.Collections.Generic;
using System.Text;

namespace EntiryModel
{
    public class PendingTransaction
    {
        public int TransactionUID { get; set; }
        public string OpenClosedFlag{ get; set;}
        public string TypeCode{ get; set;}
        public string ReinsurerCode{ get; set;}
        public string ContractCode{ get; set;}
        public int SourceCode{ get; set;}
        public string CompanyCode{ get; set;}
        public string Product{ get; set;}
        public string Coverage{ get; set;}
        public string CededAssumedFlag{ get; set;}
        public string TreatyFacFlag{ get; set;}
        public string DisputedFlag{ get; set;}
        public string Pre1984Flag{ get; set;}
        public decimal PaidLossCurrentAmt{ get; set;}
        public decimal PaidLoss1to29Amt{ get; set;}
        public decimal PaidLoss30to90Amt{ get; set;}
        public decimal PaidLoss91to120Amt{ get; set;}
        public decimal PaidLossOver120Amt{ get; set;}
        public decimal PaidLAECurrAmt{ get; set;}
        public decimal PaidLAE1to29Amt{ get; set;}
        public decimal PaidLAE30to90Amt{ get; set;}
        public decimal PaidLAE91to120Amt{ get; set;}
        public decimal PaidLAEOver120Amt{ get; set;}
        public decimal WrittenPremiumAmt{ get; set;}
        public decimal UnearnedPremiumAmt{ get; set;}
        public decimal CaseReserveAmt{ get; set;}
        public decimal CaseLAEReserveAmt{ get; set;}
        public decimal StatPaidALAEAmt{ get; set;}
        public decimal StatPaidLossAmt{ get; set;}
        public decimal CommissionAmt{ get; set;}
        public decimal PremiumPayableAmt{ get; set;}
        public decimal Prior90DayCashAmt{ get; set;}
        public decimal IBNRLossReserveAmt{ get; set;}
        public decimal IBNRSuppReserveAmt{ get; set;}
        public decimal IBNRLAEReserveAmt{ get; set;}
        public decimal FundsOnDepositWReinsAmt{ get; set;}
        public decimal AssetsPledgedForLOCAmt{ get; set;}
        public decimal LetterOfCreditAmt{ get; set;}
        public decimal OtherAllowedOffsetAmt{ get; set;}
        public decimal MiscBalanceAmt{ get; set;}
        public DateTime CreationDate{ get; set;}
        public string CreationLogonID{ get; set;}
        public string Source{ get; set;}
        public string OperId{ get; set;}
        public string Ledger{ get; set;}
        public string CnaReProductSw{ get; set;}
        public string PsUID{ get; set;}
        public short ProCedeClaimUID{ get; set;}
        public string FacultativeCode{ get; set;}
        public string ClaimFileNumber{ get; set;}
        public string ClaimSuffix{ get; set;}
        public string BusinessUnit{ get; set;}
        public string ClaimProgramCode{ get; set;}
        public string PolicyNumber{ get; set;}
        public string PRPT{ get; set;}
        public string MLSL{ get; set;}
        public string JVNumber{ get; set;}
        public string KindOfLoss{ get; set;}
        public short ClaimTransactionID{ get; set;}
        public short TableSourceCode{ get; set;}
        // public miTableSourceRecID    As Integer
        public int TableSourceRecID{ get; set;}
        public string PeoplesoftSource{ get; set;}
        public string ValidProduct{ get; set;}
        public string ValidReinsurer{ get; set;}
        public string ValidContract{ get; set;}
        public string TableLoadType{ get; set;}
        public int TableUID{ get; set;}
        // NICO code changes - Begin
        public string APFlag{ get; set;}
        public string NICOOffsetFlag{ get; set;}
        public string OriginalReinsurerCode{ get; set;}
        // NICO code changes - End
        // BNR 304485 changes start
        public string ContractCodeFAC{ get; set;}
        public short ContractUnderwritingYear{ get; set;}
        public DateTime ContractLayerEffectiveDate{ get; set;}
        public short ContractLayerNumber{ get; set;}
        public string CatastropheCode{ get; set;}
        public string LocationCodePart6{ get; set;}
        public DateTime CollateralDeferralEndDate{ get; set;}
        public DateTime CertifiedRatingEffDt{ get; set;}
        public string CertifiedRecoRatingCd{ get; set;}
        // public mscertifiedPercent As Integer
        public double certifiedPercent{ get; set;} // decimal
        public DateTime OrigReinsCollateDeferralEndDt{ get; set;}
        public DateTime OrigReinsCertifiedRatingEffDt{ get; set;}
        public string OrigReinsCertifiedRecoRatingCd{ get; set;}
        public double OrigReinsCertifiedPercent{ get; set;}
        public double CertifiedPercent { get; set; }
        
        public decimal MultipleBeneficiaryAmt{ get; set;}
    }
}
