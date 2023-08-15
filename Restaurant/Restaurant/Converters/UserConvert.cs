using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Restaurant.Models.Entities;
using Restaurant.Models.Enums;

namespace Restaurant.Converters
{
    class UserConvert : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] != null && values[1] != null && values[2] != null && values[3] != null && values[4]!=null)
            {
                var user = new User()
                {
                    Employee = new Employee
                    {
                        FirstName = values[0].ToString(),
                        LastName = values[1].ToString(),
                        EmployeeRole = (EmployeeRole)values[2],
                    },
                    UserName = values[3].ToString(),
                    Password = values[4].ToString(),
                };
                return user;
            }
            else
            {
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
