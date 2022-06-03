using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB02.GD2.Core.MISAAttribute
{
    /// <summary>
    /// Đánh dấu trường được chọn để sắp xếp
    /// </summary>
    public class OrderBy:Attribute { }

    /// <summary>
    /// Đánh dấu trường cho phép tìm kiếm
    /// </summary>
    public class AllowFilter:Attribute { }

    /// <summary>
    /// Đánh dấu để skip, omit
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class NotMap:Attribute
    {
    }

    /// <summary>
    /// Đánh dấu khóa chính
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class PrimaryKey : Attribute
    {
    }

    /// <summary>
    /// Đánh dấu không để trống
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class NotEmpty : Attribute
    {
    }

    /// <summary>
    /// Đánh dấu không trùng
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class NotDuplicate : Attribute
    {
    }

    /// <summary>
    /// Đánh dấu trường Email
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class Email : Attribute
    {
    }

    /// <summary>
    /// Đánh dấu Date
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class Date : Attribute
    {
    }

    /// <summary>
    /// Đánh dấu PhoneNumber
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class PhoneNumber : Attribute
    {
    }

    /// <summary>
    /// Đánh dấu chỉ sử dụng Alphabet
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class Alphabet : Attribute
    {
    }

    /// <summary>
    /// Đánh dấu là trường mã (code)
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class Code : Attribute
    {
    }
}
