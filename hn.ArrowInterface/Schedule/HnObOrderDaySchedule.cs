using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using hn.ArrowInterface.Jobs;

namespace hn.ArrowInterface.Schedule
{
    public class HnObOrderDaySchedule
    {
        public static async Task DoWork()
        {
            var jobs = Assembly.GetExecutingAssembly().GetTypes().Where(p=>typeof(ISyncJob).IsAssignableFrom(p)&&!p.IsAbstract);

            foreach (var type in jobs)
            {
                var instance= System.Activator.CreateInstance(type);

                var method = type.GetMethod("Sync");

                var result= method.Invoke(instance, null);

            }
        }
    }
}