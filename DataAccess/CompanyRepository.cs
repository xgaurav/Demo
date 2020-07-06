using Dapper;
using EntiryModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess
{
    public interface ICompanyRepository
    {
        IEnumerable<CompanyCode> GetCompany();

       // IEnumerable<ReportIdentification> GetReportsList(string rptName);
    }
    public class CompanyRepository : Repository<CompanyRepository>, ICompanyRepository
    {
        
            SqlDataConnection sqlDataConnection;
            public CompanyRepository(string connectionString)
            {
                sqlDataConnection = new SqlDataConnection(connectionString);
            }
            public IEnumerable<CompanyCode> GetCompany()
            {
                var query = "MNT_Select_CompanyCode";
                var result = SqlMapper.Query<CompanyCode>(sqlDataConnection.GetConnection(), query, null, commandType: CommandType.StoredProcedure);
                return result;
            }

           
        


    }
}
