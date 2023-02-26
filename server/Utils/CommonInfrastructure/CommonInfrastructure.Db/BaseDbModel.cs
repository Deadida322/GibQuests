using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CommonInfrastructure.Db
{
    public class BaseDbModel<T> where T : struct
    {
        /// <summary> Первичный ключ </summary>
        [Key, Column("id"), Comment("Первичный ключ")]
        public T Id { get; set; }

        /// <summary> Признак удаленности записи </summary>
        [Column("is_deleted"), Comment("Признак удаленности записи")]
        public bool IsDeleted { get; set; }
    }

    public class BaseDbModel : BaseDbModel<int>
    {
    }

}
