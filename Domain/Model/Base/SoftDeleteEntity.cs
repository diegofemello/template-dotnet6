using System;

namespace Domain.Model.Base
{
    public abstract class SoftDeleteEntity : BaseEntity
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
