using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB02.GD2.Core.Interfaces.Infrastructure
{
    public interface IBaseRepository<T>
    {
        /// <summary>
        /// Lấy dữ liệu của 1 class
        /// Created by: Thai(10/5/2022)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> Get();

        /// <summary>
        /// Lấy dữ liệu 1 class bằng id
        /// Created by: Thai(10/5/2022)
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public T Get(Guid entityId);

        /// <summary>
        /// Lẫy mã mới
        /// </summary>
        /// <returns>max + 1</returns>
        public string? GetNewCode();

        /// <summary>
        /// Lấy dữ liệu phân trang
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public object GetPaging(int PageSize, int PageNumber, string? TextSearch);

        /// <summary>
        /// Thêm dữ liệu 1 class
        /// Created by: Thai(10/5/2022)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert(T entity);

        /// <summary>
        /// Sửa dữ liệu 1 class
        /// Created by: Thai(10/5/2022)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public int Update(Guid entityId, T entity);

        /// <summary>
        /// Xóa dữ liệu 1 class
        /// Created by: Thai(10/5/2022)
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public int Delete(Guid entityId);

        /// <summary>
        /// Xóa nhiều dữ liệu theo list id
        /// Created by: Thai(10/5/2022)
        /// </summary>
        /// <returns></returns>
        public int DeleteMulti(List<Guid> ids);

        /// <summary>
        /// Xóa toàn bộ dữ liệu của 1 class
        /// Created by: Thai(10/5/2022)
        /// </summary>
        /// <returns></returns>
        public int DeleteAll();


        /// <summary>
        /// Kiểm tra có trùng giá trị property k
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="propValue"></param>
        /// <returns>true-trùng; false-không trùng/returns></returns>
        bool CheckDuplicate(string propName, string propValue);
    }
}
