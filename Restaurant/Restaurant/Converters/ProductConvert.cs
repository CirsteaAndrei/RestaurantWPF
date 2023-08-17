using Restaurant.Models.Entities;
using Restaurant.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Restaurant.Converters
{
    class ProductConvert : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] != null && values[1] != null && values[2] != null)
            {
                var product = new Product()
                {
                    Name = values[0].ToString(),
                    isAvailable = (bool)values[2]
                };

                float price;
                if (float.TryParse(values[1].ToString(), out price))
                {
                    product.Price = price;
                }
                else
                {
                    // Handle the error, e.g., set a default value or throw an exception
                    product.Price = 0.0f;
                }

                return product;
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
