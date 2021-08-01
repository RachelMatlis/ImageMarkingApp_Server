using DALContracts;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SQLInfraDAL
{
    public class DAL : IDAL
    {
        string _connetionString;
        public string ConnectionStr
        {
            get
            {
                return _connetionString;
            }
            set
            {
                _connetionString = value;
            }
        }

        public IParameter CreateParameter(string paramName, object value)
        {
            var retval = new SqlParameterAdapter();
            retval.Parameter = new SqlParameter(paramName, value);
            return retval as IParameter;
        }

        public DataSet ExecuteQuery(string spName, params IParameter[] parameters)
        {
            var commandSP = getCommand(spName, parameters);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(commandSP);

            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet);
            commandSP.Connection.Close();
            return dataSet;
        }
        public void ExecuteNonQuery(string spName, params IParameter[] parameters)
        {
            var commandSP = getCommand(spName, parameters);
            commandSP.ExecuteNonQuery();

            commandSP.Connection.Close();
        }

        private SqlCommand getCommand(string spName, params IParameter[] parameters)
        {
            var con = new SqlConnection(_connetionString);
            con.Open();
            SqlCommand commandSP = new SqlCommand();
            commandSP.CommandText = spName;
            commandSP.CommandType = CommandType.StoredProcedure;
            foreach (var parameter in parameters)
            {
                var paramAdapter = parameter as SqlParameterAdapter;
                commandSP.Parameters.Add(paramAdapter.Parameter);
            }

            commandSP.Connection = con;
            return commandSP;
        }
    }
}
