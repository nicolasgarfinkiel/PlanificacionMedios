using System;

namespace Irsa.PDM.Entities
{
    public class ServiceSync: EntityBase
    {
        public DateTime? LastBaseTablesSync { get; set; }

        public bool MustSync
        {
            get { return !LastBaseTablesSync.HasValue || (DateTime.Now - LastBaseTablesSync.Value).Hours > 3; }
        }
    }
}
