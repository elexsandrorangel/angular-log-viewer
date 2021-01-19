using LogViewer.Repository.Contexts;
using LogViewer.Repository.Entities;
using LogViewer.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogViewer.Repository.Implementations
{
    public class AccessLogRepository : BaseRepository<AccessLog>, IAccessLogRepository
    {
        public AccessLogRepository(LogViewerContext context) : base(context)
        {
        }

        public override Task<IEnumerable<AccessLog>> GetAsync(int page = 0, int qty = int.MaxValue, bool track = false)
        {
            return GetActiveAsync(page, qty, track);
        }
    }
}
