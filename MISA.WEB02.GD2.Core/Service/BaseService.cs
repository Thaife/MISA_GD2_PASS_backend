using MISA.WEB02.GD2.Core.Exceptions;
using MISA.WEB02.GD2.Core.Interfaces.Infrastructure;
using MISA.WEB02.GD2.Core.Interfaces.Service;
using MISA.WEB02.GD2.Core.MISAAttribute;
using MISA.WEB02.GD2.Core.FormatData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MISA.WEB02.GD2.Core.Entities;

namespace MISA.WEB02.GD2.Core.Service
{
    public class BaseService<T> : IBaseService<T>
    {
        List<object> errLstMsgs = new List<object>();
        IBaseRepository<T> _baseRepository;

        public BaseService(IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        public int? InsertService(T entity)
        {
            ValidateObject(entity);
            //Thực hiện thêm mới dữ liệu
            var res = _baseRepository.Insert(entity);
            return res;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int? UpdateService(T entity, Guid entityId)
        {
            //Validate dữ liệu
            ValidateObject(entity);

            //Thực hiện thêm mới dữ liệu
            var res = _baseRepository.Update(entityId, entity);
            return res;

        }

        public virtual void ValidateObject(T entity)
        {
            bool isValid = true;

            // Dữ liệu bắt buộc nhập
            var notEmptyProps = entity.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(NotEmpty)));
            if (notEmptyProps is not null)
            {
                foreach (var prop in notEmptyProps)
                {
                    var propValue = prop.GetValue(entity);
                    //Nếu trường này null hoặc bỏ trống => add vào list err
                    if (propValue == null || string.IsNullOrEmpty(propValue.ToString()))
                        errLstMsgs.Add(new
                        {
                            field = prop.Name,
                            mess = Properties.Resources.ValidateNotEmptyMess
                        });
                }
            }


            // Dữ liệu là Alphabe
            var alphabetProps = entity.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(Alphabet)));
            if (alphabetProps is not null)
            {
                var regexAlphabet = new Regex(@"[0-9.-]");
                foreach (var prop in alphabetProps)
                {
                    var propValue = prop.GetValue(entity);
                    //Nếu người dùng nhập kí tự khác Alphabet => add vào list err
                    if (propValue is not null && regexAlphabet.IsMatch(propValue.ToString()))
                    {
                        errLstMsgs.Add(new
                        {
                            field = prop.Name,
                            mess = $"Trường này chỉ được nhập ký tự Alphabet"
                        });
                    }
                }
            }

            //dữ liệu không trùng lặp
            var notduplicateprops = entity.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(NotDuplicate)));
            foreach (var prop in notduplicateprops)
            {
                //truy cập database kiểm tra, nếu propvalue đã tồn tại => add vào list err
                string propValue = prop.GetValue(entity).ToString();
                var isduplicate = _baseRepository.CheckDuplicate(FormatString.ToSnakeCase(prop.Name), propValue);
                if (isduplicate)
                {
                    errLstMsgs.Add(new
                    {
                        field = prop.Name,
                        mess = $" {propValue} đã tồn tại"

                    });
                }
            }

            // dữ liệu là Email
            var emailProps = entity.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(Email)));
            if (alphabetProps is not null)
            {
                var regexEmail = new Regex(@"^\w+([.-]?\w+)*@\w+([.-]?\w+)*(.\w{2,3})+$");
                foreach (var prop in emailProps)
                {
                    var propValue = prop.GetValue(entity);
                    //Nếu người dùng nhập Email không hợp lệ => add vào list err
                    if (propValue is not null && !regexEmail.IsMatch(propValue.ToString()))
                    {
                        errLstMsgs.Add(new
                        {
                            field = prop.Name,
                            mess = Properties.Resources.ValidateEmailMess
                        });
                    }
                }
            }

            //Dữ liệu là số điện thoại đầu mạng Việt Nam
            var phoneNumberVNProps = entity.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(PhoneNumber)));
            if (phoneNumberVNProps is not null)
            {
                var regexPhoneNumber = new Regex(@"(84|0[3|5|7|8|9])+([0-9]{8}$)");
                foreach (var prop in phoneNumberVNProps)
                {
                    var propValue = prop.GetValue(entity);
                    //Nếu người dùng nhập số điện thoại không hợp lệ => add vào list err
                    if (propValue is not null && !regexPhoneNumber.IsMatch(propValue.ToString()))
                    {
                        errLstMsgs.Add(new
                        {
                            field = prop.Name,
                            mess = Properties.Resources.ValidatePhoneNMess
                        });
                    }
                }
            }

            #region kiem tra date
            //Dữ liệu Date không được lớn hơn ngày hiện tại
            //var dateProps = entity.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(Date)));
            //if (dateProps is not null)
            //{
            //    foreach (var prop in dateProps)
            //    {
            //        var propValue = prop.GetValue(entity);
            //        if (propValue is not null)
            //        {
            //            string[] arr = (propValue as String).Split("-");
            //            int y = Int32.Parse(arr[0]);
            //            int m = Int32.Parse(arr[1]);
            //            int d = Int32.Parse(arr[2]);
            //            DateTime propDate = new DateTime(y, m, d);
            //            DateTime localDate = DateTime.Now;
            //            if (propDate > localDate)
            //            {
            //                errLstMsgs.Add(new
            //                {
            //                    field = prop.Name,
            //                    mess = Properties.Resources.ValidateDateMess
            //                });
            //            }

            //        }
            //    }
            //}
            #endregion
            //Kiểm tra nếu có lỗi thì throw danh sách lỗi
            if (errLstMsgs.Count > 0)
            {
                isValid = false;
                var res = new
                {
                    userMsg = Properties.Resources.ValidateErrMsg,
                    errlst = errLstMsgs
                };
                throw new MISAValidateException(res);
            }

        }

        
    }
}
