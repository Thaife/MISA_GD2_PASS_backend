using Microsoft.Extensions.Configuration;
using MISA.WEB02.GD2.Core.Entities;
using MISA.WEB02.GD2.Core.Interfaces.Infrastructure;
using MISA.WEB02.GD2.Core.MISAAttribute;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB02.GD2.Infrastructure
{
    public class VendorGroupAssistantRepository:BaseRepository<VendorGroupAssistant>, IVendorGroupAssistantRepository
    {
        public VendorGroupAssistantRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public int InsertMultiVendorGroupsAssistant(List<Guid>listIds, Guid vendorId) {
            string bodyString = "";
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("", conn))
                {
                    var props = typeof(VendorGroupAssistant).GetProperties();
                    for (int i = 0; i < listIds.Count; i++)
                    {
                        bodyString += $"(@vendorGroupsAss{i}, ";
                        cmd.Parameters.Add(new NpgsqlParameter($"@vendorGroupsAss{i}", Guid.NewGuid().ToString()));
                        bodyString += $"@vendorGroupId{i},";
                        cmd.Parameters.Add(new NpgsqlParameter($"@vendorGroupId{i}", listIds[i].ToString()));
                        bodyString += $"@vendorid{i}),";
                        cmd.Parameters.Add(new NpgsqlParameter($"@vendorid{i}", vendorId.ToString()));
                        


                    }
                    //cmd.Parameters.AddWithValue($"@{columnIdName}", entityId.ToString());
                    var sqlCommandString = $"insert into vendor_group_assistant values {bodyString.Substring(0, bodyString.Length - 1)}";
                    cmd.CommandText = sqlCommandString;
                    var res = cmd.ExecuteNonQuery();
                    return res;
                }
            }
        }
        public int DeleteMultiVendorGroupsAssistantByVendorId(Guid vendorId)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var sqlCommand = "delete from vendor_group_assistant where vendor_id = @vendorId";
                using(var cmd = new NpgsqlCommand(sqlCommand, conn))
                {
                    cmd.Parameters.AddWithValue($"@vendorId", vendorId.ToString());
                    var result = cmd.ExecuteNonQuery();
                    return result;
                }
            }
        }
    }
}
