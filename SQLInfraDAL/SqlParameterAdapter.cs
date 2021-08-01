using DALContracts;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SQLInfraDAL
{
    class SqlParameterAdapter : IParameter
    {
        public SqlParameter Parameter { get; set; }
    }
}
