using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using LockthreatCompliance.Extensions;


namespace LockthreatCompliance.TableTopExercises
{
   public class TableTopExerciseSectionAttachement : FullAuditedEntity<long>, IMayHaveTenant
    { 
        public TableTopExerciseSectionAttachement()
         {

         }

        public TableTopExerciseSectionAttachement(string fileName, string title = null)
        {
            
            Code = Guid.NewGuid().ToString() + "." + fileName.ReverseChars().GetUntil('.').ReverseChars();
            FileName = fileName;
            Title = title;
            
        }


        public int? TenantId { get; set; }
        public virtual string FileName { get; set; }
        public virtual string Title { get; set; }
        public virtual string Code { get; set; }      
        public virtual long  TableTopExerciseSectionId   { get; set; }
        public TableTopExerciseSection TableTopExerciseSection { get; set; }

    }
}
