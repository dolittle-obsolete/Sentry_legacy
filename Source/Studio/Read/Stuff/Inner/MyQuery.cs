using System.Linq;
using Dolittle.Queries;

namespace Read.Stuff.Inner
{
    /// <summary>
    /// 
    /// </summary>
    public class MyQuery : IQueryFor<MyReadModel>
    {
        public MyQuery()
        {
            Query = new[] { new MyReadModel { Something = "Fourty Two" }}.AsQueryable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<MyReadModel> Query { get; }
    }
}