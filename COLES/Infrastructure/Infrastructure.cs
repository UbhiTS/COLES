using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace COLES.Infrastructure
{
    #region Infrastructure Classes

    public enum ExpressionOperator
    {
        [Description("&&")]
        And,
        [Description("||")]
        Or
    }

    public abstract class ExpressionElement
    {
        public override string ToString()
        {
            throw new NotImplementedException();
        }

        #region Operator Overloads

        public static ExpressionElement operator |(ExpressionElement e1, ExpressionElement e2)
        {
            if (e1 == null && e2 == null) return null;
            if (e1 == null) return e2;
            if (e2 == null) return e1;

            if (e1 is ExpressionGroup) return Or((ExpressionGroup)e1, e2);

            if (e1 is GenericCriteria) return Or((GenericCriteria)e1, e2);

            throw new ApplicationException("ExpressionElement Operator | (OR). Unknown error encountered.");
        }

        public static ExpressionElement operator &(ExpressionElement e1, ExpressionElement e2)
        {
            if (e1 == null && e2 == null) return null;
            if (e1 == null) return e2;
            if (e2 == null) return e1;

            if (e1 is ExpressionGroup) return And((ExpressionGroup)e1, e2);

            if (e1 is GenericCriteria) return And((GenericCriteria)e1, e2);

            throw new ApplicationException("ExpressionElement Operator & (AND). Unknown error encountered.");
        }

        #endregion

        #region Private Methods

        private static ExpressionElement Or(ExpressionGroup e1, ExpressionElement e2)
        {
            var retGroup = new ExpressionGroup(ExpressionOperator.Or);

            if (e2 is GenericCriteria)
            {
                if (e1.Operation == null || e1.Operation == ExpressionOperator.Or)
                {
                    retGroup.AddRange(e1.Elements);
                    retGroup.Add(e2);
                }
                else if (e1.Operation == ExpressionOperator.And)
                {
                    retGroup.Add(e1);
                    retGroup.Add(e2);
                }
            }
            else if (e2 is ExpressionGroup)
            {
                if (e1.Operation == ExpressionOperator.Or && ((ExpressionGroup)e2).Operation == ExpressionOperator.Or)
                {
                    retGroup.AddRange(e1.Elements);
                    retGroup.AddRange(((ExpressionGroup)e2).Elements);
                }
                else
                {
                    retGroup.Add(e1);
                    retGroup.Add(e2);
                }
            }

            return retGroup;
        }

        private static ExpressionElement Or(GenericCriteria g1, ExpressionElement e2)
        {
            var retGroup = new ExpressionGroup(ExpressionOperator.Or);

            if (e2 is GenericCriteria) // Criteria WITH Criteria
            {
                retGroup.Add(g1);
                retGroup.Add(e2);
            }
            else if (e2 is ExpressionGroup) // Criteria WITH ExpGroup
            {
                if (((ExpressionGroup)e2).Operation == null || ((ExpressionGroup)e2).Operation == ExpressionOperator.Or)
                {
                    retGroup.Add(g1);
                    retGroup.AddRange(((ExpressionGroup)e2).Elements);
                }
                else if (((ExpressionGroup)e2).Operation == ExpressionOperator.And)
                {
                    retGroup.Add(g1);
                    retGroup.Add(e2);
                }
            }

            return retGroup;
        }

        private static ExpressionElement And(ExpressionGroup e1, ExpressionElement e2)
        {
            var retGroup = new ExpressionGroup(ExpressionOperator.And);

            if (e2 is GenericCriteria)
            {
                if (e1.Operation == null || e1.Operation == ExpressionOperator.And)
                {
                    retGroup.AddRange(e1.Elements);
                    retGroup.Add(e2);
                }
                else if (e1.Operation == ExpressionOperator.Or)
                {
                    retGroup.Add(e1);
                    retGroup.Add(e2);
                }

            }
            else if (e2 is ExpressionGroup)
            {
                if (e1.Operation == ExpressionOperator.And && ((ExpressionGroup)e2).Operation == ExpressionOperator.And)
                {
                    retGroup.AddRange(e1.Elements);
                    retGroup.AddRange(((ExpressionGroup)e2).Elements);
                }
                else
                {
                    retGroup.Add(e1);
                    retGroup.Add(e2);
                }
            }

            return retGroup;
        }

        private static ExpressionElement And(GenericCriteria g1, ExpressionElement e2)
        {
            var retGroup = new ExpressionGroup(ExpressionOperator.And);

            if (e2 is GenericCriteria)
            {
                retGroup.Add(g1);
                retGroup.Add(e2);
            }
            else if (e2 is ExpressionGroup)
            {
                if (((ExpressionGroup)e2).Operation == null || ((ExpressionGroup)e2).Operation == ExpressionOperator.And)
                {
                    retGroup.Add(g1);
                    retGroup.AddRange(((ExpressionGroup)e2).Elements);
                }
                else if (((ExpressionGroup)e2).Operation == ExpressionOperator.Or)
                {
                    retGroup.Add(g1);
                    retGroup.Add(e2);
                }
            }

            return retGroup;
        }

        #endregion
    }

    public class ExpressionGroup : ExpressionElement
    {
        #region Constructors

        public ExpressionGroup(ExpressionOperator opr)
        {
            Elements = new List<ExpressionElement>();
            Operation = opr;
        }

        #endregion

        #region Public Members

        public ExpressionOperator? Operation { get; set; }

        public List<ExpressionElement> Elements { get; set; }

        #endregion

        #region Public Methods

        public bool HasItems()
        {
            return Elements.Any();
        }

        public void Add(ExpressionElement expression)
        {
            if (expression != null)
            {
                Elements.Add(expression);
            }
        }

        public void AddRange(IEnumerable<ExpressionElement> collection)
        {
            if (collection != null)
            {
                Elements.AddRange(collection);
            }
        }

        public void Insert(int index, ExpressionElement expression)
        {
            if (expression != null)
            {
                Elements.Insert(index, expression);
            }
        }

        public void InsertRange(int index, IEnumerable<ExpressionElement> collection)
        {
            if (collection != null)
            {
                Elements.InsertRange(index, collection);
            }
        }

        #endregion

        #region Interface Methods

        public override string ToString()
        {
            string grpString = String.Empty;

            if (Elements.Count > 1) grpString = "(";

            var ctr = 0;

            foreach (var expElement in Elements)
            {
                if (ctr == 0)
                {
                    grpString += expElement.ToString();
                }
                else
                {
                    if (Operation == null) throw new ApplicationException("GroupExpression Operation cannot be null when the object has multiple ExpressionElements");

                    grpString += " " + Operation.GetDescription() + " " + expElement.ToString();
                }

                ctr++;
            }

            if (Elements.Count > 1) grpString += ")";

            return grpString;
        }

        #endregion
    }

    #endregion
}
