using System.Threading.Tasks;
using Quartz;

internal class MyOtherSyncJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        throw new System.NotImplementedException();
    }
}