using System.Threading.Tasks;
using Quartz;

internal class MySyncJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        throw new System.NotImplementedException();
    }
}