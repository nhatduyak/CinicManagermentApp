using System.Collections.Generic;

namespace ClinicManagement.Models
{
    public class MedicinesWhenToTake
    {
        public int ID{get;set;}

         public string Name{get;set;}

         public List<MedicinesWhenToTake> GetListMedicinesWhenToTake()
         {
            List<MedicinesWhenToTake> result=new List<MedicinesWhenToTake>();
            result.Add(new MedicinesWhenToTake(){ ID=1,Name="Sáng"});
            result.Add(new MedicinesWhenToTake(){ ID=2,Name="Trưa"});
            result.Add(new MedicinesWhenToTake(){ ID=3,Name="Chiều"});
            result.Add(new MedicinesWhenToTake(){ ID=4,Name="Sáng - Trưa"});
            result.Add(new MedicinesWhenToTake(){ ID=5,Name="Sáng - Chiều"});
            result.Add(new MedicinesWhenToTake(){ ID=6,Name="Trưa - Chiều"});
            result.Add(new MedicinesWhenToTake(){ ID=7,Name="Sáng - trưa - Chiều"});
            return result;
         }

    }
}