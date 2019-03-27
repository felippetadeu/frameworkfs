using Framework.Enum;
using System.Collections.Generic;
using System.Reflection;

namespace Framework.DAO
{
    public class FilterObject<T>
    {
        public T Model { get; set; }
        public List<FilterProperty> Properties { get; set; } = new List<FilterProperty>();
    }

    public class FilterProperty
    {
        public string Property { get; set; }

        public OperatorEnum OperatorEnum { get; set; }

        public LogicalOperatorEnum LogicalOperatorEnum { get; set; }

        public string Operator
        {
            get
            {
                switch (OperatorEnum)
                {
                    case OperatorEnum.EqualTo:
                        return " = ";
                    case OperatorEnum.GreaterThan:
                        return " > ";
                    case OperatorEnum.LessThan:
                        return " < ";
                    case OperatorEnum.GreaterThanOrEqualTo:
                        return " >= ";
                    case OperatorEnum.LessThanOrEqualTo:
                        return " <= ";
                    case OperatorEnum.NotEqualTo:
                        return " <> ";
                    case OperatorEnum.Like:
                        return " Like ";
                    default:
                        return "";
                }
            }
        }

        public string LogicalOperator
        {
            get
            {
                switch (LogicalOperatorEnum)
                {
                    case LogicalOperatorEnum.And:
                        return " And ";
                    case LogicalOperatorEnum.Or:
                        return " Or ";
                    default:
                        return " ";
                }
            }
        }

        public string PreAppend { get; set; }

        public string PosAppend { get; set; }
    }
}
