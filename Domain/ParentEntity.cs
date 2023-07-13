using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public abstract class ParentEntity : ParentEntityWithoutUser
    {
        public long CreatedBy { get; protected set; }
        public long? UpdatedBy { get; protected set; }

     

    }



    public abstract class ParentEntityWithoutUser
    {
     
        public bool IsDeleted { get; protected set; }

        public DateTime CreationTime { get; protected set; }
        public DateTime? UpdateTime { get; protected set; }

    }
}
