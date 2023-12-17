using BlazorApp.Application.Repositories.Interfaces;
using BlazorApp.Share.Entities;
using BlazorApp.Share.Enums;
using BlazorApp.Share.ValueObjects;

namespace BlazorApp.Application.Services;

public class DeviationService
{
    private readonly IDeviationRepository _deviationRepository;

    public DeviationService(IDeviationRepository deviationRepository)
    {
        _deviationRepository = deviationRepository;
    }

    public async Task<ResultDto<Deviation>> ValidateDeviation(Deviation deviation, Shift shift, bool isUpdate)
    {
        if (HasDeviation(deviation))
        {
            var returnData = new ResultDto<Deviation> { ErrorDetails = new Dictionary<string, List<string>>() };
            if (isUpdate && deviation.Id != 0)
            {
                var existingDeviation = await _deviationRepository.Get(deviation.Id.ToString());
                if (existingDeviation is null)
                {
                    returnData.ErrorDetails.Add(nameof(deviation.Id), new List<string> { "The DeviationId is not existing" });
                }
            }

            if (deviation.StartTime >= deviation.EndTime)
            {
                returnData.ErrorDetails.Add(nameof(deviation.StartTime), new List<string> { "The Start Time should be less than the End Time" });
            }

            if (string.IsNullOrWhiteSpace(deviation.Reason))
            {
                returnData.ErrorDetails.Add(nameof(deviation.Reason), new List<string> { "The Reason should not be null or empty" });
            }

            if (deviation.Status == DeviationStatus.None)
            {
                returnData.ErrorDetails.Add(nameof(deviation.Status), new List<string> { "The Status is invalid" });
            }

            ValidateDeviationTime(deviation, shift, returnData);

            if (returnData.ErrorDetails.Any())
            {
                returnData.IsError = true;
            }

            return returnData;
        }

        return new ResultDto<Deviation>();
    }

    private static void ValidateDeviationTime(Deviation deviation, Shift shift, ResultDto<Deviation> returnData)
    {
        switch (deviation.DeviationType)
        {
            case DeviationType.Illness:
                if (deviation.StartTime != shift.StartTime)
                {
                    returnData.ErrorDetails.Add(nameof(deviation.StartTime), new List<string> { "The Deviation StartTime is invalid" });
                }

                if (deviation.EndTime != shift.EndTime)
                {
                    returnData.ErrorDetails.Add(nameof(deviation.EndTime), new List<string> { "The Deviation EndTime is invalid" });
                }

                break;

            case DeviationType.Lateness:
                if (deviation.EndTime != shift.EndTime)
                {
                    returnData.ErrorDetails.Add(nameof(deviation.EndTime), new List<string> { "The Deviation EndTime is invalid" });
                }

                if (deviation.StartTime < shift.StartTime)
                {
                    returnData.ErrorDetails.Add(nameof(deviation.StartTime), new List<string> { "The Deviation StartTime is invalid" });
                }

                break;

            case DeviationType.EarlyLeave:
                if (deviation.StartTime != shift.StartTime)
                {
                    returnData.ErrorDetails.Add(nameof(deviation.StartTime), new List<string> { "The Deviation StartTime is invalid" });
                }

                if (deviation.EndTime > shift.EndTime)
                {
                    returnData.ErrorDetails.Add(nameof(deviation.EndTime), new List<string> { "The Deviation EndTime is invalid" });
                }

                break;
        }
    }

    public bool HasDeviation(Deviation deviation)
    {
        return deviation.DeviationType != DeviationType.None;
    }
}