using Dapper;
using EntiryModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess
{
    public interface IFinancialStatusRepository
    {
        IEnumerable<FinancialStatus> GetFinancialStatus();

       
    }
    public class FinancialStatusRepository : Repository<CategoryRepository>, IFinancialStatusRepository
    {
        
         SqlDataConnection sqlDataConnection;
            public FinancialStatusRepository(string connectionString)
            {
                sqlDataConnection = new SqlDataConnection(connectionString);
            }
            public IEnumerable<FinancialStatus> GetFinancialStatus()
            {
                var query = "MNT_Select_Financial_Status";
                var result = SqlMapper.Query<FinancialStatus>(sqlDataConnection.GetConnection(), query, null, commandType: CommandType.StoredProcedure);
                return result;
            }

                    


    }
}
