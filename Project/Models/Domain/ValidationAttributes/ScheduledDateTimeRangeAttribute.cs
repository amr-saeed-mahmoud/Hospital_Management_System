namespace Models.Domain.ValidationAttributes;

using System.ComponentModel.DataAnnotations;

public class ScheduledDateTimeRangeAttribute : ValidationAttribute
{
    private readonly DateOnly _startDateTime;

    public ScheduledDateTimeRangeAttribute()
    {
        _startDateTime = DateOnly.FromDateTime(DateTime.Now);
    }

    public override bool IsValid(object? value)
    {
        if (value is DateOnly dateTime)
        {
            return dateTime >= _startDateTime && dateTime <= DateOnly.MaxValue;
        }

        return false;
    }



}