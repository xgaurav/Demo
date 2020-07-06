using Dapper;
using EntiryModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess
{
    public interface ICategoryRepository
    {
        IEnumerable<ReportIdentification> GetRptCategory();

        IEnumerable<ReportIdentification> GetReportsList(string rptName);
    }
    public class CategoryRepository : Repository<CategoryRepository>, ICategoryRepository
    {
        
         SqlDataConnection sqlDataConnection;
            public CategoryRepository(string connectionString)
            {
                sqlDataConnection = new SqlDataConnection(connectionString);
            }
            public IEnumerable<ReportIdentification> GetRptCategory()
            {
                var query = "SF_Select_Report_SubCategory";
                var result = SqlMapper.Query<ReportIdentification>(sqlDataConnection.GetConnection(), query, null, commandType: CommandType.StoredProcedure);
                return result;
            }

            public IEnumerable<ReportIdentification> GetReportsList(string rptName)
            {
                var query = "SF_Select_Reports";
                var param = new DynamicParameters();
                param.Add("@subcategory", rptName);
                var result = SqlMapper.Query<ReportIdentification>(sqlDataConnection.GetConnection(), query, param, commandType: CommandType.StoredProcedure);
                return result;
            }
        


    }
}
