namespace SamplingAPI.Models.DataTransferModels;

/// <summary>
/// An estimator containing the estimate for the mean, variance, and confidence interval for a sample.
/// </summary>
/// <param name="Mean">The estimated mean.</param>
/// <param name="Variance">The estimated variance of the estimated mean.</param>
/// <param name="ConfidenceInterval">The confidence interval for the estimated mean.</param>
public record Estimator(double Mean, double Variance, ConfidenceInterval ConfidenceInterval);

/// <summary>
/// The confidence interval for an estimator.
/// </summary>
/// <param name="lowerBound">The lower bound of the confidence interval.</param>
/// <param name="upperBound">The upper bound of the confidence interval.</param>
/// <param name="SignificanceLevel">The significance level used to construct the confidence interval.</param>
public record struct ConfidenceInterval(double lowerBound, double upperBound, int SignificanceLevel);
