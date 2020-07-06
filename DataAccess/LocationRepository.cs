using Dapper;
using EntiryModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess
{
    public interface ILocationRepository
    {
        IEnumerable<Location> GetLocation();

       // IEnumerable<ReportIdentification> GetReportsList(string rptName);
    }
    public class LocationRepository : Repository<LocationRepository>, ILocationRepository
    {
        
            SqlDataConnection sqlDataConnection;
            public LocationRepository(string connectionString)
            {
                sqlDataConnection = new SqlDataConnection(connectionString);
            }
            public IEnumerable<Location> GetLocation()
            {
                var query = "SF_Select_Locations";
                var result = SqlMapper.Query<Location>(sqlDataConnection.GetConnection(), query, null, commandType: CommandType.StoredProcedure);
                return result;
            }

    }
}
