using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public record DefaultDataTypeView(int Value1, string Value2)
    {
        public static explicit operator DefaultDataTypeView(DefaultDataType data)
        {
            return new DefaultDataTypeView(data.Value1, data.Value2);
        }

        public static explicit operator DefaultDataType(DefaultDataTypeView view)
        {
            return new DefaultDataType(view.Value1, view.Value2);
        }
    }
}