using Microsoft.Extensions.Configuration;
using MISA.WEB02.GD2.Core.Entities;
using MISA.WEB02.GD2.Core.Interfaces.Infrastructure;
using MISA.WEB02.GD2.Core.MISAAttribute;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MISA.WEB02.GD2.Core.Exceptions;

namespace MISA.WEB02.GD2.Infrastructure
{
    public class VendorRepository:BaseRepository<Vendor>, IVendorRepository
    {
        public VendorRepository(IConfiguration configuration) : base(configuration)
        {
        }


        public object GetVendorsById(Guid id)
        {
            

            using (NpgsqlConnection? conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var sqlCommand = @$"select *from vendor v
                                    left join (
	                                    select v.vendor_id as v_vendor_id , string_agg(vendor_group_id, ',') as vendor_groups
		                                    from vendor v
		                                    join 
			                                    (select vendor_group_assistant.vendor_id, vendor_group_assistant.vendor_group_id  from vendor_group_assistant) as vga 
		                                    on v.vendor_id  = vga.vendor_id
		                                    where v.vendor_id  = @vendor_id
		                                    group by v.vendor_id )
	                                    as mr
                                    on v.vendor_id  = mr.v_vendor_id where v.vendor_id = @vendor_id";

                object res = default;

                using (var cmd = new NpgsqlCommand(sqlCommand, conn))
                {
                    cmd.Parameters.AddWithValue("@vendor_id", id.ToString());

                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            dynamic entity = new ExpandoObject();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var propName = ToPascalCase(reader.GetName(i));
                                var propValue = reader.GetValue(i);

                                ((IDictionary<string, object>)entity).Add(propName, propValue);
                            }


                            res = JsonConvert.DeserializeObject<object>(JsonConvert.SerializeObject(entity));
                        }
                    }
                }
                conn.Close();
                return res;
            }
        }

        public string GetCustomTable()
        {
            var sqlString = $"select vendor_table_custom from table_custom where table_custom_id = '1'";
            using (NpgsqlConnection? conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(sqlString, conn))
                {
                    var res = cmd.ExecuteScalar();
                    return res.ToString();
                }
                conn.Close();

            }
        }
        public int UpdateCustomTable(string infor)
        {
            var sqlString = $"update table_custom set vendor_table_custom = @infor where table_custom_id = '1'";
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
        public int InsertVendor(Vendor vendor)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                string vendorId = string.Empty;
                int vendorRowInserted = 0;
                //tạo điểm đầu transaction
                NpgsqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand("", conn))
                    {
                        cmd.Transaction = transaction;
                        var props = typeof(Vendor).GetProperties();
                        var listCol = "";
                        var listValue = "";
                        //Lặp qua tất cả các props
                        for (int i = 0; i < props.Length; i++)
                        {
                            var prop = props[i];
                            var notMapProp = prop.GetCustomAttributes(typeof(NotMap), true);
                            //Nếu prop hiện tại cho phép map
                            if (notMapProp.Length <= 0)
                            {

                                var propName = prop.Name;
                                var propValue = prop.GetValue(vendor);
                                //Nếu là khoá chính => tạo value = newGuid
                                var isPrimaryKey = Attribute.IsDefined(prop, typeof(PrimaryKey));
                                if (isPrimaryKey == true && prop.PropertyType == typeof(Guid))
                                {
                                    propValue = Guid.NewGuid();
                                    vendorId = propValue.ToString();
                                }
                                //Tạo giá trị cho trường dùng để sắp xếp = ngày hiện tại
                                var isOrderByField = Attribute.IsDefined(prop, typeof(OrderBy));

                                if (isOrderByField)
                                {
                                    propValue = DateTime.Now;
                                }
                                    if (propValue != null)
                                {
                                    propName = ToSnakeCase(propName);
                                    listCol += $"{propName},";
                                    listValue += $"@{propName},";
                                    cmd.Parameters.Add(new NpgsqlParameter($"@{propName}", propValue));
                                }

                            }
                        }
                        listCol = listCol.Substring(0, listCol.Length - 1);
                        listValue = listValue.Substring(0, listValue.Length - 1);
                        var sqlString = $"insert into vendor({listCol}) values({listValue})";
                        cmd.CommandText = sqlString;
                        vendorRowInserted = cmd.ExecuteNonQuery();
                    }

                    int vendorAssRowInserted = 0;
                    List<Guid> listGroupsId;
                    if (vendor.VendorGroups is not null)
                    {
                        //Lấy danh sách các id nhóm nhà cung cấp
                        listGroupsId = new List<Guid>(vendor.VendorGroups);
                        string bodyString = "";
                        using (var cmd = new NpgsqlCommand("", conn))
                        {
                            cmd.Transaction = transaction;
                            var props = typeof(VendorGroupAssistant).GetProperties();
                            //Lặp qua mảng id, add vào sql string
                            for (int i = 0; i < listGroupsId.Count; i++)
                            {
                                bodyString += $"(@vendorGroupsAss{i}, ";
                                cmd.Parameters.AddWithValue($"@vendorGroupsAss{i}", Guid.NewGuid().ToString());
                                bodyString += $"@vendorGroupId{i},";
                                cmd.Parameters.AddWithValue($"@vendorGroupId{i}", listGroupsId[i].ToString());
                                bodyString += $"@vendorid{i}),";
                                cmd.Parameters.AddWithValue($"@vendorid{i}", vendorId);

                            }
                            var sqlCommandString = $"insert into vendor_group_assistant values {bodyString.Substring(0, bodyString.Length - 1)}";
                            cmd.CommandText = sqlCommandString;
                            vendorAssRowInserted = cmd.ExecuteNonQuery();
                        }

                    }
                    //commit nếu cả quá trình thành công
                    transaction.Commit();
                    return vendorAssRowInserted + vendorRowInserted;
                }
                catch (Exception ex)
                {
                    //rollback lại tất cả nếu gặp exception
                    transaction.Rollback();
                    var res = new
                    {
                        userMsg = Core.Properties.Resources.ExceptionMISA,
                        devMsg = ex.Message
                    };
                    throw new MISAValidateException(res);
                }
                
            }

            return 0;
        }
    }
}
