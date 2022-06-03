using Microsoft.Extensions.Configuration;
using MISA.WEB02.GD2.Core.Entities;
using MISA.WEB02.GD2.Core.Interfaces.Infrastructure;
using MISA.WEB02.GD2.Core.MISAAttribute;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.ComponentModel;
using System.Dynamic;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace MISA.WEB02.GD2.Infrastructure
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public IEnumerable<Payment> GetPayments()
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var res = new List<Payment>();
                using (var cmd = new NpgsqlCommand(@"select * from( 
                    (select vendor.vendor_id, vendor_code as account_object_code  from vendor) as v 
                    join
	                (select *from payment) as p 
                    on  v.vendor_id  = p.account_object_id)"
                    , conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        dynamic entity = new ExpandoObject();
                        //lst.Add(reader);
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var propName = ToPascalCase(reader.GetName(i));
                            var propValue = reader.GetValue(i);
                            ((IDictionary<string, object>)entity).Add(propName, propValue);
                        }
                        Payment entityConverted = JsonConvert.DeserializeObject<Payment>(JsonConvert.SerializeObject(entity));
                        res.Add(entityConverted);
                        Console.WriteLine(reader.GetName);
                        Console.WriteLine(reader.GetString(0));
                    }
                conn.Close();
                return res;
            }
        }

        public object GePaymentPaging(int PageSize, int PageNumber, string? textSearch)
        {
            string stringGetTotalRecord = $"select count(*) from payment";
            int totalRecord = 0;

            using (NpgsqlConnection? conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                //Lấy tổng số bản ghi
                using (NpgsqlCommand cmd = new NpgsqlCommand(stringGetTotalRecord, conn))
                {
                    totalRecord = int.Parse(cmd.ExecuteScalar().ToString());
                }
                int recordNumberSkiped = PageSize * (PageNumber - 1);

                using (NpgsqlCommand cmd = new NpgsqlCommand("", conn))
                {
                    //Tạo chuỗi tìm kiếm
                    var props = typeof(Payment).GetProperties();
                    string body = " where ";
                    //Nếu textSearch tồn tại => tìm kiếm theo tất cả các prop có [attrCustom = allowFilter]
                    if (textSearch != null)
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
                        body = body.Substring(0, body.Length - 2);
                    }
                    else
                    {
                        body = "";
                    }

                    //Tạo sqlCommand
                    string sqlString = @$"select * from( 
                    (select * from payment) as p
                    left join
                    (select vendor.vendor_id, vendor_code as account_object_code  from vendor) as v
                    on v.vendor_id = p.account_object_id) {body}
                    order by p.modified_date desc,length(payment_code) desc, p.payment_code desc
                    limit {PageSize} offset {recordNumberSkiped}";
                    cmd.CommandText = sqlString;
                    using (var reader = cmd.ExecuteReader())
                    {
                        var res = new List<object>();
                        while (reader.Read())
                        {
                            //dynamic entity = new ExpandoObject();
                            IDictionary<string, object> entity = new Dictionary<string, object>();
                            //lst.Add(reader);
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var propName = ToPascalCase(reader.GetName(i));
                                var propValue = reader.GetValue(i);
                                entity.Add(new KeyValuePair<string, object>(propName, propValue));
                            }
                            res.Add(entity);
                        }
                        var result = new
                        {
                            totalRecord = textSearch == null ? totalRecord : res.Count,
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
        public float GetTotalMoney(Guid paymentId)
        {
            var sqlString = $"select accountings from payment where payment_id = '{paymentId.ToString()}'";
            using (NpgsqlConnection? conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(sqlString, conn))
                {
                    var res = cmd.ExecuteScalar().ToString();
                    JArray a = JArray.Parse(res);
                    foreach (var item in a)
                    {
                        var x = item;
                        JsonSerializer serializer = new JsonSerializer();
                        var y = serializer.Deserialize(new JTokenReader(x), typeof(object));
                        
                    }
                    return 1;
                }
                conn.Close();

            }
        }
        public string GetCustomTable()
        {
            var sqlString = $"select payment_table_custom from table_custom where table_custom_id = '1'";
            using (NpgsqlConnection? conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(sqlString, conn))
                {
                    var res = cmd.ExecuteScalar().ToString();
                    return res;
                }
                conn.Close();

            }
        }
        public int UpdateCustomTable(string infor)
        {
            var sqlString = $"update table_custom set payment_table_custom = @infor where table_custom_id = '1'";
            using (NpgsqlConnection? conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(sqlString, conn))
                {
                    cmd.Parameters.AddWithValue("@infor", infor);
                    var res = cmd.ExecuteNonQuery();
                    return res;
                }
                conn.Close();

            }
        }

        //not working
        public int InsertPayment()
        {
            var tableName = typeof(Payment).Name;
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand($"INSERT INTO {tableName} (col1) VALUES ($1), ($2)", conn)
            {
                Parameters =
                {
                    new() { Value = "some_value" },
                    new() { Value = "some_other_value" }
                }
            };

            cmd.ExecuteNonQuery();
            return 0;
        }
    }
}