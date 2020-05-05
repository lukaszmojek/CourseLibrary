using System;

namespace CourseLibrary.API.Helpers
{
    public static class DateTimeOffsetExtensions
    {
        public static int GetCurrentAge(this DateTimeOffset dateOfBirth, DateTimeOffset? dateOfDeath)
        {
            var dateToCalculateTo = DateTime.UtcNow;

            if (dateOfDeath != null)
            {
                dateToCalculateTo = dateOfDeath.Value.UtcDateTime;
            }

            var age = dateToCalculateTo.Year - dateOfBirth.Year;

            if (dateToCalculateTo < dateOfBirth.AddYears(age))
            {
                --age;
            }

            return age;
        }
    }
}
