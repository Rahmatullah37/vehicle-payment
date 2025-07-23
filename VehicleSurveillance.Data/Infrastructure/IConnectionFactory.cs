using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSurveillance.Data.Infrastructure
{
    public interface IConnectionFactory
    {
        string ConnectionString { get; }
        NpgsqlConnection Connection { get; }
    }
}
