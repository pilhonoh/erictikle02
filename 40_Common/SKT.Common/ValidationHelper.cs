using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SKT.Common
{
    public class ValidationHelper : IDisposable
    {
        public List<object> RequiredFieldValidateObject = new List<object>();
        public Dictionary<object, Type> CompareTypeValidateObject = new Dictionary<object, Type>();

        public bool Validate()
        {
            return RequiredFieldValidate() && CompareTypeValidate();
        }

        public void ClearAllValidateObject()
        {
            RequiredFieldValidateObject.Clear();
            CompareTypeValidateObject.Clear();
        }

        public bool RequiredFieldValidate()
        {
            bool result = true;
            foreach (object o in RequiredFieldValidateObject)
            {
                result = result && RequiredFieldValidate(o);
            }

            return result;
        }

        public bool RequiredFieldValidate(object o)
        {
            bool result = false;

            if (o.GetType().Equals(typeof(TextBox)))
            {
                result = !string.IsNullOrEmpty(((TextBox)o).Text);
            }
            else
            {
                result = !string.IsNullOrEmpty(Convert.ToString(o));
            }

            return result;
        }


        public bool CompareTypeValidate()
        {
            bool result = true;
            foreach (KeyValuePair<object, Type> kv in CompareTypeValidateObject)
            {
                result = result && CompareTypeValidate(kv.Key, kv.Value);
            }

            return result;
        }
        public bool CompareTypeValidate(object o, Type t)
        {
            bool result = false;

            try
            {
                if (o.GetType().Equals(typeof(TextBox)))
                {
                    Convert.ChangeType(((TextBox)o).Text, t);
                }
                else
                {
                    Convert.ChangeType(o, t);
                }
                result = true;
            }
            catch (Exception ex)
            {
                //형변환하려다 예외가 발생 하였으니... 같은 타입이 아니다..
            }

            return result;
        }

        public bool SpecificValueValidate(object o, object v)
        {
            bool result = false;

            if (o.GetType().Equals(typeof(string)))
            {
                result = Convert.ToString(o).Equals(Convert.ToString(v));
            }
            else
            {
                result = o.Equals(v);
            }

            return result;
        }

        public bool RangeValidate(object o)
        {
            //구현해야함
            bool result = false;
            return result;
        }

        public bool RegularExpressionValidate(Control c)
        {
            //구현해야함
            bool result = false;
            return result;
        }

        public void Dispose()
        {
            ;
        }

    }
}
