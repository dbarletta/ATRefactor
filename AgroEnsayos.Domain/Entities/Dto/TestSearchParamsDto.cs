using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AgroEnsayos.Domain.Entities.Dto
{
    public class TestSearchParamsDto
    {
        public int CategoryId { get; set; }
        public IEnumerable<int> Companies { get; private set; }
        public IEnumerable<string> Sources { get; private set; }
        public IEnumerable<string> Provinces { get; private set; }
        public IEnumerable<string> Localities { get; private set; }
        public IEnumerable<int> Campaigns { get; private set; }
        public string SearchTerm { get; set; }
        public Expression<Func<Test, bool>> AttributesPredicate { get; private set; }
    }
}
