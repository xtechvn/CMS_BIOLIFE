using System.Collections.Generic;

namespace Entities.ViewModels
{
    public class GenericViewModel<TEntity> where TEntity : class
    {
        public List<TEntity> ListData { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public long TotalRecord { get; set; }
        public long TotalRecord1 { get; set; }
        public long TotalRecord2{ get; set; }
        public long TotalRecord3 { get; set; }
        public long TotalRecord4 { get; set; }
        public long TotalrecordErr { get; set; }

    }

    public class Paging
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public long TotalRecord { get; set; }
        public string PageAction { get; set; }
        public string PageSelectPageSize { get; set; }
        public string RecordName { get; set; }
    }
}
