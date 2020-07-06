using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.IO;
using EntiryModel;
using System.Data.OleDb;
using Dapper;
using System.Data;

namespace DataAccess.MsAccess
{
 
    public interface IMsAccessDataRepository
    {
        List<string> GetTableName(string fileLocation);
        bool ExecuteQuery(string query, string fileLocation);
        bool InitializeErrorLog(string psTableName, string fileLocation);
        List<PendingTransaction> GetTableRecords(string fileLocation, string table,bool isManuallImport=false);
        bool InsertErrorLog(string fileLocation, string tableName, string psErrDesc, string TableUID);
        
    }
    public class MsAccessDataRepository : IMsAccessDataRepository
    {
        public string _connectionString { get; set; }
        const string LogTable = "_ERRORLOG";
        OleDbConnection oledbConnection = null;
        OleDbCommand oledbCommand = null;
        OleDbDataReader oledbReader = null;        
        public MsAccessDataRepository(string connectionString)
        {
            _connectionString = connectionString;

        }

        public bool ExecuteQuery(string query, string fileLocation)
        {
            bool result = false;            
            try
            {
                if (!string.IsNullOrEmpty(query))
                {
                    var connectionString = string.Format(_connectionString,fileLocation);
                    using (oledbConnection = new OleDbConnection(connectionString))
                    {
                        oledbConnection.Open();
                        oledbCommand = new OleDbCommand(query, oledbConnection);
                        oledbCommand.ExecuteNonQuery();
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                throw ex;
            }
            finally
            {
                if (oledbConnection != null && oledbConnection.State == ConnectionState.Open) oledbConnection.Close();
                if (oledbReader != null) oledbReader.Close();
                if (oledbCommand != null)oledbCommand.Dispose();
                if (oledbConnection!= null) oledbConnection.Dispose();
            }
            return result;
        }
        public bool InitializeErrorLog(string psTableName, string fileLocation)
        {
            var connectionString = string.Format(_connectionString, fileLocation);
            bool lbExists = false;
            bool result = false;            
            StringBuilder Query = new StringBuilder();
            try
            {
                using (oledbConnection = new OleDbConnection(connectionString))
                {

                    oledbConnection.Open();
                    var schema = oledbConnection.GetSchema("Tables");
                    foreach (System.Data.DataRow row in schema.Rows)
                    {
                        var tableName = row["TABLE_NAME"].ToString();
                        //Exclude the system tables
                        if (!tableName.StartsWith("MSys"))
                        {
                            if (tableName.Trim().ToUpper() == (psTableName + LogTable).Trim().ToUpper())
                            {
                                lbExists = true;
                                break;
                            }
                        }
                    }
                    // If Table Exists Delete records
                    if (lbExists)
                    {
                        Query.Append("DELETE * FROM [" + psTableName + LogTable + "]");
                    }
                    else
                    {
                        Query.Append("CREATE TABLE [" + psTableName + "_ERRORLOG] (TransactionUID Number, OpenClosedFlag text, " + "TypeCode text, ReinsurerCode text, ContractCode text, SourceCode Number, CompanyCode text, " +"Product text, Coverage text, CededAssumedFlag text, TreatyFacFlag text, DisputedFlag text, " +"Pre1984Flag text, PaidLossCurrentAmt Currency DEFAULT 0, PaidLoss1to29Amt Currency DEFAULT 0, PaidLoss30to90Amt Currency DEFAULT 0, " +"PaidLoss91to120Amt Currency DEFAULT 0, PaidLossOver120Amt Currency DEFAULT 0, PaidLAECurrAmt Currency DEFAULT 0, " +"PaidLAE1to29Amt Currency DEFAULT 0, PaidLAE30to90Amt Currency DEFAULT 0, PaidLAE91to120Amt Currency DEFAULT 0, " +"PaidLAEOver120Amt Currency DEFAULT 0, WrittenPremiumAmt Currency DEFAULT 0, UnearnedPremiumAmt Currency DEFAULT 0, " +"CaseReserveAmt Currency DEFAULT 0, CaseLAEReserveAmt Currency DEFAULT 0, StatPaidALAEAmt Currency DEFAULT 0, StatPaidLossAmt Currency DEFAULT 0, " +"CommissionAmt Currency DEFAULT 0, PremiumPayableAmt Currency DEFAULT 0, Prior90DayCashAmt Currency DEFAULT 0, IBNRLossReserveAmt Currency DEFAULT 0, " +"IBNRSuppReserveAmt Currency DEFAULT 0, IBNRLAEReserveAmt Currency DEFAULT 0, FundsOnDepositWReinsAmt Currency DEFAULT 0, " +"AssetsPledgedForLOCAmt Currency DEFAULT 0, LetterOfCreditAmt Currency DEFAULT 0, OtherAllowedOffsetAmt Currency DEFAULT 0, " +"MiscBalanceAmt Currency DEFAULT 0, CreationDate DateTime, CreationLogonID text, Source Text, OperId text, " +"Ledger text, CnaReProductSw text, PsUID text, ProCedeClaimUID Number, FacultativeCode text, ClaimFileNumber text, " +"ClaimSuffix text, BusinessUnit text,ClaimProgramCode text, PolicyNumber text, PRPT text, MLSL text, " +"JVNumber text, KindOfLoss text, ClaimTransactionID Number, TableSourceCode Number, TableSourceRecID Number, " +"PeoplesoftSource text, ValidProduct text, ValidReinsurer text, ValidContract text, TableLoadType text, TableUID counter, ErrMessage text, " +"APFlag Text, NICOOffsetFlag Text, OriginalReinsurerCode Text, ContractCodeFAC Text,ContractUnderwritingYear Number,ContractLayerEffectiveDate DateTime, " +"ContractLayerNumber Number,CatastropheCode Text,LocationCodePart6 Text,CollateralDeferralEndDate DateTime,CertifiedRatingEffDt DateTime,CertifiedRecoRatingCd Text, " +"CertifiedPercent Number,OrigReinsCollateDeferralEndDt DateTime,OrigReinsCertifiedRatingEffDt DateTime,OrigReinsCertifiedRecoRatingCd Text,OrigReinsCertifiedPercent Number, " +"MultipleBeneficiaryAmt  Currency)");
                    }
                    if (!string.IsNullOrEmpty(Query.ToString()))
                    {

                        oledbCommand = new OleDbCommand(Query.ToString(), oledbConnection);
                        oledbCommand.ExecuteNonQuery();
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oledbConnection != null && oledbConnection.State == ConnectionState.Open) oledbConnection.Close();
                if (oledbReader != null) oledbReader.Close();
                if (oledbCommand != null) oledbCommand.Dispose();
                if (oledbConnection != null) oledbConnection.Dispose();
            }

            return result;
        }

        public List<PendingTransaction> GetTableRecords(string fileLocation, string table, bool isManuallImport = false)
        {
            var transactions = new List<PendingTransaction>();            
            var connectionString = string.Format(_connectionString, fileLocation);
            PendingTransaction _pendingTransaction = new PendingTransaction();
            try
            {
                using (oledbConnection = new OleDbConnection(connectionString))
                {
                    oledbConnection.Open();
                    if (!string.IsNullOrEmpty(table))
                    {
                        var sql = "SELECT * From[" + table + "] WHERE OpenClosedFlag = 'O'";
                        oledbCommand = new OleDbCommand(sql, oledbConnection);
                        oledbReader = oledbCommand.ExecuteReader();
                        while (oledbReader.Read())
                        {
                            if (isManuallImport) _pendingTransaction = MapAceesDataManual(oledbReader);
                            else _pendingTransaction = MapAceesData(oledbReader);
                            transactions.Add(_pendingTransaction);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oledbConnection != null && oledbConnection.State == ConnectionState.Open) oledbConnection.Close();
                if (oledbReader != null) oledbReader.Close();
                if (oledbCommand != null) oledbCommand.Dispose();
                if (oledbConnection != null) oledbConnection.Dispose();
            }
            return transactions;
        }
        
        public List<string> GetTableName(string fileLocation)
        {
            var connectionString = string.Format(_connectionString, fileLocation);
            List<string> tableNames = new List<string>();
            try
            {
                using (oledbConnection = new OleDbConnection(connectionString))
                {
                    oledbConnection.Open();
                    var schema = oledbConnection.GetSchema("Tables");
                    foreach (System.Data.DataRow row in schema.Rows)
                    {
                        var tableName = row["TABLE_NAME"].ToString();
                        //Exclude the system tables
                        if (!tableName.StartsWith("MSys"))
                        {
                            tableNames.Add(tableName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oledbConnection != null && oledbConnection.State == ConnectionState.Open) oledbConnection.Close();
                if (oledbReader != null) oledbReader.Close();
                if (oledbCommand != null) oledbCommand.Dispose();
                if (oledbConnection != null) oledbConnection.Dispose();
            }
            return tableNames;
        }
        public bool InsertErrorLog(string fileLocation, string tableName, string psErrDesc, string TableUID)
        {
            var connectionString = string.Format(_connectionString, fileLocation);
            string query;
            bool result = false;
            try
            {
                using (oledbConnection = new OleDbConnection(connectionString))
                {
                    oledbConnection.Open();
                    query = "INSERT INTO [" + tableName + LogTable + "] " + "SELECT * From  [" + tableName + "] " + "WHERE TableUID = " + TableUID;
                    oledbCommand = new OleDbCommand(query, oledbConnection);
                    var affectedRecords = oledbCommand.ExecuteNonQuery();
                    query = "UPDATE [" + tableName + LogTable + "] " + "SET ErrMessage = '" + psErrDesc + "' " + "WHERE TableUID = " + TableUID;
                    oledbCommand = new OleDbCommand(query, oledbConnection);
                    var affectedRecordsUpdates = oledbCommand.ExecuteNonQuery();
                    result = true;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (oledbConnection != null && oledbConnection.State == ConnectionState.Open) oledbConnection.Close();
                if (oledbReader != null) oledbReader.Close();
                if (oledbCommand != null) oledbCommand.Dispose();
                if (oledbConnection != null) oledbConnection.Dispose();
            }
            return result;
        }

        #region Private Mapping Method
        
        private PendingTransaction MapAceesData(OleDbDataReader reader)
        {
            PendingTransaction tran = new PendingTransaction();
            if (reader != null)
            {
                tran.TransactionUID = CheckNull.ConvertToInt32(reader["TransactionUID"]);
                tran.OpenClosedFlag = CheckNull.ConvertToString(reader["OpenClosedFlag"]);
                tran.TypeCode = CheckNull.ConvertToString(reader["TypeCode"]);
                tran.CededAssumedFlag = CheckNull.ConvertToString(reader["CededAssumedFlag"]);
                tran.DisputedFlag = CheckNull.ConvertToString(reader["DisputedFlag"]);
                tran.Pre1984Flag = CheckNull.ConvertToString(reader["Pre1984Flag"]);
                tran.CnaReProductSw = CheckNull.ConvertToString(reader["CnaReProductSw"]);
                tran.ReinsurerCode = CheckNull.ConvertToString(reader["ReinsurerCode"]);
                tran.ContractCode = CheckNull.ConvertToString(reader["ContractCode"]);
                tran.SourceCode = CheckNull.ConvertToInt32(reader["SourceCode"]);
                tran.CompanyCode = CheckNull.ConvertToString(reader["CompanyCode"]);
                tran.TableUID = CheckNull.ConvertToShort(reader["TableUID"]);
                tran.APFlag = CheckNull.ConvertToString(reader["APFlag"]);
                tran.NICOOffsetFlag = CheckNull.ConvertToString(reader["NICOOffsetFlag"]);
                tran.OriginalReinsurerCode = CheckNull.ConvertToString(reader["OriginalReinsurerCode"]);
                tran.ContractCodeFAC = CheckNull.ConvertToString(reader["ContractCodeFAC"]);
                tran.ContractUnderwritingYear = CheckNull.ConvertToShort(reader["ContractUnderwritingYear"]);
                tran.ContractLayerEffectiveDate = CheckNull.ConvertToDateTime(reader["ContractLayerEffectiveDate"]);
                tran.ContractLayerNumber = CheckNull.ConvertToShort(reader["ContractLayerNumber"]);
                tran.CatastropheCode = CheckNull.ConvertToString(reader["CatastropheCode"]);
                tran.LocationCodePart6 = CheckNull.ConvertToString(reader["LocationCodePart6"]);
                tran.CollateralDeferralEndDate = CheckNull.ConvertToDateTime(reader["CollateralDeferralEndDate"]);
                tran.CertifiedRatingEffDt = CheckNull.ConvertToDateTime(reader["CertifiedRatingEffDt"]);
                tran.CertifiedRecoRatingCd = CheckNull.ConvertToString(reader["CertifiedRecoRatingCd"]);
                tran.CertifiedPercent = CheckNull.ConvertToDouble(reader["CertifiedPercent"]);
                tran.OrigReinsCollateDeferralEndDt = CheckNull.ConvertToDateTime(reader["OrigReinsCollateDeferralEndDt"]);
                tran.OrigReinsCertifiedRatingEffDt = CheckNull.ConvertToDateTime(reader["OrigReinsCertifiedRatingEffDt"]);
                tran.OrigReinsCertifiedRecoRatingCd = CheckNull.ConvertToString(reader["OrigReinsCertifiedRecoRatingCd"]);
                tran.OrigReinsCertifiedPercent = CheckNull.ConvertToInt32(reader["OrigReinsCertifiedPercent"]);
                tran.MultipleBeneficiaryAmt = CheckNull.ConvertToDecimal(reader["MultipleBeneficiaryAmt"]);

            }

            return tran;
        }
        private PendingTransaction MapAceesDataManual(OleDbDataReader reader)
        {
            PendingTransaction tran = new PendingTransaction();
            if (reader != null)
            {
                
                // common
                tran.TransactionUID = CheckNull.ConvertToInt32(reader["TransactionUID"]); 
                tran.OpenClosedFlag = CheckNull.ConvertToString(reader["OpenClosedFlag"],"O"); 
                tran.TypeCode = CheckNull.ConvertToString(reader["TypeCode"],"M");
                tran.ReinsurerCode = CheckNull.ConvertToString(reader["ReinsurerCode"]);
                tran.CededAssumedFlag = CheckNull.ConvertToString(reader["CededAssumedFlag"]);
                tran.ContractCode = CheckNull.ConvertToString(reader["ContractCode"]);
                tran.SourceCode = CheckNull.ConvertToInt32(reader["SourceCode"]);
                tran.CompanyCode = CheckNull.ConvertToString(reader["CompanyCode"]);
                tran.DisputedFlag = CheckNull.ConvertToString(reader["DisputedFlag"]);
                tran.Pre1984Flag = CheckNull.ConvertToString(reader["Pre1984Flag"]);
                tran.CnaReProductSw = CheckNull.ConvertToString(reader["CnaReProductSw"]);
                tran.TableUID = CheckNull.ConvertToShort(reader["TableUID"]);
                tran.APFlag = CheckNull.ConvertToString(reader["APFlag"]);
                tran.NICOOffsetFlag = CheckNull.ConvertToString(reader["NICOOffsetFlag"]);
                tran.OriginalReinsurerCode = CheckNull.ConvertToString(reader["OriginalReinsurerCode"]);
                tran.ContractCodeFAC = CheckNull.ConvertToString(reader["ContractCodeFAC"]);
                tran.ContractUnderwritingYear = CheckNull.ConvertToShort(reader["ContractUnderwritingYear"]);
                tran.ContractLayerEffectiveDate = CheckNull.ConvertToDateTime(reader["ContractLayerEffectiveDate"]);
                tran.ContractLayerNumber = CheckNull.ConvertToShort(reader["ContractLayerNumber"]);
                tran.CatastropheCode = CheckNull.ConvertToString(reader["CatastropheCode"]);
                tran.LocationCodePart6 = CheckNull.ConvertToString(reader["LocationCodePart6"]);
                tran.CollateralDeferralEndDate = CheckNull.ConvertToDateTime(reader["CollateralDeferralEndDate"]);
                tran.CertifiedRatingEffDt = CheckNull.ConvertToDateTime(reader["CertifiedRatingEffDt"]);
                tran.CertifiedRecoRatingCd = CheckNull.ConvertToString(reader["CertifiedRecoRatingCd"]);
                tran.CertifiedPercent = CheckNull.ConvertToDouble(reader["CertifiedPercent"]);
                tran.OrigReinsCollateDeferralEndDt = CheckNull.ConvertToDateTime(reader["OrigReinsCollateDeferralEndDt"]);
                tran.OrigReinsCertifiedRatingEffDt = CheckNull.ConvertToDateTime(reader["OrigReinsCertifiedRatingEffDt"]);
                tran.OrigReinsCertifiedRecoRatingCd = CheckNull.ConvertToString(reader["OrigReinsCertifiedRecoRatingCd"]);
                tran.OrigReinsCertifiedPercent = CheckNull.ConvertToInt32(reader["OrigReinsCertifiedPercent"]);
                tran.MultipleBeneficiaryAmt = CheckNull.ConvertToDecimal(reader["MultipleBeneficiaryAmt"]);
                // specific to this method
                tran.Product = CheckNull.ConvertToString(reader["Product"]);
                tran.Coverage = CheckNull.ConvertToString(reader["Coverage"]);
                tran.TreatyFacFlag = CheckNull.ConvertToString(reader["TreatyFacFlag"]);
                tran.PaidLossCurrentAmt = CheckNull.ConvertToDecimal(reader["TreatyFacFlag"]);
                tran.PaidLoss1to29Amt = CheckNull.ConvertToDecimal(reader["PaidLoss1to29Amt"]);
                tran.PaidLoss30to90Amt = CheckNull.ConvertToDecimal(reader["PaidLoss30to90Amt"]);
                tran.PaidLoss91to120Amt = CheckNull.ConvertToDecimal(reader["PaidLoss91to120Amt"]);
                tran.PaidLossOver120Amt = CheckNull.ConvertToDecimal(reader["PaidLossOver120Amt"]);
                tran.PaidLAECurrAmt = CheckNull.ConvertToDecimal(reader["PaidLAECurrAmt"]);
                tran.PaidLAE1to29Amt = CheckNull.ConvertToDecimal(reader["PaidLAE1to29Amt"]);
                tran.PaidLAE30to90Amt = CheckNull.ConvertToDecimal(reader["PaidLAE30to90Amt"]);
                tran.PaidLAE91to120Amt = CheckNull.ConvertToDecimal(reader["PaidLAE91to120Amt"]);
                tran.PaidLAEOver120Amt = CheckNull.ConvertToDecimal(reader["PaidLAEOver120Amt"]);
                tran.WrittenPremiumAmt = CheckNull.ConvertToDecimal(reader["WrittenPremiumAmt"]);
                tran.UnearnedPremiumAmt = CheckNull.ConvertToDecimal(reader["UnearnedPremiumAmt"]);
                tran.CaseReserveAmt = CheckNull.ConvertToDecimal(reader["CaseReserveAmt"]);
                tran.CaseLAEReserveAmt = CheckNull.ConvertToDecimal(reader["CaseLAEReserveAmt"]);
                tran.StatPaidALAEAmt = CheckNull.ConvertToDecimal(reader["StatPaidALAEAmt"]);
                tran.StatPaidLossAmt = CheckNull.ConvertToDecimal(reader["StatPaidLossAmt"]);
                tran.CommissionAmt = CheckNull.ConvertToDecimal(reader["CommissionAmt"]);
                tran.PremiumPayableAmt = CheckNull.ConvertToDecimal(reader["PremiumPayableAmt"]);
                tran.Prior90DayCashAmt = CheckNull.ConvertToDecimal(reader["Prior90DayCashAmt"]);
                tran.IBNRLossReserveAmt = CheckNull.ConvertToDecimal(reader["IBNRLossReserveAmt"]);
                tran.IBNRSuppReserveAmt = CheckNull.ConvertToDecimal(reader["IBNRSuppReserveAmt"]);
                tran.IBNRLAEReserveAmt = CheckNull.ConvertToDecimal(reader["IBNRLAEReserveAmt"]);
                tran.FundsOnDepositWReinsAmt = CheckNull.ConvertToDecimal(reader["FundsOnDepositWReinsAmt"]);
                tran.AssetsPledgedForLOCAmt = CheckNull.ConvertToDecimal(reader["AssetsPledgedForLOCAmt"]);
                tran.LetterOfCreditAmt = CheckNull.ConvertToDecimal(reader["LetterOfCreditAmt"]);
                tran.OtherAllowedOffsetAmt = CheckNull.ConvertToDecimal(reader["OtherAllowedOffsetAmt"]);
                tran.MiscBalanceAmt = CheckNull.ConvertToDecimal(reader["MiscBalanceAmt"]);
                tran.CreationDate = CheckNull.ToDateTimeDefault(reader["CreationDate"]);
                tran.CreationLogonID = "SCH_CAE7093";
                tran.Source = CheckNull.ConvertToString(reader["Source"]);
                tran.OperId = CheckNull.ConvertToString(reader["OperId"]);
                tran.Ledger = CheckNull.ConvertToString(reader["Ledger"]);
                tran.PsUID = CheckNull.ConvertToString(reader["PsUID"]);
                tran.ProCedeClaimUID = CheckNull.ConvertToShort(reader["ProCedeClaimUID"]);
                tran.FacultativeCode = CheckNull.ConvertToString(reader["FacultativeCode"]);
                tran.ClaimFileNumber = CheckNull.ConvertToString(reader["ClaimFileNumber"]);
                tran.ClaimSuffix = CheckNull.ConvertToString(reader["ClaimSuffix"]);
                tran.BusinessUnit = CheckNull.ConvertToString(reader["BusinessUnit"]);
                tran.ClaimProgramCode = CheckNull.ConvertToString(reader["ClaimProgramCode"]);
                tran.PolicyNumber = CheckNull.ConvertToString(reader["PolicyNumber"]);
                tran.PRPT = CheckNull.ConvertToString(reader["PRPT"]);
                tran.MLSL = CheckNull.ConvertToString(reader["MLSL"]);
                tran.JVNumber = CheckNull.ConvertToString(reader["JVNumber"]);
                tran.KindOfLoss = CheckNull.ConvertToString(reader["KindOfLoss"]);
                tran.ClaimTransactionID = CheckNull.ConvertToShort(reader["ClaimTransactionID"]);
                tran.TableSourceCode = CheckNull.ConvertToShort(reader["TableSourceCode"]);
                tran.TableSourceRecID = CheckNull.ConvertToInt32(reader["TableSourceRecID"]);
                tran.PeoplesoftSource = CheckNull.ConvertToString(reader["PeoplesoftSource"]);
                tran.ValidProduct = CheckNull.ConvertToString(reader["ValidProduct"]);
                tran.ValidReinsurer = CheckNull.ConvertToString(reader["ValidReinsurer"]);
                tran.ValidContract = CheckNull.ConvertToString(reader["ValidContract"]);
                tran.TableLoadType = CheckNull.ConvertToString(reader["TableLoadType"]);
                




               
                

            }

            return tran;
        }



        #endregion


    }


}
