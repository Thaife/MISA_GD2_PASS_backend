using Microsoft.Extensions.Configuration;
using MISA.WEB02.GD2.Core.Interfaces.Infrastructure;
using MISA.WEB02.GD2.Core.MISAAttribute;
using MySqlConnector;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.WEB02.GD2.Infrastructure
{
    public class BaseRepository<T> : IBaseRepository<T>
    {
        #region field and constructor
        protected string connectionString = String.Empty;
        protected string _tableName = ToSnakeCase(typeof(T).Name);
        public BaseRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("Misa");
        }
        #endregion
        //not woking
        public bool CheckDuplicate(string propName, string propValue)
        {
            // Tạo kết nối tới csdl postgres
            using (NpgsqlConnection? conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var isDuplicate = false;
                //Tạo cmd với query string null
                using (NpgsqlCommand cmd = new NpgsqlCommand("", conn))
                {
                    //Add Parameter
                    cmd.Parameters.AddWithValue($"@{propName}", propValue);
                    //Add CommandText
                    cmd.CommandText = $@"select {propName} from {_tableName} where {propName} = @{propName}";
                    //thực hiện truy vấn
                    var res = cmd.ExecuteScalar();
                    if(res != null)
                    {
                        isDuplicate = true;
                    }

                    return isDuplicate;
                }
                conn.Close();

            }
        }


        public IEnumerable<T> Get()
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var res = new List<T>();
                using (var cmd = new NpgsqlCommand($"SELECT * FROM {_tableName}", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        dynamic entity = new ExpandoObject();
                        //lst.Add(reader);
                        for (int i=0; i<reader.FieldCount; i++)
                        {
                            var propName = ToPascalCase(reader.GetName(i));
                            var propValue = reader.GetValue(i);
                            ((IDictionary<string, object>)entity).Add(propName, propValue);
                        }
                        T entityConverted = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(entity));
                        res.Add(entityConverted);
                        Console.WriteLine(reader.GetName);
                        Console.WriteLine(reader.GetString(0));
                    }
                conn.Close();
                return res;
            }

        }

        public T Get(Guid entityId)
        {
            var columnIdName = ToSnakeCase(_tableName)+ "_id";
            var sqlCommand = $"SELECT * FROM {_tableName} WHERE {columnIdName} = @id ";

            //khởi tạo sqlconnection
            using (NpgsqlConnection? conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                T res = default;
                //tạo sqlCommand
                using (var cmd = new NpgsqlCommand(sqlCommand, conn))
                {
                    cmd.Parameters.AddWithValue($"@id", entityId.ToString());

                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            dynamic entity = new ExpandoObject();
                            //lặp qua các prop => convert sang kiểu PascalCase
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var propName = ToPascalCase(reader.GetName(i));
                                var propValue = reader.GetValue(i);

                                ((IDictionary<string, object>)entity).Add(propName, propValue);
                            }

                            //Ép kiểu object => T
                            res = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(entity));
                        }
                    }
                }
                conn.Close();
                return res;
            }
        }

        public string? GetNewCode()
        {
            // định nghĩa sql query dữ liệu 
            var sqlString = @$"SELECT MAX(e.{_tableName}_code) FROM {_tableName} e where length(e.{_tableName}_code) = (select max(Length({_tableName}.{_tableName}_code)) from {_tableName})";
            // Tạo kết nối tới csdl postgres
            using (NpgsqlConnection? conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var newCode = string.Empty;
                
                using (NpgsqlCommand cmd = new NpgsqlCommand(sqlString, conn))
                {
                    var newcode = cmd.ExecuteScalar()?.ToString();
                    string[] temp = newcode?.Split("-");
                    int numberCode = Int32.Parse(temp[1]);
                    string nextCode = numberCode < 9 ? "0" + (numberCode + 1) : numberCode + 1 + "";
                    newcode = temp[0] + "-" + nextCode;


                    return newcode;

                }
                conn.Close();

                return newCode;
            }
            
        }

        public  object GetPaging(int PageSize, int PageNumber, string? textSearch)
        {
            string stringGetTotalRecord = $"select count(*) from {_tableName}";
            int totalRecord = 0;
            
            using (NpgsqlConnection? conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                //Lấy tổng số bản ghi
                using (NpgsqlCommand cmd = new NpgsqlCommand(stringGetTotalRecord, conn))
                {
                    totalRecord = int.Parse(cmd.ExecuteScalar().ToString());
                }
                int recordNumberSkiped = PageSize * (PageNumber-1);
                
                using (NpgsqlCommand cmd = new NpgsqlCommand("", conn))
                {
                    //Tạo chuỗi tìm kiếm
                    var props = typeof(T).GetProperties();
                    string body = " where ";
                    if(textSearch != null)
                    {
                        for (int i = 0; i < props.Length; i++)
                        {
                            var isNotMap = Attribute.IsDefined(props[i], typeof(AllowFilter));
                            if (isNotMap)
                            {
                                var propName = ToSnakeCase(props[i].Name);
                                body += $" {propName} ilike '%{textSearch}%' or";
                            }
                        }
                        //cmd.Parameters.AddWithValue("@text", textSearch);
                        body = body.Substring(0, body.Length - 2);
                    } else
                    {
                        body = "";
                    }
                    
                    //Tạo sqlCommand
                    string sqlString = $"select *from {_tableName} {body}  order by modified_date desc, length({_tableName}_code) desc ,{_tableName}_code desc limit {PageSize} offset {recordNumberSkiped}";
                    cmd.CommandText = sqlString;
                    using (var reader = cmd.ExecuteReader())
                    {
                        var res = new List<object>();
                        while (reader.Read())
                        {
                            dynamic entity = new ExpandoObject();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var propName = ToPascalCase(reader.GetName(i));
                                var propValue = reader.GetValue(i);
                                ((IDictionary<string, object>)entity).Add(propName, propValue);
                            }
                            res.Add(entity);
                        }
                        var result = new
                        {
                            totalRecord = textSearch == null ?  totalRecord : res.Count,
                            pageSize = PageSize,
                            pageNumber = PageNumber,
                            textSearch = textSearch,
                            data = res
                            
                        };
                        return result;
                        conn.Close();
                    }
                }
            }
            

        }

        public int Insert(T entity)
        {
            try
            {

                var funcName = $"func_insert_{_tableName}";
                using (NpgsqlConnection? conn = new NpgsqlConnection(connectionString))
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand(funcName, conn))
                    {
                        conn.Open();

                        cmd.CommandType = CommandType.StoredProcedure;

                        var properties = typeof(T).GetProperties();

                        foreach (var prop in properties)
                        {
                            //3.1. Lấy ra tên và value của property hiện tại
                            var propName = ToSnakeCase(prop.Name);
                            var propValue = prop.GetValue(entity);

                            var isPrimaryKey = prop.IsDefined(typeof(PrimaryKey), true);
                            var isNotMapped = prop.IsDefined(typeof(NotMap), true);
                            if (isPrimaryKey)
                            { 
                                  propValue = Guid.NewGuid();
                            }

                            if (!isNotMapped)
                            {
                                propValue = propValue == null ? DBNull.Value : propValue.ToString();
                                cmd.Parameters.AddWithValue($"_{propName}", propValue);
                            }
                        }

                        int rowsChange = (int)cmd.ExecuteScalar();
                        conn.Close();

                        return rowsChange;
                    }
                }
            }
            catch (Exception x)
            {

                throw;
            }
            
        }
        #region
        //public int Update(Guid entityId, T entity)
        //{
        //    // Lấy đối tượng đã có trong csdl (check trường nào thay đổi)
        //    var entityInDatabase = Get(entityId);

        //    // Định nghĩa tên function dùng trong DB
        //    var funcName = $"func_update_{_tableName}";
        //    var rowsAffected = 0;

        //    // Tạo kết nối tới csdl postgres
        //    using (NpgsqlConnection? conn = new NpgsqlConnection(connectionString))
        //    using (NpgsqlCommand cmd = new NpgsqlCommand(funcName, conn))
        //    {
        //        conn.Open();
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        var properties = typeof(T).GetProperties();

        //        foreach (var prop in properties)
        //        {
        //            // Lấy ra tên và value của property hiện tại
        //            var propName = ToSnakeCase(prop.Name);
        //            var propValue = prop.GetValue(entity);

        //            // kiểm tra có phải khóa chính
        //            var isPrimary = prop.IsDefined(typeof(PrimaryKey), true);

        //            if (isPrimary)
        //            {
        //                propValue = entityId;
        //            }
        //            // Không phải thì check giá trị có null
        //            else
        //            {
        //                if (propValue == null)
        //                {
        //                    // nếu giá trị truyền vào là null >> dùng giá trị của trường trong db
        //                    propValue = prop.GetValue(entityInDatabase);
        //                }
        //            }

        //            var isNotMapped = prop.IsDefined(typeof(NotMap), true);
        //            if (!isNotMapped)
        //            {
        //                propValue = propValue == null ? DBNull.Value : propValue.ToString();

        //                cmd.Parameters.AddWithValue($"m_{propName}", propValue);
        //            }
        //        }

        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                rowsAffected = reader.GetInt32(0);
        //            }
        //        }
        //        conn.Close();

        //        return rowsAffected;
        //    }
        //}

        #endregion


        //
        public int Update(Guid entityId, T entity)
        {
            try
            {
                var tableName = ToSnakeCase(_tableName);
                var columnIdName = $"{tableName}_id";

                var props = typeof(T).GetProperties();
                string bodyString = "";
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand("", conn))
                    {
                        
                        for (int i = 0; i < props.Length; i++)
                        {
                            var prop = props[i];
                            var propName = ToSnakeCase(prop.Name);
                            var propValue = prop.GetValue(entity);
                            var isOrderByField = Attribute.IsDefined(prop, typeof(OrderBy));

                            if (isOrderByField)
                            {
                                propValue = DateTime.Now;
                                bodyString += $" {propName} = @{propName},";
                                cmd.Parameters.Add(new NpgsqlParameter($"@{propName}", propValue));
                            } else
                            {
                                var isPrimaryKey = prop.GetCustomAttributes(typeof(PrimaryKey), true);
                                var isNotMap = prop.GetCustomAttributes(typeof(NotMap), true);
                                if (isPrimaryKey.Length == 0 && isNotMap.Length == 0 && propValue != null)
                                {
                                    propValue = propValue == null ? DBNull.Value : propValue;
                                    bodyString += $" {propName} = @{propName},";

                                    cmd.Parameters.Add(new NpgsqlParameter($"@{propName}", propValue));
                                }
                            }
                            

                        }
                        var sqlCommandString = $"UPDATE {tableName} SET {bodyString.Substring(0, bodyString.Length - 1)} WHERE {columnIdName} = '{entityId}'";
                        cmd.CommandText = sqlCommandString;
                        var res = cmd.ExecuteNonQuery();
                        return res;
                    }

                }

            }
            catch (Exception)
            {

                throw;
            }
            
        }

        #region
        //public int Delete(Guid entityId)
        //{
        //    var idColumnName = $"{_tableName}_id";
        //    var sqlCommand =
        //        $"WITH deleted AS (DELETE FROM {_tableName} WHERE {idColumnName} = @m_entity_id IS TRUE RETURNING *) " +
        //        $"SELECT COUNT(*) FROM deleted";
        //    using (NpgsqlConnection? conn = new NpgsqlConnection(connectionString))
        //    {
        //        conn.Open();

        //        int result = 0;

        //        using (var cmd = new NpgsqlCommand(sqlCommand, conn))
        //        {
        //            cmd.Parameters.AddWithValue($"@m_entity_id", entityId.ToString());

        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    result = reader.GetInt32(0);
        //                }
        //            }
        //        }
        //        conn.Close();

        //        return result;
        //    }
        //}
        #endregion
        public int Delete(Guid entityId)
        {
            using (NpgsqlConnection? conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var idColumnName = $"{_tableName}_id";
                var sqlCommand = $"delete from {_tableName} where {idColumnName} = @entityId";
                int result = 0;

                using (var cmd = new NpgsqlCommand(sqlCommand, conn))
                {
                    cmd.Parameters.AddWithValue($"@entityId", entityId.ToString());
                    result = cmd.ExecuteNonQuery();
                }

                return result;
            }
        }
        public int DeleteAll()
        {
            throw new NotImplementedException();
        }

        public int DeleteMulti(List<Guid> ids)
        {
            using(NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("", conn))
                {
                    var idColumnName = $"{_tableName}_id";
                    string bodySqlString = "";
                    for(int i = 0; i < ids.Count; i++)
                    {
                        bodySqlString += $" {idColumnName} = @id{i} or";
                        cmd.Parameters.AddWithValue($"@id{i}", ids[i].ToString());
                    }
                    string sqlString = $"delete from {_tableName} where  ({bodySqlString.Substring(0, bodySqlString.Length-2)})";
                    cmd.CommandText = sqlString;
                    int res = cmd.ExecuteNonQuery();

                    return res;
                }
            }
        }



        //not use
        public static string ToPascalCase(string str)
        {
            string result = Regex.Replace(str, "_[a-z]", delegate (Match m) {
                return m.ToString().TrimStart('_').ToUpper();
            });

            result = char.ToUpper(result[0]) + result.Substring(1);

            return result;
        }
        public static string ToSnakeCase(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (text.Length < 2)
            {
                return text;
            }
            var sb = new StringBuilder();
            sb.Append(char.ToLowerInvariant(text[0]));
            for (int i = 1; i < text.Length; ++i)
            {
                char c = text[i];
                if (char.IsUpper(c))
                {
                    sb.Append('_');
                    sb.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    
    }
}
