using NpgsqlTypes;

namespace APIDirecciones.Postgres.Utils
{
    public class ParameterPGsql
    {
        public string ParameterName { get; }

        public NpgsqlDbType ParameterType { get; }

        public object? Value { get; }

        public ParameterPGsql(string parameterName, NpgsqlDbType parameterType, object? value)
        {
            ParameterName = parameterName;
            ParameterType = parameterType;
            Value = value;
        }
    }
}