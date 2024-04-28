using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Common.Attributes
{
    public sealed class NotEmptyAttribute : RequiredAttribute
    {
        public override bool IsValid(object? value)
        {
            if (!base.IsValid(value))
            {
                return false;
            }

            if (value is IEnumerable enumerable)
            {
                return enumerable.GetEnumerator().MoveNext();
            }
            else
            {
                return true;
            }
        }
    }
}
