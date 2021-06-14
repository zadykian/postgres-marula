using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.PeriodicJobs.PublicApi
{
	/// <summary>
	/// Info about long-running job.
	/// </summary>
	public interface IJobInfo
	{
		/// <summary>
		/// Job's name.
		/// </summary>
		NonEmptyString Name { get; }

		/// <summary>
		/// Job's state.
		/// </summary>
		JobState State { get; }
	}

	/// <inheritdoc cref="IJobInfo"/>
	public record JobInfo(NonEmptyString Name, JobState State) : IJobInfo;
}