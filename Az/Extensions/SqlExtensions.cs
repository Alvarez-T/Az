using System.Globalization;

namespace Az.Extensions
{
    public static class SqlExtensions
    {
        public enum SqlData
        {
            ApenasData,
            ApenasHora,
            DataHora,
        }

        public static string Sql_Char_To_UUID(this string value)
        {
            return string.IsNullOrEmpty(value) ? "''" : $"CHAR_TO_UUID({value.ToSql()})";
        }

        public static string ToSql(this string value)
        {
            if (value == null)
                return "null";

            return "'" + value + "'";
        }

        public static string ToSql(this double value)
        {
            return value.ToString("0.00", CultureInfo.InvariantCulture);
        }

        public static string ToSql(this int value)
        {
            return value.ToString();
        }

        public static string ToSql(this long value)
        {
            return value.ToString();
        }

        public static string ToSql(this int? value)
        {
            if (value == null)
                return "null";

            return value.ToString();
        }

        public static string ToSql(this int value, bool nullIfZero)
        {
            if (value == 0 && nullIfZero)
                return "null";

            return value.ToString();
        }

        public static string ToSql(this DateTime value)
        {
            return value.ToSql(SqlData.DataHora);
        }

        public static string ToSql(this DateTime value, SqlData sqlData)
        {
            string result;

            if (value != default)
            {
                if (sqlData == SqlData.ApenasData)
                    result = "'" + value.ToString("MM/dd/yyyy") + "'";
                else if (sqlData == SqlData.ApenasHora)
                    result = $"'{value.ToString("HH:mm:ss")}'";
                else
                    result = "'" + value.ToString("MM/dd/yyyy HH:mm:ss") + "'";
            }
            else
            {
                result = "null";
            }

            return result;
        }

        public static string ToSql(this DateTime? value)
        {
            if (value == null)
                return "null";

            return ((DateTime)value).ToSql(SqlData.DataHora);
        }

        public static string ToSql(this DateTime? value, SqlData sqlData)
        {
            if (value == null)
                return "null";

            return ((DateTime)value).ToSql(sqlData);
        }

        public static string ToSql(this bool value)
        {
            return (value ? 1 : 0).ToString();
        }



        public static string ChangeField(this string sql, string fieldName, string value)
        {
            return sql.Replace(fieldName, value);
        }

        public static string ToSql(this int[] values)
        {
            return string.Join(", ", values);
        }

        public static string ToSql(this string[] values)
        {
            return string.Join(", ", values.ToSqlStringArray());
        }

        public static string[] ToSqlStringArray(this string[] values)
        {
            int index = 0;
            foreach (string value in values)
            {
                values[0] = $"'{value}'";
                index++;
            }

            return values;
        }
    }
}