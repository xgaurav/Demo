using Dapper;
using EntiryModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DataAccess
{
    public interface IImportExportRepository
    {
        IEnumerable<CompanyGroup> GetCompanyGroup();
        DateTime GetCloseDate();
        ErrorDetails ExecuteProcedure(string procedureName, DynamicParameters parameters);
        string GetErrorDescription(string errorCode);
    }
    public class ImportExportRepository :  IImportExportRepository
    {
        SqlDataConnection sqlDataConnection;
        public ImportExportRepository(string connectionString)
        {
            sqlDataConnection = new SqlDataConnection(connectionString);
        }
        public IEnumerable<CompanyGroup> GetCompanyGroup()
        {
            var query = "SF_Get_CompanyGroups";            
            var result = SqlMapper.Query<CompanyGroup>(sqlDataConnection.GetConnection(), query, commandType: CommandType.StoredProcedure);
            return result;
        }
        public ErrorDetails ExecuteProcedure(string procedureName, DynamicParameters parameters)
        {
            try
            {
                var result = SqlMapper.Query<ErrorDetails>(sqlDataConnection.GetConnection(), procedureName, parameters, commandType: CommandType.StoredProcedure, commandTimeout: 180).SingleOrDefault();
                return result;
            }
            catch(Exception ex)
            {
                throw ex;                
            }
        }
        public DateTime GetCloseDate()
        {
            DateTime? closeDate = null;
            var query = @"SELECT convert(varchar(4),datepart(mm,CloseDate))+'/'+convert(varchar(4)," + " datepart(dd,CloseDate))+'/'+convert(VARCHAR(4),datepart(yy,CloseDate)) as PreCloseDate from ProcessClose";
            var connection = sqlDataConnection.GetConnection();
            var result = connection.ExecuteScalar(sql : query,param: null, commandType: CommandType.Text);
            if(result!= null ) closeDate = Convert.ToDateTime(result);
            
            return closeDate.Value;            
        }

        public string GetErrorDescription(string errorCode)
        {
            string errorDescription = string.Empty;
            string query = "SELECT ErrorDesc " + "FROM MDBErrorDescriptions " + "WHERE  Errorcode = '" + errorCode.Trim() + "'";            
            var connection = sqlDataConnection.GetConnection();            
            var result = connection.ExecuteScalar(sql: query, param: null, commandType: CommandType.Text);
            if (result != null) errorDescription = Convert.ToString(result);
            return errorDescription;
        }
    }
}
